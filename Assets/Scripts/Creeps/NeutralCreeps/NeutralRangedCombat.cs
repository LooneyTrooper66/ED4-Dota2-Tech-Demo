using UnityEngine;

public class NeutralRangedCombat : MonoBehaviour
{
    private ObjectPooler projectilePool;
    private string projectilePoolTag = "projectilesN";
    public Transform shootPoint;
    private float attackRange;
    private float attackDamage;
    private float attackInterval;
    private float lastAttackTime;
    private float offset = 0.5f;

    public enum currentState { attackingHero, attackingOther, travelling };
    public currentState state = currentState.travelling;
    private NeutralCreepAI neutralAI;
    private Stats stats;

    void Awake()
    {
        GameObject temp = GameObject.Find("--NeutralCreepPooler--");
        projectilePool = temp.GetComponent<ObjectPooler>();
        stats = GetComponent<Stats>();
        neutralAI = GetComponent<NeutralCreepAI>();
    }

    void OnEnable()
    {
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

        GameObject projectile = projectilePool.SpawnFromPool(projectilePoolTag, shootPoint.position, shootPoint.rotation);
        NeutralCreepProjectile projectileScript = projectile.GetComponent<NeutralCreepProjectile>();
        projectileScript.SetTarget(neutralAI.currentTarget.gameObject, attackDamage);

        if (neutralAI.currentTarget.tag == "AllyCreep" || neutralAI.currentTarget.tag == "EnemyCreep")
        {
            LaneCreepAI laneAI = neutralAI.currentTarget.GetComponent<LaneCreepAI>();
            laneAI.currentTarget = transform;
        }

        Invoke("Check", 1);
    }

    void Check()
    {
        if (neutralAI.currentTarget != null && neutralAI.currentTarget.gameObject.activeSelf == false)
        {
            neutralAI.currentTarget = null;
            neutralAI.FindAndSetCreepTarget();
        }
    }
}
