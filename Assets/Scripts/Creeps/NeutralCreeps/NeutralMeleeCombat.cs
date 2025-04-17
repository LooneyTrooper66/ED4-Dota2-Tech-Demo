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
    HeroStats heroStats;


    private void Awake()
    {
        stats = GetComponent<Stats>();
        neutralAI = GetComponent<NeutralCreepAI>();
        GameObject player = GameObject.Find("Player");
        heroStats = player.GetComponent<HeroStats>();

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
        if (neutralAI.currentTarget.tag == "Player") { state = currentState.attackingHero; }
        else { state = currentState.attackingOther; }

        if (neutralAI.currentTarget.gameObject.tag != "CornerMarker")
        {
            if (neutralAI.currentTarget.tag == "Player")
            {
                heroStats.DamageHero(neutralAI.currentTarget.gameObject, attackDamage);
                neutralAI.currentTarget = null;
                neutralAI.FindAndSetCreepTarget();
                return;
            }

            IDamageable damageable = GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.DealDamage(neutralAI.currentTarget.gameObject, attackDamage);
            }
        }
    }
}
