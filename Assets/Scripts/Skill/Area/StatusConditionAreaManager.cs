using UnityEngine;

using System.Collections.Generic;

public class StatusConditionAreaManager : MonoBehaviour, IMessageListener
{
    public static StatusConditionAreaManager current;

    protected List<StatusConditionArea> areas = new List<StatusConditionArea>();

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        MessageManager.current.AddListener(MessageTag.NEXT_TURN, this);
    }

    public void AddArea(StatusConditionArea area)
    {
        this.areas.Add(area);
    }

    public void Receive(Message msg)
    {
        for (int i = 0; i < this.areas.Count; i++)
        {
            StatusConditionArea area = this.areas[i];

            area.ConsumeOneTurn();
            area.TryToResolveArea();

            if (area.isDepleted)
            {
                this.areas.RemoveAt(i);
            }
        }
    }
}