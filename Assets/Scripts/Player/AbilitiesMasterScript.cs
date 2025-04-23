using UnityEngine;

public class AbilitiesMasterScript : MonoBehaviour
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

    [Header("Meat Shield")]
    public int meatShhieldLevel = 1;
    public float damageStop;

    [Header("Dismember")]
    public int dismemberLevel = 1;
    public float castRange;


    private void Update() // make sure to check for cooldowns
    {
        if (Input.GetKeyDown(KeyCode.Q)) // check keybinds later
        {
            CastMeatHook();
        }

        if (Input.GetKeyDown(KeyCode.W)) // check keybinds later
        {
            CastRot();
        }

        if (Input.GetKeyDown(KeyCode.E)) // check keybinds later
        {
            CastMeatShield();
        }

        if (Input.GetKeyDown(KeyCode.R)) // check keybinds later
        {
            CastDismember();
        }
    }

    void CastMeatHook()
    {
        // send out hook
        // only let hook catch things when cast out, not when coming back to pudge
        // damage the enemy, dont worry about wether they can teleport coz only against creeps which cant
    }

    void CastRot()
    {
        // anything in radius takes damage at set intervals
        // pudge takes damage to but cant die from it, check that pudges current health is bigger than the damage to be taken
        // its a chennel ability so will need to drain mana continuously
    }

    void CastMeatShield()
    {
        // just change a value in the herostats damagehero function to reduce incoming damage by a certain amount
    }

    void CastDismember()
    {
        // maths maths maths aaaaaaa
    }
}
