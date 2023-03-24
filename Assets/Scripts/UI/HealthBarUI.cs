using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthSlider;
    public Text healthValuesLabel;

    public void SetHealth(int current, int max)
    {
        this.healthSlider.value = current / (float)max;
        this.healthValuesLabel.text = current + " / " + max;
    }
}