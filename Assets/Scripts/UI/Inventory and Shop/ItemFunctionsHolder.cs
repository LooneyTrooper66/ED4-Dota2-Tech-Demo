using UnityEngine;

public class ItemFunctionsHolder : MonoBehaviour
{
    [SerializeField] private HeroStats stats;
    [SerializeField] private PudgeAbilities abilities;
    bool clarityTimerOn = false;
    bool healingSalveTimerOn = false;
    float clarityTimer = 0;
    float healingSalveTimer = 0;


    private void Update()
    {
        #region // clarity timer

        if (clarityTimerOn)
        {
            clarityTimer -= Time.deltaTime;
            if (clarityTimer <= 0)
            {
                clarityTimerOn = false;
                stats.manaRegen -= 6;
            }
        }

        #endregion

        #region // healing salve timer

        if (healingSalveTimerOn)
        {
            healingSalveTimer -= Time.deltaTime;
            if (healingSalveTimer <= 0)
            {
                healingSalveTimerOn = false;
                stats.healthRegen -= 30;
            }
        }

        #endregion
    }

    public void ClarityFunctionality()
    {
        // +6 mana regen 25 seconds
        stats.manaRegen += 6;
        clarityTimer = 25;
        clarityTimerOn = true;
    }

    public void HealingSalveFunctionality()
    {
        // +30 health regen for 13 seconds
        stats.healthRegen += 30;
        healingSalveTimer = 13;
        healingSalveTimerOn = true;
    }

    public void WindLaceFunctionality()
    {
        // +20 move speed while in inventory (should be 0.1476 but i want speeeed)
        stats.movementSpeed += 1.476f;
    }

    public void BlinkDaggerFunctionality()
    {
        // teleport up to 12 away from your position. If used to try to teleport further than 12, will teleport you in the same direction but only by 9.6. 15 second cooldown
    }

    public void AetherLensFunctionality()
    {
        // +300 mana and +2.5 mana regen. passive ability increases cast range by 2.25
        stats.currentMana += 300;
        stats.manaRegen += 2.5f;
        abilities.travelDist += 2.25f;
        abilities.areaOfEffect += 2.25f;
        abilities.castRange += 2.25f;
    }

    public void TranquilBootsFunctionality()
    {
        // +14 health regen and +65 move speed (should be 0.4787 but i want speeeed). not adding passive for time reasons
        stats.healthRegen += 14;
        stats.movementSpeed += 4.797f;
    }

    public void ObserverWardFunctionality()
    {
        // 16 radius of ground vision, can be blocked by trees, higher elevations and other structures that block vision. wards last 6 minutes when placed
    }
}
