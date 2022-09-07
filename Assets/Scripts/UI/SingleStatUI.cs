using UnityEngine;
using UnityEngine.UI;

public class SingleStatUI : MonoBehaviour
{
    public Text label;
    public Text valueLabel;

    public void Configure(string text, float value)
    {
        this.label.text = text;
        this.valueLabel.text = value.ToString();
    }
}
