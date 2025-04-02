using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaneCreepAI : MonoBehaviour
{
    private NavMeshAgent creepAgent;
    public Transform currentTarget;
    public string enemyCreepTag;
    public string enemySiegeTag;
    public string enemyTowerTag;
    public float stoppingDist = 0.9f;
    public float attackRange;
    public float aggroRange;
    private float targetSwitchInterval = 0.1f;
    private float aggroSwapInterval = 3;// aggro swap of 3
    private float timeSinceLastTargetSwitch = 0f;
    private float offset = 0.3f; // half the creeps radius (ish)
    Transform closestEnemyCreep = null;
    public float distFromTarget;
    GameObject[] cornerMarkers;
    GameObject[] towers;


    private void Start()
    {
        creepAgent = GetComponent<NavMeshAgent>();

        cornerMarkers = GameObject.FindGameObjectsWithTag("CornerMarkers");
        towers = GameObject.FindGameObjectsWithTag(enemyTowerTag);

        OffsetAllValuesForRadius();

        FindAndSetTarget();
        SetDestinationOfCreep();
    }

    private void Update()
    {
        // changes enemy destination target - different intervals depending on if has current target
        AggroSwapTimer();

        // checks if this creep is close enough to the corner marker to go down the next lane correctly
        if (cornerMarkers != null)
        {
            float dist = Vector3.Distance(currentTarget.position, transform.position);
            if (dist < 3)
            {
                cornerMarkers = null;
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

    void FindAndSetTarget()
    {
        // use player attack to set creep target to the player

        GameObject[] creeps = GameObject.FindGameObjectsWithTag(enemyCreepTag);
        closestEnemyCreep = GetClosestObjectInRadius(creeps, aggroRange);

        if (closestEnemyCreep != null)
        {
            currentTarget = closestEnemyCreep; // get closest creep and attak
        }
        else
        {
            GameObject[] siege = GameObject.FindGameObjectsWithTag(enemySiegeTag);
            if (siege.Length > 0)
            {
                currentTarget = GetClosestObjectInRadius(siege, aggroRange);  // if none, set to closest siege creep
            }
            else  // if no, set to structures (towers, etc..)
            {
                if (cornerMarkers != null)
                {
                    currentTarget = GetClosestObject(cornerMarkers); // if none and 
                }
                else
                {
                    currentTarget = GetClosestObject(towers);
                }
            }
        }
    }

    private Transform GetClosestObject(GameObject[] Things)
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

    void OffsetAllValuesForRadius()
    {
        stoppingDist += offset;
        attackRange += offset;
        aggroRange += offset;
    }
}
