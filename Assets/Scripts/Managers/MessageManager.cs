using UnityEngine;

using System.Collections.Generic;

public class MessageManager : MonoBehaviour
{
    public static MessageManager current;

    protected Dictionary<MessageTag, List<IMessageListener>> listenerDict = new Dictionary<MessageTag, List<IMessageListener>>();

    void Awake()
    {
        current = this;
    }

    public void AddListener(MessageTag tag, IMessageListener listener)
    {
        if (this.listenerDict.ContainsKey(tag) == false)
        {
            this.listenerDict.Add(tag, new List<IMessageListener>());
        }

        this.listenerDict[tag].Add(listener);
    }

    public void RemoveListener(MessageTag tag, IMessageListener listener)
    {
        if (this.listenerDict.ContainsKey(tag) == false)
        {
            return;
        }

        this.listenerDict[tag].Remove(listener);
    }

    public void Send(Message msg)
    {
        if (this.listenerDict.ContainsKey(msg.tag) == false)
        {
            // Debug.Log($"Message with unregistered tag: {msg.tag}");
            return;
        }

        List<IMessageListener> listeners = this.listenerDict[msg.tag];
        foreach (var listener in listeners)
        {
            listener.Receive(msg);
        }
    }
}