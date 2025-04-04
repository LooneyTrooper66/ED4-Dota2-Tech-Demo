using UnityEngine;

public class NeutralMeleeCombat : MonoBehaviour
{
    private float attackRange;
    private float attackDamage;
    private float attackInterval;
    private float lastAttackTime;
    private float offset = 0.3f; // half the creeps radius (ish)

    public enum currentState { attackingHero, attackingOther, travelling };
    public currentState state = currentState.travelling;
    private NeutralCreepAI neutralAI;
    private Stats stats;


    private void Start()
    {
        stats = GetComponent<Stats>();
        neutralAI = GetComponent<NeutralCreepAI>();

        state = currentState.travelling;
        attackRange = stats.attackRange + offset;
        attackDamage = stats.damage;
        attackInterval = stats.secondsBetweenAttacks;
    }

    private void Update()
    {
        lastAttackTime += Time.deltaTime;
        if (state == currentState.attackingHero || state == currentState.attackingOther)
        {
            if (lastAttackTime >= attackInterval)
            {
                state = currentState.travelling;
            }
        }

        if (CanAttack())
        {
            Attack();
        }
    }

    private bool CanAttack()
    {
        if (state == currentState.travelling && neutralAI.currentTarget != null)
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

        if (neutralAI.currentTarget.gameObject.tag != "CornerMarker")
        {
            IDamageable damageable = GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(neutralAI.currentTarget.gameObject, attackDamage);
            }
        }
    }
}
