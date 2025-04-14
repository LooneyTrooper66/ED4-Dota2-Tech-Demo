using UnityEngine;

public class HeroStats : MonoBehaviour, IDamageable
{
    public PlayerHeroStats playerStats;  // if adding multiple characters, pass this from the character selection screen so gets correct hero stats
    Movement heroMovement;
    Timer timer;
    float timeSinceLastGold = 0;
    float addGoldIncrement;

    [Header("Primary Stats")]
    public int level;
    public int health;
    public float healthRegen;
    public int mana;
    public float manaRegen;
    public float damage;
    public float movementSpeed;
    public float secondsBetweenAttacks;
    public float attackRange;
    public float acquisitionRange;

    [Header("Secondary Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int visionDay;
    public int visionNight;
    public int currentNetworth;

    [Header("Resorces")]
    public int reliableGold;
    public int unreliableGold;
    public int xp;


    private void Start()
    {
        #region // set all base stats for game start
        level = 0;
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

        heroMovement = GetComponent<Movement>();
        GameObject timerHolder = GameObject.Find("--Timer--");
        timer = timerHolder.GetComponent<Timer>();
        addGoldIncrement = 0.667f; // 60/90
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

        timeSinceLastGold += Time.deltaTime;
        if (timeSinceLastGold >= addGoldIncrement)
        {
            reliableGold++;
        }

        currentNetworth = unreliableGold / 40;
    }

    public void TakeDamage(GameObject target, float damage)
    {
        Stats targetStats = target.GetComponent<Stats>();
        targetStats.targetHealth -= damage;

        // check if creep is dead
        if (targetStats != null && targetStats.targetHealth <= 0)
        {
            heroMovement.currentTarget = null;
            target.SetActive(false);
            LoseResorcesOnDeath();
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }

    void LoseResorcesOnDeath()
    {
        unreliableGold -= currentNetworth;
    }

    void UpdateUI()
    {
        // update all stats for ui and that
    }
}
