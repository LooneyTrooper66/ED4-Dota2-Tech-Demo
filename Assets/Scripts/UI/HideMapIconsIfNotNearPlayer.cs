using UnityEngine;
using System.Collections.Generic;

public class HideMapIconsIfNotNearPlayer : MonoBehaviour
{
    GameObject player;
    HeroStats stats;
    Timer timer;
    public GameObject uiDot;
    float visionRn;


    private void Awake()
    {
        GameObject temp = GameObject.Find("--Timer--");
        timer = temp.GetComponent<Timer>();
        player = GameObject.Find("Player");
        stats = player.GetComponent<HeroStats>();
    }

    private void FixedUpdate()
    {
        if (timer.currentTime == Timer.timeOfDay.day && visionRn != stats.visionDay) visionRn = stats.visionDay;
        if (timer.currentTime == Timer.timeOfDay.night && visionRn != stats.visionNight) visionRn = stats.visionNight;

        if (Vector3.Distance(player.transform.position, transform.position) > visionRn) uiDot.SetActive(false);
        else uiDot.SetActive(true);
    }
}
