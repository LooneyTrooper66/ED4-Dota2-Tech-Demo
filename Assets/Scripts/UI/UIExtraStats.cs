using UnityEngine;
using TMPro;

public class UIExtraStats : MonoBehaviour
{
    public TextMeshProUGUI statsText;
    public HeroStats stats;


    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab)) statsText.enabled = true;
        else statsText.enabled = false;

        if (statsText.enabled ) UpdateStatsText();
    }

    void UpdateStatsText()
    {
        statsText.text = ($"Level: {stats.level}   MaxHealth: {stats.health}\nHealthRegen: {stats.healthRegen}   Mana: {stats.mana}\n" +
            $"ManaRegen: {stats.manaRegen}   MaxHealth: {stats.health}\nDamage: {stats.damage}   MoveSpeed: {stats.movementSpeed}\n" +
            $"AttackRange: {stats.attackRange}   Strength: {stats.strength}\nIntelligence: {stats.intelligence}   Agility: {stats.agility}\n" +
            $"VisionDay: {stats.visionDay}   VisionNight: {stats.visionNight}\nCurrentNetworth: {stats.currentNetworth}");
    }
}
