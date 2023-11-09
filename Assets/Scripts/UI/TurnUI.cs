using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour, IMessageListener
{
    public Text currentTurnLabel;

    public GameObject nextTurnBtn;
    public GameObject fleeBtn;

    void Start()
    {
        MessageManager.current.AddListener(MessageTag.NEXT_TURN, this);

        this.nextTurnBtn.SetActive(false);
        this.fleeBtn.SetActive(false);
    }

    public void Receive(Message msg)
    {
        NextTurnMessage ntm = msg as NextTurnMessage;
        this.currentTurnLabel.text = ntm.currentTurnMaster.masterName;

        if (ntm.currentTurnMaster is HumanMaster)
        {
            this.nextTurnBtn.SetActive(true);
            this.fleeBtn.SetActive(true);
        }
        else
        {
            this.nextTurnBtn.SetActive(false);
            this.fleeBtn.SetActive(false);
        }
    }
}
