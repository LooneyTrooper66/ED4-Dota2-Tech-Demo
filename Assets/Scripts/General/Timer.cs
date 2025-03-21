using UnityEngine;

public class Timer : MonoBehaviour
{
    // need timer 
    // have day night cycle in here
    // connect this to a scriptable object to change states between day and night and to connect any time related things

    public RoundTimer roundTimer;


    private void Start()
    {
        roundTimer.timeElapsed = -60f;
        roundTimer.gamePlaying = false;
    }

    private void Update()
    {
        if (roundTimer.gamePlaying ==  true)
        {
            roundTimer.timeElapsed += Time.deltaTime;
            // need to add day/night cycle
            // add option to pause game with the gamePlaying bool
            // have round start as night for the -1 minute
        }
    }
}
