using UnityEngine;
using System.Collections;

public class HeroStats : MonoBehaviour, IDamageable
{
    public PlayerHeroStats playerStats;  // if adding multiple characters, pass this from the character selection screen so gets correct hero stats
    Movement heroMovement;
    HeroCombat heroCombat;
    Timer timer;
    HealthUI healthUI;
    RespawnHero respawn;
    float addGoldIncrement;

    [Header("Primary Stats")]
    public float level;
    public float health;
    public float healthRegen;
    public float mana;
    public float manaRegen;
    public float damage;
    public float movementSpeed;
    public float secondsBetweenAttacks;
    public float attackRange;
    public float acquisitionRange;

    [Header("Secondary Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public int visionDay;
    public int visionNight;
    public float currentNetworth;

    [Header("Resorces")]
    public float reliableGold;
    public float unreliableGold;
    public int xp;

    private float damageLerpDuration = 0.2f;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float targetHealth; // need to update this whenever level up or get item that increases health
    [HideInInspector] public Coroutine damageCoroutine;
    [HideInInspector] public bool meatShieldActive;
    [HideInInspector] public float meatShieldDamageStop = 8;
    public LayerMask layerMask;


    void Awake()
    {
        #region // set all base stats for game start
        level = playerStats.level;
        health = playerStats.health;
        healthRegen = playerStats.healthRegen;
        mana = playerStats.mana;
        manaRegen = playerStats.manaRegen;
        damage = playerStats.damage;
        movementSpeed = playerStats.movementSpeed;
        secondsBetweenAttacks = playerStats.secondsBetweenAttacks;
        attackRange = playerStats.attackRange;
        acquisitionRange = playerStats.acquisitionRange;
        strength = playerStats.strength;
        agility = playerStats.agility;
        intelligence = playerStats.intelligence;
        visionDay = playerStats.visionDay;
        visionNight = playerStats.visionNight;
        currentNetworth = 0;
        reliableGold = playerStats.reliableGold;
        unreliableGold = 0;
        xp = 0;
        #endregion
    }

    private void Start()
    {
        heroMovement = GetComponent<Movement>();
        heroCombat = GetComponent<HeroCombat>();
        healthUI = GetComponent<HealthUI>();
        GameObject timerHolder = GameObject.Find("--Timer--");
        timer = timerHolder.GetComponent<Timer>();
        GameObject eventHolder = GameObject.Find("EventSystem");
        respawn = eventHolder.GetComponent<RespawnHero>();
        addGoldIncrement = 0.667f; // 60/90

        currentHealth = health;
        targetHealth = health;
        healthUI.Start3DSlider(health);
        meatShieldActive = false;
    }

    public void StartLerpHealth()
    {
        if (damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(LerpHealth());
        }
    }

    private IEnumerator LerpHealth()
    {
        float timeElapsed = 0;
        float initialHealth = currentHealth;
        float target = targetHealth;

        while (timeElapsed < damageLerpDuration)
        {
            currentHealth = Mathf.Lerp(initialHealth, target, timeElapsed / damageLerpDuration);
            UpdateHealthUI();

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        currentHealth = target;
        UpdateHealthUI();

        damageCoroutine = null;
    }

    public void UpdateHealthUI()
    {
        healthUI.Update3DSlider(currentHealth);
    }

    void Update()
    {
        UpdateUI();

        #region // changing increment at certain times
        if (timer.timeElapsed > 360 && addGoldIncrement == 0.667) // (12/2) * 60 --- 60/94.8
        {
            addGoldIncrement = 0.633f;
        }
        else if (timer.timeElapsed > 900 && addGoldIncrement == 0.633) // (30/2) * 60 --- 60/99.8
        {
            addGoldIncrement = 0.6f;
        }
        else if (timer.timeElapsed > 1350 && addGoldIncrement == 0.6) // (45/2) * 60 --- 60/105.5
        {
            addGoldIncrement = 0.568f; // stopping here coz realistically i am not showing this for more than 20 minutes
        }
        #endregion

        #region // increases reliable gold 
        if (timer.gamePlaying == true)
        {
            reliableGold += addGoldIncrement * Time.deltaTime;
        }
        #endregion

        currentNetworth = unreliableGold / 40;

        #region // incrementing health and mana regen
        if (currentHealth < health)
        {
            currentHealth += healthRegen * Time.deltaTime;
            UpdateHealthUI();
        }

        /*if (mana != 0)
        {
            if (currentMana < mana)
            {
                currentMana += manaRegen * Time.deltaTime;
            }
        }*/
        #endregion
    }

    public void DealDamage(GameObject target, float damage)
    {
        Stats targetStats = target.GetComponent<Stats>();
        targetStats.targetHealth -= damage;

        // check if creep is dead
        if (targetStats != null && targetStats.targetHealth <= 0)
        {
            unreliableGold += targetStats.goldWorth;
            xp += targetStats.xpWorth; // not wirth checking dist coz no pudge attacks have more than 15 range
            heroMovement.currentTarget = null;
            heroCombat.currentTargetGO = null;
            FindNewClosestEnemyCreep();
            target.SetActive(false);
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }

    public void DamageHero(GameObject target, float damage)
    {
        HeroStats targetStats = target.GetComponent<HeroStats>();
        if (meatShieldActive)
        {
            damage -= meatShieldDamageStop;
        }
        targetStats.targetHealth -= damage;

        if (targetStats != null && targetStats.targetHealth <= 0)
        {
            heroMovement.currentTarget = null;
            heroCombat.currentTargetGO = null;
            LoseResorcesOnDeath();
            targetHealth = health;
            currentHealth = health;
            UpdateHealthUI();
            respawn.RespawnPlayer(level);
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }

    void FindNewClosestEnemyCreep()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, 6, layerMask); // any gameobjects within a 6 unit radius (600/100) on layer 8

        if (enemiesInRange.Length <= 0)
        {
            heroMovement.currentTarget = null;
            heroCombat.currentTargetGO = null;
            heroMovement.agent.SetDestination(transform.position);
            return;
        }

        float closestDist = Mathf.Infinity;
        GameObject closestObject = null;
        Vector3 currentPos = transform.position;

        foreach (Collider enemy in enemiesInRange)
        {
            float dist = Vector3.Distance(currentPos, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestObject = enemy.gameObject;
            }
        }

        if (closestObject != null)
        {
            heroMovement.currentTarget = closestObject.transform;
            heroCombat.currentTargetGO = closestObject;
        }
    }

    void LoseResorcesOnDeath()
    {
        unreliableGold -= currentNetworth;
    }

    void UpdateUI()
    {
        // update all stat based ui

        // reliable gold, unreliable gold, all gold, mana, health, etc...
    }
}
