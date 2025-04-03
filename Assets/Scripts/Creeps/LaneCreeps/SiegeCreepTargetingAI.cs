using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SiegeCreepTargetingAI : MonoBehaviour
{
    private NavMeshAgent creepAgent;
    public Transform currentTarget;
    public string enemyCreepTag;
    public string enemySiegeTag;
    public string enemyTowerTag;
    public float stoppingDist = 6f; // needs to be less than attack/aggro range
    public float aggroRange = 8f;
    private float targetSwitchInterval = 0.5f;
    private float aggroSwapInterval = 3;// aggro swap of 3
    private float timeSinceLastTargetSwitch = 0f;

    List<GameObject> TowerList = new List<GameObject>();
    List<GameObject> attackingAllyHero = new List<GameObject>();
    List<GameObject> attackingAndInRange = new List<GameObject>();
    List<GameObject> attackingAndInRangeSiege = new List<GameObject>();
    List<GameObject> cornerMarkers = new List<GameObject>();


    private void Start()
    {
        creepAgent = GetComponent<NavMeshAgent>();

        // add all enemy towers to a list at the start so dont have to make a new one each time it checks
        GameObject[] Tower = GameObject.FindGameObjectsWithTag(enemyTowerTag);
        foreach(GameObject t in Tower)
        {
            TowerList.Add(t);
        }
        // add all corner markers to list at start
        GameObject[] Corners = GameObject.FindGameObjectsWithTag("CornerMarker");
        foreach (GameObject t in Corners)
        {
            cornerMarkers.Add(t);
        }

        FindAndSetTarget();
    }

    private void Update()
    {
        // checking if any enemy units in range - different intervals depending on if has target
        AggroSwapTimer();

        // setting creep destination to the current target || i am a fag cat its true -jrodan (why am i signing off my own comments idfk -fag cat)
        if (currentTarget != null)
        {
            Vector3 directionToTarget = currentTarget.position - transform.position;
            Vector3 stoppingPos = currentTarget.position - directionToTarget.normalized * stoppingDist;

            creepAgent.SetDestination(stoppingPos);
        }
    }

    void AggroSwapTimer()
    {
        timeSinceLastTargetSwitch += Time.deltaTime;
        if (currentTarget == null)
        {
            if (timeSinceLastTargetSwitch > targetSwitchInterval)
            {
                FindAndSetTarget();
                timeSinceLastTargetSwitch = 0f;
            }
        }
        else
        {
            if (timeSinceLastTargetSwitch > aggroSwapInterval)
            {
                FindAndSetTarget();
                timeSinceLastTargetSwitch = 0f;
            }
        }
    }

    public void FindAndSetTarget()
    {
        currentTarget = GetClosestObjectInRadius(TowerList, aggroRange);  // check for towers to attack within range first
        if (currentTarget == null)
        {
            SortWithinRange(); // sorting all gameobjects within aggro range into 3 lists, attacking self, attacking ally hero and within range

            Transform closestEnemyCreep = null;
            closestEnemyCreep = GetClosestObjectInRadius(attackingAndInRangeSiege, aggroRange);  // set to closest siege creep

            if (closestEnemyCreep == null)
            {
                closestEnemyCreep = GetClosestObjectAttackingMe(attackingAndInRange, aggroRange);  // if no siege creeps, set target to closest enemy attacking self
                if (closestEnemyCreep == null)
                {
                    closestEnemyCreep = GetClosestObjectInRadius(attackingAllyHero, aggroRange);  // if no enemy attacking self, set target to closest enemy attacking ally hero
                    if (closestEnemyCreep == null)
                    {
                        closestEnemyCreep = GetClosestObjectInRadius(attackingAndInRange, aggroRange);  // if no enemy attacking ally hero, set target to closest enemy attacking allies
                    }
                }
            }


            if (closestEnemyCreep != null)
            {
                currentTarget = closestEnemyCreep;
            }
            else
            {
                if (Mathf.Abs(transform.position.x) < 45)  // if no, set to structures (towers, etc..)
                {
                    currentTarget = GetClosestObject(cornerMarkers);
                }
                else
                {
                    currentTarget = GetClosestObject(TowerList);
                }
            }
        }              

        attackingAllyHero.Clear();
        attackingAndInRange.Clear();
    }

    void SortWithinRange()
    {
        Vector3 currentPos = transform.position;

        CurrentAttackingState[] temp = FindObjectsByType<CurrentAttackingState>(FindObjectsSortMode.None);
        foreach (var t in temp)
        {
            float dist = Vector3.Distance(currentPos, t.gameObject.transform.position);

            if (dist <= aggroRange)
            {
                if (t.gameObject.tag == enemySiegeTag)
                {
                    attackingAndInRangeSiege.Add(t.gameObject);
                }
                else if (t.isAttackingHero == true)
                {
                    attackingAllyHero.Add(t.gameObject);
                }
                else if (t.isAttacking == true)
                {
                    attackingAndInRange.Add(t.gameObject);
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

    private Transform GetClosestObjectInRadius(List<GameObject> Things, float radius)
    {
        float closestDist = Mathf.Infinity;
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

    private Transform GetClosestObjectAttackingMe(List<GameObject> Things, float radius)
    {
        float closestDist = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPos = transform.position;

        foreach (GameObject t in Things)
        {
            CurrentAttackingState temp = t.GetComponent<CurrentAttackingState>();
            if (temp.currentlyAttacking == gameObject.name)
            {
                float dist = Vector3.Distance(currentPos, t.transform.position);

                if (dist < closestDist && dist <= radius)
                {
                    closestDist = dist;
                    closestObject = t.transform;
                }
            }
        }

        return closestObject;
    }
}
