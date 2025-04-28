using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SiegeCreepTargetingAI : MonoBehaviour, INavmeshSpeed
{
    private NavMeshAgent creepAgent;
    public Transform currentTarget;
    public string enemyCreepTag;
    public string enemySiegeTag;
    public string enemyTowerTag;
    public float stoppingDist = 6f; // needs to be less than attack/aggro range
    public float attackRange = 6.9f;
    public float aggroRange = 8f;
    private float targetSwitchInterval = 0.5f;
    private float aggroSwapInterval = 3;// aggro swap of 3
    private float timeSinceLastTargetSwitch = 0f;
    Transform closestEnemy = null;
    private float distFromTarget;
    [HideInInspector] public Coroutine slowCoroutine;

    List<GameObject> cornerMarkers = new List<GameObject>();
    List<GameObject> cornerMarkersStore = new List<GameObject>();
    List<GameObject> towers = new List<GameObject>();


    void Start()
    {
        creepAgent = GetComponent<NavMeshAgent>();

        GameObject[] cornerMarkersA = GameObject.FindGameObjectsWithTag("CornerMarker");
        foreach (GameObject c in cornerMarkersA)
        {
            cornerMarkers.Add(c);
            cornerMarkersStore.Add(c);
        }
        GameObject[] towersA = GameObject.FindGameObjectsWithTag(enemyTowerTag);
        foreach (GameObject t in towersA)
        {
            towers.Add(t);
        }
    }

    public void OnEnable()
    {
        timeSinceLastTargetSwitch = aggroSwapInterval - 0.1f;

        foreach (GameObject c in cornerMarkersStore)
        {
            cornerMarkers.Add(c);
        }

        FindAndSetTarget();
        SetDestinationOfCreep();
    }

    private void Update()
    {
        // checking if any enemy units in range - different intervals depending on if has target
        AggroSwapTimer();

        // checks if this creep is close enough to the corner marker to go down the next lane correctly
        if (currentTarget != null && currentTarget.gameObject.tag == "CornerMarker")
        {
            float dist = Vector3.Distance(currentTarget.position, transform.position);
            if (dist < 7)
            {
                cornerMarkers = null;
                FindAndSetTarget();
                SetDestinationOfCreep();
            }
        }
    }

    private void FixedUpdate()
    {
        // checks the priority list for next target
        FindAndSetTarget();
    }

    //meow meow meow 

    void AggroSwapTimer()
    {
        // sets next target
        timeSinceLastTargetSwitch += Time.deltaTime;
        if (currentTarget == null)
        {
            if (timeSinceLastTargetSwitch > targetSwitchInterval)
            {
                SetDestinationOfCreep();
                timeSinceLastTargetSwitch = 0f;
            }
        }
        else
        {
            if (timeSinceLastTargetSwitch > aggroSwapInterval)
            {
                SetDestinationOfCreep();
                timeSinceLastTargetSwitch = 0f;
            }
        }
    }

    // setting creep destination to the current target, dont move if in targeting range
    public void SetDestinationOfCreep()
    {
        if (currentTarget != null)
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

    public void FindAndSetTarget()
    {
        // tower, siege unit, heroes/creeps
        // use player attack to set creep target to the player

        GameObject[] towersInRange = GameObject.FindGameObjectsWithTag(enemyTowerTag);
        closestEnemy = GetClosestObjectInRadius(towersInRange, aggroRange);  // closest tower
        if (closestEnemy == null)
        {
            GameObject[] siegeInRange = GameObject.FindGameObjectsWithTag(enemySiegeTag);
            closestEnemy = GetClosestObjectInRadius(siegeInRange, aggroRange); // closest siege creep
        }

        if (closestEnemy != null)
        {
            currentTarget = closestEnemy;
        }
        else
        {
            GameObject[] creeps = GameObject.FindGameObjectsWithTag(enemyCreepTag);
            if (creeps.Length > 0)
            {
                currentTarget = GetClosestObjectInRadius(creeps, aggroRange);  // if none, set to closest creep
                if (currentTarget == null)  // if no, set to structures (towers, etc..)
                {
                    if (cornerMarkers != null)
                    {
                        currentTarget = GetClosestObject(cornerMarkers); // if none and there are still corner markers
                    }
                    else
                    {
                        currentTarget = GetClosestObject(towers);
                    }
                }
            }
            else
            {
                if (cornerMarkers != null)
                {
                    currentTarget = GetClosestObject(cornerMarkers); // if none and there are still corner markers
                }
                else
                {
                    currentTarget = GetClosestObject(towers);
                }
            }
        }
    }

    private Transform GetClosestObject(List<GameObject> Things)
    {
        float closestDist = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPos = transform.position;

        foreach (GameObject t in Things)
        {
            float dist = Vector3.Distance(currentPos, t.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestObject = t.transform;
            }
        }

        return closestObject;
    }

    private Transform GetClosestObjectInRadius(GameObject[] Things, float radius)
    {
        float closestDist = 1000;
        Transform closestObject = null;
        Vector3 currentPos = transform.position;

        foreach (GameObject t in Things)
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
