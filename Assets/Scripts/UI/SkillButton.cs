using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Text label;
    public Button btn;

    public void Configure(string text, UnityAction onClick)
    {
        this.label.text = text;

        this.btn.onClick.RemoveAllListeners();
        this.btn.onClick.AddListener(onClick);
    }
}
