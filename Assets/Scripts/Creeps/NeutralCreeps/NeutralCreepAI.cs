using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class NeutralCreepAI : MonoBehaviour, INavmeshSpeed
{
    private NavMeshAgent creepAgent;
    public Transform currentTarget = null;
    // also have hero combat script here at some point
    public float stoppingDist = 0.9f;
    private float attackRange = 3f;
    private bool cannotBeAggroed;
    [HideInInspector] public float loseAggroTimer = 0;
    private float offset = 0.3f; // half the creeps radius (ish)
    private Vector3 homeCamp; // set in code on spawn, needed for deaggro
    private float distFromTarget;
    [HideInInspector] public Coroutine slowCoroutine;

    List<GameObject> hostileUnits = new List<GameObject>();


    private void Awake()
    {
        creepAgent = GetComponent<NavMeshAgent>();

        stoppingDist += offset;
    }

    private void OnEnable()
    {
        homeCamp = transform.position;
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, homeCamp);

        if (dist <= 4)
        {
            loseAggroTimer = 0;
        }
        else
        {
            loseAggroTimer += Time.deltaTime;
            if (loseAggroTimer > 5)
            {
                // if timer more than 5 seconds return to camp and make so cant be aggroed for 3
                cannotBeAggroed = true;
                StartCoroutine(CantBeAggroedTimer());
            }
        }

        if (currentTarget != null && cannotBeAggroed == false)
        {
            distFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);

            if (distFromTarget > attackRange)
            {
                Vector3 directionToTarget = currentTarget.position - transform.position;
                Vector3 stoppingPos = currentTarget.position - directionToTarget.normalized * stoppingDist;

                creepAgent.SetDestination(stoppingPos);
            }
            else
            {
                creepAgent.SetDestination(transform.position);
            }
        }
    }

    IEnumerator CantBeAggroedTimer()
    {
        creepAgent.SetDestination(homeCamp);
        currentTarget = null;
        yield return new WaitForSecondsRealtime(3);
        cannotBeAggroed = false;
    }

    // for if a unit comes within 240 (2.4) units of them (called in RadialAggroCheck script)
    // for when another unit attacks (call in attacking units combat script)
    public void FindAndSetCreepTarget()
    {
        NeutralCreepsCanAttack[] temp = FindObjectsByType<NeutralCreepsCanAttack>(FindObjectsSortMode.None);
        foreach (var scr in temp) // any unit within radius will draw aggro, does not need to be hostile
        {
            hostileUnits.Add(scr.gameObject);
        }

        Transform closestEnemyCreep = GetClosestObjectInRadius(hostileUnits, attackRange);
        hostileUnits.Clear();
        if (closestEnemyCreep != null)
        {
            currentTarget = closestEnemyCreep;
            return;
        }

        if (currentTarget == null)
        {
            creepAgent.SetDestination(homeCamp);
        }
    }

    private Transform GetClosestObjectInRadius(List<GameObject> hostileUnits, float radius)
    {
        float closestDist = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPos = transform.position;

        foreach (GameObject t in hostileUnits)
        {
            float dist = Vector3.Distance(currentPos, t.transform.position);

            if (dist < closestDist && dist <= radius)
            {
                closestDist = dist;
                closestObject = t.transform;
            }
        }

        return closestObject;
    }

    public void ChangeNavAgentSpeed(float speedDebuff)
    {
        if (slowCoroutine == null)
        {
            slowCoroutine = StartCoroutine(SlowNavmeshAgent(speedDebuff));
        }
    }

    IEnumerator SlowNavmeshAgent(float speedDebuff)
    {
        // speedDebuff is the % that the speed needs to be reduced by
        float temp = creepAgent.speed;
        float thingToTimesBy = (100 - speedDebuff) / 100;
        float newSpeed = temp * thingToTimesBy;
        creepAgent.speed = newSpeed;

        yield return new WaitForSecondsRealtime(0.5f);
        creepAgent.speed = temp;

        slowCoroutine = null;
    }
}
