using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour, IMessageListener
{
    public Text currentTurnLabel;

    public GameObject nextTurnBtn;

    void Start()
    {
        MessageManager.current.AddListener(MessageTag.NEXT_TURN, this);
    }

    public void Receive(Message msg)
    {
        NextTurnMessage ntm = msg as NextTurnMessage;
        this.currentTurnLabel.text = ntm.currentTurnMaster.masterName;

        if (ntm.currentTurnMaster is HumanMaster)
        {
            this.nextTurnBtn.SetActive(true);
        }
        else
        {
            this.nextTurnBtn.SetActive(false);
        }
    }
}
