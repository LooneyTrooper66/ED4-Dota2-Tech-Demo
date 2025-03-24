using UnityEngine;

public class NeutralCreepCombat : MonoBehaviour
{
    private float attackRange = 1f;
    private float attackDamage = 20;
    public float attackInterval;
    private float lastAttackTime;

    public enum currentState { attackingHero, attackingOther, idle };
    public currentState state = currentState.idle;
    private NeutralCreepAI neutralAI;
    private Stats stats;


    private void Start()
    {
        stats = GetComponent<Stats>();
        neutralAI = GetComponent<NeutralCreepAI>();

        state = currentState.idle;
        attackRange = stats.attackRange;
        attackDamage = stats.damage;
    }

    private void Update()
    {
        lastAttackTime += Time.deltaTime;
        if (state == currentState.attackingHero || state == currentState.attackingOther)
        {
            if (lastAttackTime >= attackInterval)
            {
                state = currentState.idle;
            }
        }

        if (CanAttack())
        {
            Attack();
        }
    }

    private bool CanAttack()
    {
        if (state == currentState.idle && neutralAI.currentTarget != null)
        {
            float distToTarget = Vector3.Distance(transform.position, neutralAI.currentTarget.position);

            return distToTarget <= attackRange;
        }

        return false;
    }

    void Attack()
    {
        lastAttackTime = 0;
        if (neutralAI.currentTarget.tag == "hero") { state = currentState.attackingHero; }
        else { state = currentState.attackingOther; }

        // could use if statements here to change how attack different enemies, more useful for hero attacks tho
        stats.TakeDamage(neutralAI.currentTarget.gameObject, attackDamage);
    }
}
