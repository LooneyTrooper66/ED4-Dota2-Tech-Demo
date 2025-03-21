using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider healthBar3D;


    public void Start3DSlider(float maxVal)
    {
        healthBar3D.maxValue = maxVal;
        healthBar3D.value = maxVal;
    }

    public void Update3DSlider(float val)
    {
        healthBar3D.value = val;
    }
}
