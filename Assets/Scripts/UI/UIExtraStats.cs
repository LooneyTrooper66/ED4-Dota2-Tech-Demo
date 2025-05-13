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
        statsText.text = ($"Level: {stats.level}   ");
    }
}
