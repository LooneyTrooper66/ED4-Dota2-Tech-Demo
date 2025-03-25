using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NeutralCreepAI : MonoBehaviour
{
    private NavMeshAgent creepAgent;
    public Transform currentTarget;
    public LaneCreepCombat[] laneCreepCombat;
    // also have hero combat script here at some point
    private float stoppingDist = 0.9f;
    private float aggroRange = 5f;
    List<GameObject> hostileUnits = new List<GameObject>();


    private void Start()
    {
        creepAgent = GetComponent<NavMeshAgent>();
    }


    #region

    // OK FUCKO, HERES THE PLAN

    // CREEPS FOLLOW AN EMPTY OBJECT DOWN THE LANES
    // HOWEVER
    // IF THEY ENCOUNTER ANY HOSTILE ENEMIES ON THE WAY
    // THE ENEMIES TAKE PRIORITY OVER THE EMPTY OBJECT
    // AND IF THEY FIND A TOWER, ATTACK IT IF THERES NO ENEMIES

    #endregion


    private void Update()
    {
        // target closest enemy within 500 units
        if (currentTarget == null)
        {
            // aggro ignores wether attacker is creep or hero, make sure this isnt biased.
            FindAndSetCreepTarget();
        }

        // setting creep destination to the current target
        if (currentTarget != null)
        {
            Vector3 directionToTarget = currentTarget.position - transform.position;
            Vector3 stoppingPos = currentTarget.position - directionToTarget.normalized * stoppingDist;

            creepAgent.SetDestination(stoppingPos);
        }
    }

    void FindAndSetCreepTarget()
    {
        NeutralCreepsCanAttack[] temp = FindObjectsByType<NeutralCreepsCanAttack>(FindObjectsSortMode.None);
        foreach (var obj in temp)
        {
            hostileUnits.Add(obj.gameObject);
        }

        Transform closestEnemyCreep = GetClosestObjectInRadius(hostileUnits, aggroRange);
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

        Debug.Log(closestObject);
        return closestObject;
    }
}
