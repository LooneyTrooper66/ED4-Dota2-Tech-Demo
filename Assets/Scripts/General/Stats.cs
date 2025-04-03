using UnityEngine;
using System.Collections;

public interface IDamageable
{
    void TakeDamage(GameObject target, float damage);
}

public class Stats : MonoBehaviour
{
    [Header("Base Stats")]
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
    private float currentHealth;
    [HideInInspector] public float targetHealth;
    [HideInInspector] public Coroutine damageCoroutine;

    HealthUI healthUI;
    GameObject player;
    HeroStats heroStats;


    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();

        currentHealth = health;
        targetHealth = health;

        healthUI.Start3DSlider(health);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        heroStats = player.GetComponent<HeroStats>();
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
