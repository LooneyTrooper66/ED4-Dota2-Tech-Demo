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
    public float areaOfEffect;
    private bool rotActive;

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
            // anything in radius takes damage at set intervals
            // pudge takes damage to but cant die from it, check that pudges current health is bigger than the damage to be taken
            // its a chennel ability so will need to drain mana continuously

            // use checksphere at set intervals 
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
        if (rotActive) { rotActive = false; }
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
