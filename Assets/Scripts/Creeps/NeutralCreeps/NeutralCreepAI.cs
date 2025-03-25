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
    private float attackRange = 5f;
    List<GameObject> hostileUnits = new List<GameObject>();


    private void Start()
    {
        creepAgent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        // setting creep destination to the current target
        if (currentTarget != null)
        {
            Vector3 directionToTarget = currentTarget.position - transform.position;
            Vector3 stoppingPos = currentTarget.position - directionToTarget.normalized * stoppingDist;

            creepAgent.SetDestination(stoppingPos);
        }
    }

    // for if a unit comes within 240 (2.4) units of them (called in other script)
    public void FindAndSetCreepTarget()
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

    // for if a unit casts a damage spell or auto attack on them from within 1800 (18) range
    public void FindAndSetHostileCreepTarget()
    {
        NeutralCreepsCanAttack[] temp = FindObjectsByType<NeutralCreepsCanAttack>(FindObjectsSortMode.None);
        foreach (var scr in temp)
        {
            if (scr.hostile == true)
            {
                hostileUnits.Add(scr.gameObject);
            }
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
