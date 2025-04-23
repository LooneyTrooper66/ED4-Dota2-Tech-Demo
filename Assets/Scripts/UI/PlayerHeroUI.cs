using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHeroUI : MonoBehaviour
{
    public TextMeshProUGUI health;
    public HeroStats heroStats;


    private void Update()
    {
        health.text = heroStats.currentHealth.ToString();
    }
}
