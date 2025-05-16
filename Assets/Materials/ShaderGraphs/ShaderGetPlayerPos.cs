using UnityEngine;

public class ShaderGetPlayerPos : MonoBehaviour
{
    public Material distCullMaterial;
    public HeroStats stats;
    public Timer time;
    float visionRn;


    private void FixedUpdate()
    {
        distCullMaterial.SetVector("_PlayerPosition", transform.position);

        if (time.currentTime == Timer.timeOfDay.day && visionRn != stats.visionDay) visionRn = stats.visionDay;
        if (time.currentTime == Timer.timeOfDay.night && visionRn != stats.visionNight) visionRn = stats.visionNight;

        distCullMaterial.SetFloat("_PlayerVision", visionRn);
    }
}
