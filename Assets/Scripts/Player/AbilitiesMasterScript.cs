using UnityEngine;
using UnityEngine.UI;

public class AbilitiesMasterScript : MonoBehaviour
{
    IHeroAbilityFunctionality heroAbilities;
    HeroStats stats;
    public float qManaCost;
    public float eManaCost;
    public float rManaCost;
    public Image qAbility;
    public Image eAbility;
    public Image rAbility;
    [HideInInspector] public bool qCooldownZero;
    [HideInInspector] public bool eCooldownZero;
    [HideInInspector] public bool rCooldownZero;
    [HideInInspector] public float qTimer = 18;
    [HideInInspector] public float eTimer = 20;
    [HideInInspector] public float rTimer = 30;


    private void Start()
    {
        heroAbilities = GetComponent<IHeroAbilityFunctionality>();
        stats = GetComponent<HeroStats>();
        qCooldownZero = true;
        eCooldownZero = true;
        rCooldownZero = true;
    }

    private void Update() // make sure to check for cooldowns
    {
        #region inputs to activate abilities
        if (Input.GetKeyDown(KeyCode.Q) && stats.currentMana >= qManaCost && qCooldownZero == true) // check keybinds later
        {
            heroAbilities.QAbility();
        }

        if (Input.GetKeyDown(KeyCode.W)) // doesnt have cooldown
        {
            heroAbilities.WAbility();
        }

        if (Input.GetKeyDown(KeyCode.E) && stats.currentMana >= eManaCost && eCooldownZero == true) // check keybinds later
        {
            heroAbilities.EAbility();
        }

        if (Input.GetKeyDown(KeyCode.R) && stats.currentMana >= rManaCost && rCooldownZero == true) // check keybinds later
        {
            heroAbilities.RAbility();
        }
        #endregion

        #region cooldown timers

        if (qCooldownZero == false)
        {
            qTimer -= Time.deltaTime;
            qAbility.color = Color.gray;
            if (qTimer <= 0)
            {
                qAbility.color = Color.white;
                qCooldownZero = true;
            }
        }

        if (eCooldownZero == false)
        {
            eTimer -= Time.deltaTime;
            eAbility.color = Color.gray;
            if (eTimer <= 0)
            {
                eAbility.color = Color.white;
                eCooldownZero = true;
            }
        }

        if (rCooldownZero == false)
        {
            rTimer -= Time.deltaTime;
            rAbility.color = Color.gray;
            if (rTimer <= 0)
            {
                rAbility.color = Color.white;
                rCooldownZero = true;
            }
        }

        #endregion
    }
}
