using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PudgeAbilities : MonoBehaviour, IHeroAbilityFunctionality
{
    public HeroStats stats;
    public AbilitiesMasterScript abilitiesMS;
    public Movement move;

    [Header("Meat Hook")]
    public int meatHookLevel = 1;
    public float travelDist = 13;
    public float hookSearchRadius = 1;
    public float damage = 150;
    public float meathookCooldown = 18;
    public float meathookManaCost = 110;
    private bool hookAiming;
    private bool hookCast;
    private bool hookReturning;

    public GameObject hook;
    public GameObject spinny;
    public GameObject hookTarget;
    public LayerMask hookAimMask;
    public LayerMask hookCollideMask;

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
        hookTarget.SetActive(false);
        hookAiming = false;
        hookCast = false;
        hookReturning = false;
        rotActive = false;
        abilitiesMS.qTimer = meathookCooldown;
        abilitiesMS.eTimer = meatShieldCooldown;
        abilitiesMS.rTimer = dismemberCooldown;
    }

    private void Update()
    {

        #region // meat hook

        if (hookAiming)
        {
            if (Input.GetMouseButtonDown(1))
            {
                // reset all ui and that
                hookAiming = false;
                hookTarget.SetActive(false);
                spinny.transform.rotation = Quaternion.identity;
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                abilitiesMS.qTimer = meathookCooldown;
                abilitiesMS.qCooldownZero = false;
                hookAiming = false;
                hookCast = true;
                //StartCoroutine(CastHookOut());
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, hookAimMask, QueryTriggerInteraction.Ignore))
            {
                spinny.transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }
        }

        if (hookCast)
        {
            move.agent.SetDestination(transform.position);

            hook.transform.position = Vector3.MoveTowards(hook.transform.position, hookTarget.transform.position, 16 * Time.deltaTime);
            if (Vector3.Distance(hook.transform.position, hookTarget.transform.position) < 0.1f)
            {
                hookCast = false;
                hookReturning = true;
            }

            // check for hitting things and if do start return
            Collider[] hitTargets = Physics.OverlapSphere(hook.transform.position, 1, hookCollideMask, QueryTriggerInteraction.Ignore);
            if (hitTargets.Length >= 1)
            {
                foreach (Collider c in hitTargets)
                {
                    if (c.gameObject.layer == 7) // ally
                    {
                        StartCoroutine(DrawInAlly(c.gameObject));
                    }

                    if (c.gameObject.layer == 8) // enemy
                    {
                        IDamageable damageable = GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            damageable.DealDamage(c.gameObject, 10000); // make sure it oneshots lol
                        }
                    }
                            
                }

                hookCast = false;
                hookReturning = true;
            }
        }

        if (hookReturning)
        {
            move.agent.SetDestination(transform.position);

            hook.transform.position = Vector3.MoveTowards(hook.transform.position, transform.position, 16 * Time.deltaTime);
            if (Vector3.Distance(hook.transform.position, transform.position) < 0.1f)
            {
                hookReturning = false;
                hookTarget.SetActive(false);
                spinny.transform.rotation = Quaternion.identity;
            }
        }

        #endregion

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
        if (hookAiming) { hookAiming = false; }
        else { hookAiming = true; hookTarget.SetActive(true); }
    }

    IEnumerator DrawInAlly(GameObject allyCreep)
    {
        while (hookReturning)
        {
            allyCreep.gameObject.transform.position = hook.transform.position;
        }
        yield return null;
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

    IEnumerator BringHookBack()
    {
        float distLeft = Vector3.Distance(hook.transform.position, hookTarget.transform.position);
        while (distLeft < (travelDist - 0.5f))
        {
            hook.transform.position = Vector3.MoveTowards(hookTarget.transform.position, transform.position, 6 * Time.deltaTime);
        }
        hookReturning = false; // set to false after killing creep/once back depending on wether caught anything
        hookTarget.SetActive(false);
        yield return null;
    }
}
