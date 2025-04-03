using UnityEngine;

public class LaneStats : Stats
{
    LaneCreepAI laneAI;
    Stats stats;


    void Start()
    {
        laneAI = GetComponent<LaneCreepAI>();
        stats = GetComponent<Stats>();
    }

    public override void TakeDamage(GameObject target, float damage)
    {
        Stats targetStats = target.GetComponent<Stats>();
        targetStats.targetHealth -= damage;

        // check if creep is dead
        if (targetStats != null && targetStats.targetHealth <= 0)
        {
            laneAI.currentTarget = null;
            laneAI.SetDestinationOfCreep();
            Destroy(target.gameObject); // should pool later for optimisation reasons
            stats.AwardXP();
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }
}
