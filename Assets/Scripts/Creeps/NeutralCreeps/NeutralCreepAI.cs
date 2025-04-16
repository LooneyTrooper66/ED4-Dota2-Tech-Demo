using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class NeutralCreepAI : MonoBehaviour
{
    private NavMeshAgent creepAgent;
    public Transform currentTarget = null;
    // also have hero combat script here at some point
    public float stoppingDist = 0.9f;
    private float attackRange = 5f;
    private bool deAggroTimerRunning;
    private bool cannotBeAggroed;
    private float loseAggroTimer = 0;
    private float offset = 0.3f; // half the creeps radius (ish)
    private Vector3 homeCamp; // set in code on spawn, needed for deaggro

    List<GameObject> hostileUnits = new List<GameObject>();


    private void Awake()
    {
        creepAgent = GetComponent<NavMeshAgent>();

        stoppingDist += offset;
        attackRange += offset;
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
            Vector3 directionToTarget = currentTarget.position - transform.position;
            Vector3 stoppingPos = currentTarget.position - directionToTarget.normalized * stoppingDist;

            creepAgent.SetDestination(stoppingPos);
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
        }
    }

    // for if a unit casts a damage spell or auto attack on them from within 1800 (18) range   
    // access this from the script of the attacking unit
    public void FindAndSetHostileCreepTarget()
    {
        NeutralCreepsCanAttack[] temp = FindObjectsByType<NeutralCreepsCanAttack>(FindObjectsSortMode.None);
        foreach (var scr in temp)
        {
            hostileUnits.Add(scr.gameObject);
        }

        Transform closestEnemyCreep = GetClosestObjectInRadius(hostileUnits, attackRange);
        hostileUnits.Clear();
        if (closestEnemyCreep != null)
        {
            currentTarget = closestEnemyCreep;
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
}
