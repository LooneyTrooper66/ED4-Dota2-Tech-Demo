using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TowerTargeting : MonoBehaviour
{
    public Transform currentTarget;
    public string thisTowersAlignment;
    public string enemyCreepTag;
    public string enemySiegeTag;
    public float attackRange;
    public float aggroRange;
    private float targetSwitchInterval = 0.1f;
    private float aggroSwapInterval = 3;// aggro swap of 3
    private float timeSinceLastTargetSwitch = 0f;
    private float offset = 0.5f;
    Transform closestEnemyCreep = null;
    GameObject playerGO;


    private void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        OffsetAllValuesForRadius();

        FindAndSetTarget();
    }

    private void Update()
    {
        // changes enemy destination target - different intervals depending on if has current target
        AggroSwapTimer();
    }

    private void FixedUpdate()
    {
        // checks the priority list for next target
        FindAndSetTarget();
    }

    void AggroSwapTimer()
    {
        // sets next target
        timeSinceLastTargetSwitch += Time.deltaTime;
        if (currentTarget == null)
        {
            if (timeSinceLastTargetSwitch > targetSwitchInterval)
            {
                timeSinceLastTargetSwitch = 0f;
            }
        }
        else
        {
            if (timeSinceLastTargetSwitch > aggroSwapInterval)
            {
                timeSinceLastTargetSwitch = 0f;
            }
        }
    }

    public void FindAndSetTarget()
    {
        float dist = Vector3.Distance(transform.position, playerGO.transform.position);
        if (dist <= attackRange && thisTowersAlignment == "dire")
        {
            currentTarget = playerGO.transform;
            return;
        }

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
        }
    }

    private Transform GetClosestObjectInRadius(GameObject[] Things, float radius)
    {
        float closestDist = 1000;
        float closestAttackingHero = 1000;
        Transform closestObject = null;
        Transform closestObjAttackingHero = null;
        Vector3 currentPos = transform.position;

        foreach (GameObject t in Things)
        {
            CurrentAttackingState currentAttacking = t.GetComponent<CurrentAttackingState>();
            float dist = Vector3.Distance(currentPos, t.transform.position);
            if (currentAttacking.state == CurrentAttackingState.currentState.attackingHero)
            {
                closestAttackingHero = dist;
                closestObjAttackingHero = t.transform;
            }
            else if (dist < closestDist && dist <= radius)
            {
                closestDist = dist;
                closestObject = t.transform;
            }
        }
         
        // if any enemies attacking the player (only hero so only one needs to check), attack that creep, else attack whatevers closest
        if (closestObjAttackingHero != null) { return closestObjAttackingHero; }
        else { return closestObject; }
    }

    void OffsetAllValuesForRadius()
    {
        attackRange += offset;
        aggroRange += offset;
    }
}
