using UnityEngine;

public class PudgeAbilities : MonoBehaviour, IHeroAbilityFunctionality
{
    public HeroStats stats;

    [Header("Meat Hook")]
    public int meatHookLevel = 1;
    public float travelDist = 13;
    public float hookSearchRadius = 1;
    public float damage = 150;
    public float cooldown;
    public float manaCost;

    public GameObject hook;

    [Header("Rot")]
    public int rotLevel = 1;
    public float areaOfEffect = 2.5f;
    public float rotDamage = 30;
    public float rotSlow = 14; // in %
    private bool rotActive;
    private float rotTimer = 0;
    private float pudgeTakeDmgTimer = 0;
    public LayerMask rotLayerMask;

    [Header("Meat Shield")]
    public int meatShhieldLevel = 1;
    public float damageStop;
    private bool meatShieldActive;

    [Header("Dismember")]
    public int dismemberLevel = 1;
    public float castRange;


    private void OnEnable()
    {
        rotActive = false;
        meatShieldActive = false;
    }

    private void Update() // make sure to check for cooldowns
    {
        #region // rot

        if (rotActive)
        {
            rotTimer += Time.deltaTime;
            pudgeTakeDmgTimer += Time.deltaTime;
            if (rotTimer >= 0.2f)
            {
                // damage anything within radius, also apply slow somehow
                Collider[] inRadius = Physics.OverlapSphere(transform.position, areaOfEffect, rotLayerMask);
                if (inRadius.Length <= 0) { rotTimer = 0; return; }
                foreach (Collider c in inRadius)
                {
                    INavmeshSpeed speedChange = c.gameObject.GetComponent<INavmeshSpeed>();
                    if (speedChange != null)
                    {
                        speedChange.ChangeNavAgentSpeed(rotSlow);
                    }

                    IDamageable damageable = c.gameObject.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.DealDamage(c.gameObject, rotDamage);
                    }
                }

                rotTimer = 0;
            }

            if (pudgeTakeDmgTimer >= 1)
            {
                // damage pudge
                if (stats.currentHealth >= rotDamage)
                {
                    stats.currentHealth -= rotDamage;
                    stats.UpdateHealthUI();
                }

                pudgeTakeDmgTimer = 0;
            }
        }

        #endregion

        #region // meat shield

        if (meatShieldActive)
        {
            // just change a value in the herostats damagehero function to reduce incoming damage by a certain amount
        }

        #endregion
    }

    public void QAbility() // meat hook
    {
        // send out hook
        // only let hook catch things when cast out, not when coming back to pudge
        // damage the enemy, dont worry about wether they can teleport coz only against creeps which cant
    }

    public void WAbility() // rot
    {
        if (rotActive) { rotActive = false; } // dont reset rotTimer here coz the increments are meant to be set the first time you activate it (see docs)
        else { rotActive = true; }
    }

    public void EAbility() // meat shield
    {
        // this one is timed and will deactivate itself if not deactivated by player
        if (meatShieldActive) { meatShieldActive = false; }
        else { meatShieldActive = true; }
    }

    public void RAbility() // dismember
    {
        // maths maths maths aaaaaaa
    }
}
