using UnityEngine;

public class LaneStatsR : MonoBehaviour, IDamageable
{
    SiegeCreepTargetingAI laneAI;
    Stats stats;


    void Start()
    {
        laneAI = GetComponent<SiegeCreepTargetingAI>();
        stats = GetComponent<Stats>();
    }

    public void TakeDamage(GameObject target, float damage)
    {
        Stats targetStats = target.GetComponent<Stats>();
        targetStats.targetHealth -= damage;

        // check if creep is dead
        if (targetStats != null && targetStats.targetHealth <= 0)
        {
            laneAI.currentTarget = null;
            laneAI.FindAndSetTarget();
            Destroy(target.gameObject); // should pool later for optimisation reasons
            stats.AwardXP();
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }
}
