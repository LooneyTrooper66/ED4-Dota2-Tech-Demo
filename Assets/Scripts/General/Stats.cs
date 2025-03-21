using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour
{
    [Header("Base Stats")]
    public float health;
    public float mana;
    public float damage;
    public float secondsBetweenAttacks;
    public float attackRange;
    public int goldWorth;
    public int xpWorth;

    [Header("HealthBar Variables")]
    public float damageLerpDuration;
    private float currentHealth;
    private float targetHealth;
    private Coroutine damageCoroutine;

    HealthUI healthUI;


    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();

        currentHealth = health;
        targetHealth = health;

        healthUI.Start3DSlider(health);
    }

    public void TakeDamage(GameObject target, float damage)
    {
        Stats targetStats = target.GetComponent<Stats>();
        targetStats.targetHealth -= damage;

        // check if creep is dead
        if (target.CompareTag("AllyCreep") && targetStats.targetHealth <= 0)
        {
            Destroy(target.gameObject);
        }
        else if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
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
}
