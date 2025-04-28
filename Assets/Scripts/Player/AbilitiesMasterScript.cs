using UnityEngine;

public class AbilitiesMasterScript : MonoBehaviour
{
    IHeroAbilityFunctionality heroAbilities;


    private void Start()
    {
        heroAbilities = GetComponent<IHeroAbilityFunctionality>();
    }

    private void Update() // make sure to check for cooldowns
    {
        if (Input.GetKeyDown(KeyCode.Q)) // check keybinds later
        {
            heroAbilities.QAbility();
        }

        if (Input.GetKeyDown(KeyCode.W)) // check keybinds later
        {
            heroAbilities.WAbility();
        }

        if (Input.GetKeyDown(KeyCode.E)) // check keybinds later
        {
            heroAbilities.EAbility();
        }

        if (Input.GetKeyDown(KeyCode.R)) // check keybinds later
        {
            heroAbilities.RAbility();
        }
    }
}
