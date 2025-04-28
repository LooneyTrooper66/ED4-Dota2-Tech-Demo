using UnityEngine;

public class AbilitiesMasterScript : MonoBehaviour
{
    IHeroAbilityFunctionality heroAbilities;
    bool qCooldownZero;
    bool wCooldownZero;
    bool eCooldownZero;
    bool rCooldownZero;
    [HideInInspector] public float qCooldownTimer;
    [HideInInspector] public float wCooldownTimer;
    [HideInInspector] public float eCooldownTimer;
    [HideInInspector] public float rCooldownTimer;


    private void Start()
    {
        heroAbilities = GetComponent<IHeroAbilityFunctionality>();
        qCooldownZero = false;
        wCooldownZero = false;
        eCooldownZero = false;
        rCooldownZero = false;
    }

    private void Update() // make sure to check for cooldowns
    {
        #region inputs to activate abilities
        if (Input.GetKeyDown(KeyCode.Q) && qCooldownZero == true) // check keybinds later
        {
            heroAbilities.QAbility();
        }

        if (Input.GetKeyDown(KeyCode.W) && wCooldownZero == true) // check keybinds later
        {
            heroAbilities.WAbility();
        }

        if (Input.GetKeyDown(KeyCode.E) && eCooldownZero == true) // check keybinds later
        {
            heroAbilities.EAbility();
        }

        if (Input.GetKeyDown(KeyCode.R) && rCooldownZero == true) // check keybinds later
        {
            heroAbilities.RAbility();
        }
        #endregion

        #region cooldown timers

        /*if (qCooldownZero == false)
        {
            meatShieldTimer += Time.deltaTime;
            if (meatShieldTimer >= shieldDuration)
            {
                stats.meatShieldActive = false;
                meatShieldTimer = 0;
            }
        }

        if (stats.meatShieldActive)
        {
            meatShieldTimer += Time.deltaTime;
            if (meatShieldTimer >= shieldDuration)
            {
                stats.meatShieldActive = false;
                meatShieldTimer = 0;
            }
        }

        if (stats.meatShieldActive)
        {
            meatShieldTimer += Time.deltaTime;
            if (meatShieldTimer >= shieldDuration)
            {
                stats.meatShieldActive = false;
                meatShieldTimer = 0;
            }
        }

        if (stats.meatShieldActive)
        {
            meatShieldTimer += Time.deltaTime;
            if (meatShieldTimer >= shieldDuration)
            {
                stats.meatShieldActive = false;
                meatShieldTimer = 0;
            }
        }*/

        #endregion
    }
}
