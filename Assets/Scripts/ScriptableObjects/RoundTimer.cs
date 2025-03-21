using UnityEngine;

[CreateAssetMenu(fileName = "RoundTimer", menuName = "Scriptable Objects/RoundTimer")]
public class RoundTimer : ScriptableObject
{
    public float timeElapsed;
    public bool gamePlaying;
    public enum timeOfDay { day, night };
}
