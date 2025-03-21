using UnityEngine;

public class LaneCreepCombat : MonoBehaviour
{
    private float attackRange = 1f;
    private float attackDamage = 20;
    public float attackInterval;
    private float lastAttackTime;

    public enum currentState { attackingHero, attackingOther, travelling };
    public currentState state = currentState.travelling;
    private LaneCreepAI laneAI;
    private Stats stats;


    private void Start()
    {
        stats = GetComponent<Stats>();
        laneAI = GetComponent<LaneCreepAI>();

        state = currentState.travelling;
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
        if (state == currentState.travelling && laneAI.currentTarget != null)
        {
            float distToTarget = Vector3.Distance(transform.position, laneAI.currentTarget.position);

            #region // delete after debugging
            bool temp = distToTarget <= attackRange;
            Debug.Log(temp);
            #endregion

            return distToTarget <= attackRange;
        }

        return false;
    }

    void Attack()
    {
        lastAttackTime = 0;
        if (laneAI.currentTarget.tag == "hero") { state = currentState.attackingHero; }
        else { state = currentState.attackingOther; }

        // could use if statements here to change how attack different enemies, more useful for hero attacks tho
        stats.TakeDamage(laneAI.currentTarget.gameObject, attackDamage);
    }
}
