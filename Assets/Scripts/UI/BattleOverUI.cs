using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class BattleOverUI : MonoBehaviour, IMessageListener
{
    public Text winnerLabel;
    public GameObject uiPanel;

    void Start()
    {
        MessageManager.current.AddListener(MessageTag.BATTLE_OVER, this);
        this.uiPanel.SetActive(false);
    }

    public void Receive(Message msg)
    {
        BattleOverMessage bom = msg as BattleOverMessage;

        StartCoroutine(this.DisplayWinner(bom));
    }

    private IEnumerator DisplayWinner(BattleOverMessage bom)
    {
        yield return new WaitForSeconds(1.2f);

        this.uiPanel.SetActive(true);
        this.winnerLabel.text = bom.winner.masterName + " wins!";
    }

}