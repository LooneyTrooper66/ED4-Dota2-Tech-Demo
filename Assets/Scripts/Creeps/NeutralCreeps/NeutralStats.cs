using UnityEngine;

public class NeutralStats : MonoBehaviour
{
    NeutralCreepAI neutralAI;
    Stats stats;


    void Start()
    {
        neutralAI = GetComponent<NeutralCreepAI>();
        stats = GetComponent<Stats>();
    }

    public void DealDamage(GameObject target, float damage)
    {
        Stats targetStats = target.GetComponent<Stats>();
        targetStats.targetHealth -= damage;

        // check if creep is dead
        if (targetStats != null && targetStats.targetHealth <= 0)
        {
            neutralAI.currentTarget = null;
            neutralAI.FindAndSetCreepTarget();
            target.SetActive(false);
            stats.AwardXP();
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }
}
