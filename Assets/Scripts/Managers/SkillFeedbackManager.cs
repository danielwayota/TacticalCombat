using UnityEngine;

using System.Collections.Generic;

public class SkillFeedbackManager : MonoBehaviour, IMessageListener
{
    private List<SkillFeedbackUI> items;

    void Start()
    {
        MessageManager.current.AddListener(MessageTag.SKILL_DAMAGE, this);
        MessageManager.current.AddListener(MessageTag.SKILL_MISS, this);

        this.items = new List<SkillFeedbackUI>();

        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            SkillFeedbackUI feedbackUI = child.GetComponent<SkillFeedbackUI>();
            feedbackUI.Hide();

            this.items.Add(feedbackUI);
        }
    }

    public void Receive(Message msg)
    {
        if (msg is SkillDamageMessage)
        {
            SkillDamageMessage sm = msg as SkillDamageMessage;
            SkillFeedbackUI ui = this.GetNextItemForReceiver(sm.receiver);

            ui.ConfigureForDamage(sm.receiver, sm.damage, sm.critical);
        }

        if (msg is SkillMissMessage)
        {
            SkillMissMessage sm = msg as SkillMissMessage;
            SkillFeedbackUI ui = this.GetNextItemForReceiver(sm.receiver);

            ui.ConfigureForMiss(sm.receiver);
        }
    }

    public SkillFeedbackUI GetNextItemForReceiver(Creature receiver)
    {
        foreach (var fui in this.items)
        {
            if (fui.isHidden || fui.receiver == receiver)
            {
                return fui;
            }
        }

        GameObject sample = this.items[0].gameObject;
        GameObject copy = Instantiate(sample);

        copy.transform.SetParent(this.transform);

        SkillFeedbackUI newui = copy.GetComponent<SkillFeedbackUI>();
        this.items.Add(newui);
        newui.Hide();

        return newui;
    }
}