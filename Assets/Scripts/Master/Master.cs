using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Master : MonoBehaviour
{
    public string masterName = "";

    protected Map map;

    protected List<Creature> creatures = new List<Creature>();

    public void Configure(Map gameMap)
    {
        this.map = gameMap;
        this.Init();
    }

    protected void RechargeAllCreatures()
    {
        foreach (var creature in this.creatures)
        {
            creature.Recharge();
        }
    }

    protected abstract void Init();
    public abstract void BeginTurn();
}
