using UnityEngine;

public class StatusConditionListUI : MonoBehaviour
{
    public DynamicItemUIList singleStatusConditionList;

    public void ConfigureAndHide()
    {
        this.singleStatusConditionList.ConfigureAndHide();
    }

    public void DisplayStatusConditions(StatusCondition[] conditions)
    {
        this.singleStatusConditionList.HideAll();

        if (conditions.Length != 0)
        {
            this.gameObject.SetActive(true);

            foreach (var condition in conditions)
            {
                SingleStatusConditionUI single = this.singleStatusConditionList.GetNextItemAndActivate<SingleStatusConditionUI>();
                single.Configure(condition);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}