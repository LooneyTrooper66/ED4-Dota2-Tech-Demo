 using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    private float attackRange;
    private float attackDamage;
    private float attackInterval;
    private float lastAttackTime;
    private float offset = 0.3f;
    TowerTargeting towerAI;
    private Stats stats;


    private void Start()
    {
        towerAI = GetComponent<TowerTargeting>();
        stats = GetComponent<Stats>();

        attackRange = stats.attackRange + offset;
        attackDamage = stats.damage;
        attackInterval = stats.secondsBetweenAttacks;
    }
    private void Update()
    {
        lastAttackTime += Time.deltaTime;

        if (CanAttack())
        {
            Attack();
        }
    }

    private bool CanAttack()
    {
        if (lastAttackTime >= attackInterval && towerAI.currentTarget != null)
        {
            float distToTarget = Vector3.Distance(transform.position, towerAI.currentTarget.position);

            return distToTarget <= attackRange;
        }

        return false;
    }

    void Attack()
    {
        lastAttackTime = 0;

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        TowerProjectile projectileScript = projectile.GetComponent<TowerProjectile>();
        projectileScript.SetTarget(towerAI.currentTarget.gameObject, attackDamage);
    }
}
