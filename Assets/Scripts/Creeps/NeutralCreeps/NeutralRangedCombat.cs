using UnityEngine;

public class NeutralRangedCombat : MonoBehaviour, IPooledObject
{
    private ObjectPooler projectilePool;
    private string projectilePoolTag = "projectilesN";
    public Transform shootPoint;
    private float attackRange = 1f;
    private float attackDamage = 20;
    private float attackInterval;
    private float lastAttackTime;
    private float offset = 0.3f;

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

    public void OnObjectSpawn()
    {
        Debug.Log("i have been called i promise");

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
            GameObject projectile = projectilePool.SpawnFromPool(projectilePoolTag, shootPoint.position, shootPoint.rotation);
            NeutralCreepProjectile projectileScript = projectile.GetComponent<NeutralCreepProjectile>();
            projectileScript.SetTarget(neutralAI.currentTarget.gameObject, attackDamage);
        }
    }
}
