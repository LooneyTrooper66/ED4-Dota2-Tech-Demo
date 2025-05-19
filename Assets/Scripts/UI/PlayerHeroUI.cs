using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHeroUI : MonoBehaviour
{
    public HeroStats heroStats;

    public TextMeshProUGUI health;
    public TextMeshProUGUI mana;
    public Slider healthBar;
    public Slider manaBar;


    private void Start()
    {
        UpdateSliderStats();
    }

    private void Update()
    {
        health.text = ((int)heroStats.currentHealth).ToString();
        mana.text = ((int)heroStats.currentMana).ToString();

        healthBar.value = heroStats.currentHealth;
        manaBar.value = heroStats.currentMana;
    }

    public void UpdateSliderStats() // call whenever updated in other scr
    {
        healthBar.maxValue = heroStats.health;
        manaBar.maxValue = heroStats.mana;
    }
}
