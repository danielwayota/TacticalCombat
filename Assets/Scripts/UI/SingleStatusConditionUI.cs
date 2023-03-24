using UnityEngine;
using UnityEngine.UI;

public class SingleStatusConditionUI : MonoBehaviour
{
    public Text statusConditionLabel;
    public Text statusConditionTurnsLabel;

    public void Configure(StatusCondition condition)
    {
        this.statusConditionLabel.text = condition.conditionName;
        this.statusConditionTurnsLabel.text = condition.remainingTurns.ToString();
    }
}