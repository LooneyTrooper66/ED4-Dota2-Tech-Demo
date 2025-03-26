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
    private float stoppingDist = 0.9f;
    public float aggroRange = 6f;
    private float targetSwitchInterval = 0.5f;
    private float aggroSwapInterval = 3;// aggro swap of 3
    private float timeSinceLastTargetSwitch = 0f;

    List<GameObject> attackingAllyHero = new List<GameObject>();
    List<GameObject> attackingAndInRange = new List<GameObject>();


    private void Start()
    {
        creepAgent = GetComponent<NavMeshAgent>();
        FindAndSetTarget();
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

    void FindAndSetTarget()
    {
        // set target to closest enemy attacking self
        // if no enemy attacking, set target to closest enemy attacking ally hero
        // if null, set target to closest enemy attacking allies
        // set to closest siege creep
        // set to structures (towers, etc..)


        SortWithinRange(); // sorting all gameobjects within aggro range into 3 lists, attacking self, attacking ally hero and within range

        Transform closestEnemyCreep = null;
        if (attackingAndInRange.Count > 0)
        {
            closestEnemyCreep = GetClosestObjectAttackingMe(attackingAndInRange, aggroRange);  // set target to closest enemy attacking self
            if (closestEnemyCreep == null)
            {
                if (attackingAllyHero.Count > 0)
                {
                    closestEnemyCreep = GetClosestObjectInRadius(attackingAllyHero, aggroRange);  // if no enemy attacking self, set target to closest enemy attacking ally hero
                }
                else
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
            GameObject[] Siege = GameObject.FindGameObjectsWithTag(enemySiegeTag);
            if (Siege.Length > 0 )
            {
                currentTarget = GetClosestObject(Siege);
            }
            else
            {
                if (Mathf.Abs(transform.position.x) < 45)
                {
                    GameObject[] Corners = GameObject.FindGameObjectsWithTag("CornerMarker");
                    currentTarget = GetClosestObject(Corners);
                }
                else
                {
                    GameObject[] Towers = GameObject.FindGameObjectsWithTag(enemyTowerTag);
                    currentTarget = GetClosestObject(Towers);
                }
            }
        }

        attackingAllyHero.Clear();
        attackingAndInRange.Clear();
    }

    void SortWithinRange()
    {
        float closestDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        CurrentAttackingState[] temp = FindObjectsByType<CurrentAttackingState>(FindObjectsSortMode.None);
        foreach (var t in temp)
        {
            float dist = Vector3.Distance(currentPos, t.gameObject.transform.position);

            if (dist < closestDist && dist <= aggroRange)
            {
                if (t.isAttackingHero == true)
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
            if (temp.currentlyAttacking == t.gameObject.name)
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
