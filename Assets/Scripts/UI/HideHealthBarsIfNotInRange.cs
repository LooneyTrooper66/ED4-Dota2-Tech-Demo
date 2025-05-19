using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HideHealthBarsIfNotInRange : MonoBehaviour
{
    GameObject player;
    HeroStats stats;
    Timer timer;
    Canvas canvas;
    float visionRn;
    ItemFunctionsHolder itemFH;
    GameObject closestVision;


    private void Awake()
    {
        canvas = GetComponent<Canvas>();
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

        if (Vector3.Distance(closestVision.transform.position, transform.position) > visionRn) canvas.enabled = false;
        else canvas.enabled = true;
    }
}
