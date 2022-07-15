using UnityEngine;

using System.Collections.Generic;

public class HumanMaster : Master
{
    protected MapDisplay mapDisplay;

    public override void BeginTurn()
    {
        this.RechargeAllCreatures();
    }
}