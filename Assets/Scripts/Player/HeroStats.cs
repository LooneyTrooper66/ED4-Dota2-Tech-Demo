using UnityEngine;
using System.Collections;

public class HeroStats : MonoBehaviour, IDamageable
{
    public PlayerHeroStats playerStats;  // if adding multiple characters, pass this from the character selection screen so gets correct hero stats
    Movement heroMovement;
    Timer timer;
    HealthUI healthUI;
    RespawnHero respawn;
    float timeSinceLastGold = 0;
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
    public int currentNetworth;

    [Header("Resorces")]
    public int reliableGold;
    public int unreliableGold;
    public int xp;

    private float damageLerpDuration = 0.2f;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float targetHealth; // need to update this whenever level up or get item that increases health
    [HideInInspector] public Coroutine damageCoroutine;


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
        healthUI = GetComponent<HealthUI>();
        GameObject timerHolder = GameObject.Find("--Timer--");
        timer = timerHolder.GetComponent<Timer>();
        GameObject eventHolder = GameObject.Find("EventSystem");
        respawn = eventHolder.GetComponent<RespawnHero>();
        addGoldIncrement = 0.667f; // 60/90

        currentHealth = health;
        targetHealth = health;
        healthUI.Start3DSlider(health);
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

    void UpdateHealthUI()
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
            timeSinceLastGold += Time.deltaTime;
        }
        if (timeSinceLastGold >= addGoldIncrement)
        {
            reliableGold++;
        }
        #endregion

        currentNetworth = unreliableGold / 40;
    }

    public void DealDamage(GameObject target, float damage)
    {
        Stats targetStats = target.GetComponent<Stats>();
        targetStats.targetHealth -= damage;

        // check if creep is dead
        if (targetStats != null && targetStats.targetHealth <= 0)
        {
            heroMovement.currentTarget = null;
            target.SetActive(false);
            AwardXP(target, targetStats);
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }

    public void DamageHero(GameObject target, float damage)
    {
        HeroStats targetStats = target.GetComponent<HeroStats>();
        targetStats.targetHealth -= damage;

        if (targetStats != null && targetStats.targetHealth <= 0)
        {
            heroMovement.currentTarget = null;
            LoseResorcesOnDeath();
            targetHealth = health;
            currentHealth = health;
            UpdateHealthUI();
            respawn.RespawnPlayer();
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }

    public void AwardXP(GameObject target, Stats targetStats)
    {
        float dist = Vector3.Distance(target.transform.position, transform.position); // dont bother checking how many and spliting xp for now as only one hero should be in the game
        if (dist <= 15) // this is based on 1500 / 100
        {
            xp += targetStats.xpWorth;
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
