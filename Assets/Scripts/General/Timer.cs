using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // need timer 
    // have day night cycle in here
    // connect this to a scriptable object to change states between day and night and to connect any time related things

    public float timeElapsed;
    public bool gamePlaying;
    public enum timeOfDay { day , night };
    public timeOfDay currentTime;
    private float dayNightTimer;

    public TextMeshProUGUI uiTimerText;
    public Transform dayNightHolder;



    private void Start()
    {
        timeElapsed = 0f; // set to -60 for actual game
        dayNightTimer = timeElapsed;
        gamePlaying = true; // set this to false when in menus
        currentTime = timeOfDay.night;
    }

    private void Update()
    {
        // pause mechanic
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePlaying) { gamePlaying = false; Time.timeScale = 0; }
            else { gamePlaying = true; Time.timeScale = 1; }
        }

        // for debugging and testing
        if (Input.GetKeyDown(KeyCode.Space) && gamePlaying == true)
        {
            if (Time.timeScale == 1) Time.timeScale = 4;
            else if (Time.timeScale == 4) Time.timeScale = 2f;
            else Time.timeScale = 1;
        }

        // updates day/night cycle
        if (gamePlaying)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimer();

            dayNightHolder.RotateAround(dayNightHolder.position, Vector3.forward, 1.24f * Time.deltaTime);

            dayNightTimer += Time.deltaTime;
            if (dayNightTimer < 0) currentTime = timeOfDay.night;
            else if (dayNightTimer >= 0 && dayNightTimer <= 150) currentTime = timeOfDay.day;
            else if (dayNightTimer > 150 && dayNightTimer <= 300) currentTime = timeOfDay.night;
            else dayNightTimer = 0;
        }
    }

    void UpdateTimer()
    {
        float minutes = (timeElapsed / 60);
        int minutesI =  (int) minutes;
        float seconds = Mathf.FloorToInt(timeElapsed % 60);
        seconds = Mathf.Abs(seconds); // make not negative
        uiTimerText.text = string.Format("{0} : {1:00}", minutesI, seconds);
    }
}
