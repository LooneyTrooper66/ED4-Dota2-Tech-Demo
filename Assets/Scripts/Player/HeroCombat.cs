using UnityEngine;

public class HeroCombat : MonoBehaviour
{
    public Movement heroTarget;
    private float attackDamage;
    private float attackRange;
    private HeroStats stats;
    private float attackInterval;
    private float lastAttackTime = 0;

    public enum currentState { attackingHero, attackingOther, travelling };
    public currentState state = currentState.travelling;
    public GameObject currentTargetGO = null;


    private void Start()
    {
        heroTarget = GetComponent<Movement>();
        stats = GetComponent<HeroStats>();

        // making hero base modular (hopefully)
        attackDamage = stats.damage;
        attackRange = stats.attackRange + heroTarget.offset;
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

        // changing the target gameobject coz ye
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Invoke("ChangeTarget", 0.2f);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            currentTargetGO = null;
        }


        if (currentTargetGO != null)
        {
            // check layer, if its an ally then only attack if health is below 50%, if enemy then attack anyway
            if (currentTargetGO.layer == 8) // 8 is Enemy
            {
                if (CanAttack())
                {
                    Attack();
                }
            }
            else if (currentTargetGO.layer == 7) // 7 is Ally
            {
                Stats stats = currentTargetGO.GetComponent<Stats>();

                float tCurrentHealth = stats.currentHealth;
                float halfOfAllHealth = stats.health / 2;

                // if health is below 50%
                if (tCurrentHealth <= halfOfAllHealth)
                {
                    if (CanAttack())
                    {
                        Attack();
                    }
                }
            }
        }
    }

    void ChangeTarget()
    {
        if (heroTarget.currentTarget != null)
        {
            currentTargetGO = heroTarget.currentTarget.gameObject;
        }
    }

    private bool CanAttack()
    {
        if (state == currentState.travelling && heroTarget.currentTarget != null)
        {
            float distToTarget = Vector3.Distance(heroTarget.currentTarget.position, transform.position);

            return distToTarget <= attackRange;
        }

        return false;
    }

    void Attack()
    {
        lastAttackTime = 0;
        state = currentState.attackingOther; // cant be attacking hero so ye

        switch (currentTargetGO.tag)
        {
            case "NeutralCreep":
                NeutralCreepAI thisAi = currentTargetGO.GetComponent<NeutralCreepAI>();
                thisAi.loseAggroTimer = 0;
                thisAi.FindAndSetCreepTarget(); // dont need to check if its within the 18 unit radius coz the creeps dont have that much attack range
                break;
            case "EnemyCreep":
                LaneCreepAI laneAI = currentTargetGO.GetComponent<LaneCreepAI>();
                laneAI.currentTarget = transform;
                break;
        }

        IDamageable damageable = GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.DealDamage(currentTargetGO, attackDamage);
        }
    }
}
