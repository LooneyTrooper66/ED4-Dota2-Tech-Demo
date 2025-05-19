using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class HideUILaneCreeps : MonoBehaviour
{
    GameObject player;
    HeroStats stats;
    Timer timer;
    public GameObject uiDot;
    float visionRn;
    GameObject closestVision;
    ItemFunctionsHolder itemFH;


    private void Awake()
    {
        GameObject temp = GameObject.Find("--Timer--");
        timer = temp.GetComponent<Timer>();
        player = GameObject.Find("Player");
        closestVision = player;
        stats = player.GetComponent<HeroStats>();
        temp = GameObject.Find("EventSystem");
        itemFH = temp.GetComponent<ItemFunctionsHolder>();
    }

    private void FixedUpdate()
    {
        if (itemFH.observers.Count > 0)
        {
            foreach (GameObject observer in itemFH.observers)
            {
                if (Vector3.Distance(observer.transform.position, transform.position) < Vector3.Distance(closestVision.transform.position, transform.position))
                {
                    closestVision = observer;
                    visionRn = 16;
                }
            }
        }
        if (closestVision == null || Vector3.Distance(player.transform.position, transform.position) < Vector3.Distance(closestVision.transform.position, transform.position))
        {
            closestVision = player;
        }
        if (closestVision == player)
        {
            if (timer.currentTime == Timer.timeOfDay.day && visionRn != stats.visionDay) visionRn = stats.visionDay;
            if (timer.currentTime == Timer.timeOfDay.night && visionRn != stats.visionNight) visionRn = stats.visionNight;
        }

        if (Vector3.Distance(closestVision.transform.position, transform.position) > visionRn) uiDot.SetActive(false);
        else uiDot.SetActive(true);

        if (uiDot.activeSelf == false)
        {
            if (transform.position.x < -20 || transform.position.z < -20)
            {
                if (timer.currentTime == Timer.timeOfDay.day && visionRn != stats.visionDay) visionRn = stats.visionDay;
                if (timer.currentTime == Timer.timeOfDay.night && visionRn != stats.visionNight) visionRn = stats.visionNight;
                uiDot.SetActive(true);
            }
            else
            {
                uiDot.SetActive(false);
            }
        }
    }
}
