using UnityEngine;
using UnityEngine.AI;

public class LaneCreepAI : MonoBehaviour
{
    private NavMeshAgent creepAgent;
    private Transform currentTarget;
    public string enemyCreepTag;
    public string enemyTowerTag;
    public float stoppingDist = 2f;
    public float aggroRange = 5f;
    public float targetSwitchInterval = 2f;
    private float timeSinceLastTargetSwitch = 0f;


    private void Start()
    {
        creepAgent = GetComponent<NavMeshAgent>();
        FindAndSetTarget();
    }

    private void Update()
    {
        timeSinceLastTargetSwitch += Time.deltaTime;
        if (timeSinceLastTargetSwitch > targetSwitchInterval)
        {
            CheckAndSwitchTargets();
            timeSinceLastTargetSwitch = 0f;
        }

        if (currentTarget != null)
        {
            Vector3 directionToTarget = currentTarget.position - transform.position;
            Vector3 stoppingPos = currentTarget.position - directionToTarget.normalized * stoppingDist;

            creepAgent.SetDestination(stoppingPos);
        }
    }

    void CheckAndSwitchTargets()
    {
        GameObject[] enemyCreeps = GameObject.FindGameObjectsWithTag(enemyCreepTag);
        Transform closestEnemyCreep = GetClosestObjectInRadius(enemyCreeps, aggroRange);

        if (closestEnemyCreep != null)
        {
            currentTarget = closestEnemyCreep;
        }
        else
        {
            GameObject[] Towers = GameObject.FindGameObjectsWithTag(enemyTowerTag);
            currentTarget = GetClosestObject(Towers);
        }
    }

    private Transform GetClosestObject(GameObject[] Towers)
    {
        float closestDist = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPos = transform.position;

        foreach (GameObject t in Towers)
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

    private Transform GetClosestObjectInRadius(GameObject[] Towers, float radius)
    {
        float closestDist = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPos = transform.position;

        foreach (GameObject t in Towers)
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

    void FindAndSetTarget()
    {
        GameObject[] enemyCreeps = GameObject.FindGameObjectsWithTag(enemyCreepTag);
        Transform closestEnemyCreep = GetClosestObjectInRadius(enemyCreeps, aggroRange);

        if (closestEnemyCreep != null)
        {
            currentTarget = closestEnemyCreep;
        }
        else
        {
            GameObject[] Towers = GameObject.FindGameObjectsWithTag(enemyTowerTag);
            currentTarget = GetClosestObject(Towers);
        }
    }
}
