using UnityEngine;
using UnityEngine.UI;

public class SingleStatUI : MonoBehaviour
{
    public Text label;
    public Text baseValueLabel;
    public Text modValueLabel;

    public void Configure(string text, int baseValue, int value)
    {
        this.label.text = text;
        this.baseValueLabel.text = baseValue.ToString();

        int diff = value - baseValue;

        if (diff != 0)
        {
            this.modValueLabel.gameObject.SetActive(true);

            if (diff > 0)
            {
                this.modValueLabel.text = "+" + diff.ToString();
            }
            else if (diff < 0)
            {
                this.modValueLabel.text = diff.ToString();
            }
        }
        else
        {
            this.modValueLabel.gameObject.SetActive(false);
        }
    }

    public void Configure(string text, int baseValue)
    {
        this.label.text = text;
        this.baseValueLabel.text = baseValue.ToString();
    }
}
