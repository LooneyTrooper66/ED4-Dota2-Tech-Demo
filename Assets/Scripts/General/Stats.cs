using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour
{
    public CreepStatDefaults creepStats;

    [Header("Current Stats")]
    public float health;
    public float healthRegen;
    public float mana;
    public float manaRegen;
    public float damage;
    public float secondsBetweenAttacks;
    public float attackRange;
    public float acquisitionRange;
    public int goldWorth;
    public int xpWorth;
    public int visionDay;
    public int visionNight;

    private float damageLerpDuration = 0.2f;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float targetHealth;
    [HideInInspector] public Coroutine damageCoroutine;

    HealthUI healthUI;
    GameObject player;
    HeroStats heroStats;


    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();
        player = GameObject.FindGameObjectWithTag("Player");
        heroStats = player.GetComponent<HeroStats>();
    }

    public void OnEnable()
    {
        health = creepStats.health;
        healthRegen = creepStats.healthRegen;
        mana = creepStats.mana;
        manaRegen = creepStats.manaRegen;
        damage = creepStats.damage;
        secondsBetweenAttacks = creepStats.secondsBetweenAttacks;
        attackRange = creepStats.attackRange;
        acquisitionRange = creepStats.acquisitionRange;
        goldWorth = creepStats.goldWorth;
        xpWorth = creepStats.xpWorth;
        visionDay = creepStats.visionDay;
        visionNight = creepStats.visionNight;

        currentHealth = creepStats.health;
        targetHealth = creepStats.health;

        healthUI.Start3DSlider(creepStats.health);
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

    public void AwardXP()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position); // dont bother checking how many and spliting xp for now as only one hero should be in the game
        if (dist <= 15) // this is based on 1500 / 100
        {
            heroStats.xp += xpWorth;
        }
    }
}
