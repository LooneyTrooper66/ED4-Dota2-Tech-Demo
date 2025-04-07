using UnityEngine;

public class RangedLaneCreepCombat : MonoBehaviour, IPooledObject
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    private float attackRange = 1f;
    private float attackDamage = 20;
    private float attackInterval;
    private float lastAttackTime;
    private float offset = 0.3f;

    public enum currentState { attackingHero, attackingOther, travelling };
    public currentState state = currentState.travelling;
    private LaneCreepAI laneAI;
    private Stats stats;


    public void OnObjectSpawn()
    {
        stats = GetComponent<Stats>();
        laneAI = GetComponent<LaneCreepAI>();

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
        if (state == currentState.travelling && laneAI.currentTarget != null)
        {
            float distToTarget = Vector3.Distance(transform.position, laneAI.currentTarget.position);

            return distToTarget <= attackRange;
        }

        return false;
    }

    void Attack()
    {
        lastAttackTime = 0;
        if (laneAI.currentTarget.tag == "hero") { state = currentState.attackingHero; }
        else { state = currentState.attackingOther; }

        if (laneAI.currentTarget.gameObject.tag != "CornerMarker")
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            CreepProjectile projectileScript = projectile.GetComponent<CreepProjectile>();
            projectileScript.SetTarget(laneAI.currentTarget.gameObject, attackDamage);

            if (laneAI.currentTarget.tag == "NeutralCreep")
            {
                NeutralCreepAI thisAi = laneAI.currentTarget.GetComponent<NeutralCreepAI>();
                thisAi.FindAndSetHostileCreepTarget(); // dont need to check if its within the 18 unit radius coz the creeps dont have that much attack range
            }
        }
    }
}
