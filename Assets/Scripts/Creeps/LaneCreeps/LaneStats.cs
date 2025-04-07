using UnityEngine;

public class LaneStats : MonoBehaviour, IDamageable
{
    LaneCreepAI laneAI;
    Stats stats;


    public void Start()
    {
        laneAI = GetComponent<LaneCreepAI>();
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
            laneAI.SetDestinationOfCreep();
            target.SetActive(false);
            stats.AwardXP();
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }
}
