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
    public Light directionalLight;
    public Color dayColour;
    public Color nightColour;



    private void Start()
    {
        timeElapsed = -5f; // set to -60 for actual game
        dayNightTimer = timeElapsed;
        gamePlaying = true; // set this to false when in menus
        currentTime = timeOfDay.night;
        directionalLight.color = nightColour;
    }

    private void Update()
    {
        if (gamePlaying)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimer();

            // day/night cycle
            dayNightTimer += Time.deltaTime;
            if (dayNightTimer < 0)
            {
                currentTime = timeOfDay.night;
                directionalLight.color = nightColour;
            }
            else if (dayNightTimer >= 0 && dayNightTimer <= 150)
            {
                currentTime = timeOfDay.day;
                directionalLight.color = dayColour;
            }
            else if (dayNightTimer > 150 && dayNightTimer <= 300)
            {
                currentTime = timeOfDay.night;
                directionalLight.color = nightColour;
            }
            else { dayNightTimer = 0; }

            // add option to pause game with the gamePlaying bool
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
