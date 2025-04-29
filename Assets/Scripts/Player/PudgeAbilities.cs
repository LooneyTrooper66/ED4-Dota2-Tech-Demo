using UnityEngine;

public class PudgeAbilities : MonoBehaviour, IHeroAbilityFunctionality
{
    public HeroStats stats;
    public AbilitiesMasterScript abilitiesMS;

    [Header("Meat Hook")]
    public int meatHookLevel = 1;
    public float travelDist = 13;
    public float hookSearchRadius = 1;
    public float damage = 150;
    public float meathookCooldown = 18;
    public float meathookManaCost = 110;

    public GameObject hook;
    public GameObject hookTarget;

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
    public int meatShieldLevel = 1;
    public float damageStop = 8;
    public float shieldDuration = 4;
    private float meatShieldTimer = 0;
    public float meatShieldCooldown = 20;
    public float meatShieldManaCost = 50;

    [Header("Dismember")]
    public int dismemberLevel = 1;
    public float castRange;
    public float dismemberCooldown = 30;
    public float dismemberManaCost = 100;


    private void OnEnable()
    {
        rotActive = false;
        abilitiesMS.qTimer = meathookCooldown;
        abilitiesMS.eTimer = meatShieldCooldown;
        abilitiesMS.rTimer = dismemberCooldown;
    }

    private void Update()
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

        if (stats.meatShieldActive)
        {
            meatShieldTimer += Time.deltaTime;
            if (meatShieldTimer >= shieldDuration)
            {
                stats.meatShieldActive = false;
                meatShieldTimer = 0;
            }
        }

        #endregion
    }

    public void QAbility() // meat hook
    {
        abilitiesMS.qTimer = meathookCooldown;
        abilitiesMS.qCooldownZero = false;

        bool movingOut = true; // set lerpOut to false when its reached full extension and is coming back to pudge

        // set position of the move target to 13 units out in dir of mouse
        // change pos of move target back to 0,0,0 if either the hook gets to the target or the hook attatches to something on the way to the target
        // use ovetowards for hook movement, disable pudge movement for duration of attack


        // send out hook
        // only let hook catch things when cast out, not when coming back to pudge
        // damage the enemy, dont worry about wether they can teleport coz only against creeps which cant
    }

    public void WAbility() // rot
    {
        if (rotActive) { rotActive = false; } // dont reset rotTimer here coz the increments are meant to be set the first time you activate it (see docs)
        else { rotActive = true; }

        if (stats.meatShieldDamageStop != damageStop)
        {
            stats.meatShieldDamageStop = damageStop;
        }
    }

    public void EAbility() // meat shield
    {
        abilitiesMS.eTimer = meatShieldCooldown;
        abilitiesMS.eCooldownZero = false;

        // this one is timed and will deactivate itself if not deactivated by player
        if (stats.meatShieldActive) { stats.meatShieldActive = false; meatShieldTimer = 0; }
        else { stats.meatShieldActive = true; }
    }

    public void RAbility() // dismember
    {
        abilitiesMS.rTimer = dismemberCooldown;
        abilitiesMS.rCooldownZero = false;

        // maths maths maths aaaaaaa
    }
}
