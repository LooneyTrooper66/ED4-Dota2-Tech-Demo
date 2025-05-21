using UnityEngine;

public class StencilSizeScript : MonoBehaviour
{
    public float visionDay;
    public float visionNight;
    GameObject sphere;
    Timer timer;


    private void Start()
    {
        sphere = gameObject;
        timer = GameObject.Find("--Timer--").GetComponent<Timer>();
    }

    private void Update()
    {
        if (timer.currentTime == Timer.timeOfDay.day && sphere.transform.localScale.x != visionDay) sphere.transform.localScale = new Vector3(visionDay, visionDay, visionDay);
        if (timer.currentTime == Timer.timeOfDay.night && sphere.transform.localScale.x != visionNight) sphere.transform.localScale = new Vector3(visionNight, visionNight, visionNight);
    }
}
