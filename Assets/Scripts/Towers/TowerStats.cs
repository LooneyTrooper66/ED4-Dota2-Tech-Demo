using UnityEngine;

public class TowerStats : MonoBehaviour, IDamageable
{
    TowerTargeting towerAI;
    Stats stats;


    void Start()
    {
        towerAI = GetComponent<TowerTargeting>();
        stats = GetComponent<Stats>();
    }

    public void DealDamage(GameObject target, float damage)
    {
        Stats targetStats = target.GetComponent<Stats>();
        targetStats.targetHealth -= damage;

        // check if creep is dead
        if (targetStats != null && targetStats.targetHealth <= 0)
        {
            towerAI.currentTarget = null;
            towerAI.FindAndSetTarget();
            target.SetActive(false);
            stats.AwardXP();
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }
}
