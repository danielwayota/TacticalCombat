using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
    public static TurnUI current;

    public Text currentTurnLabel;

    void Awake()
    {
        current = this;
    }

    public void SetCurrentTurnLabel(string name)
    {
        this.currentTurnLabel.text = name;
    }
}
