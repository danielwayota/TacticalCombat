using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public TextAsset mapData;

    private Master[] masters;
    private int turnIndex;

    void Start()
    {
        current = this;

        Map map = Map.CreateWithStringData(this.mapData.text);

        MapDisplay display = GameObject.FindObjectOfType<MapDisplay>();
        display.RenderMapData(map);

        this.masters = new Master[] {
            this.GetComponentInChildren<HumanMaster>(),
            this.GetComponentInChildren<AIMaster>(),
        };

        foreach (var master in this.masters)
        {
            master.Configure(map);
        }

        this.turnIndex = -1;
        this.NextTurn();
    }

    public void NextTurn()
    {
        this.turnIndex = (this.turnIndex + 1) % this.masters.Length;

        Master currentMaster = this.masters[this.turnIndex];
        TurnUI.current.SetCurrentTurnLabel(currentMaster.masterName);

        currentMaster.BeginTurn();
    }
}
