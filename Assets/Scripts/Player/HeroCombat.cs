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
    private GameObject currentTargetGO = null;


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
        if (Input.GetKeyDown(KeyCode.Mouse0) && heroTarget.currentTarget != null)
        {
            currentTargetGO = heroTarget.currentTarget.gameObject;
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

                float currentHealth = stats.currentHealth;
                float targetHealth = stats.targetHealth;
                float howMuchHealthLeft = currentHealth % targetHealth;

                // if health is below 50%
                if (howMuchHealthLeft <= 5)
                {
                    if (CanAttack())
                    {
                        Attack();
                    }
                }
            }
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

        IDamageable damageable = GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.DealDamage(currentTargetGO, attackDamage);
        }

        if (currentTargetGO.tag == "NeutralCreep")
        {
            NeutralCreepAI thisAi = heroTarget.currentTarget.GetComponent<NeutralCreepAI>();
            thisAi.FindAndSetHostileCreepTarget(); // dont need to check if its within the 18 unit radius coz the creeps dont have that much attack range
        }
    }
}
