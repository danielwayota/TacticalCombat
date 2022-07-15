using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Master : MonoBehaviour
{
    public GameObject creaturePrfb;

    public string masterName = "";

    protected List<Creature> creatures = new List<Creature>();

    public void SpawnCreatures(List<Vector3> spawnPoints)
    {
        foreach (var point in spawnPoints)
        {
            this.CreateCreature(this.creaturePrfb, point);
        }
    }

    protected void RechargeAllCreatures()
    {
        foreach (var creature in this.creatures)
        {
            creature.Recharge();
        }
    }

    protected void CreateCreature(GameObject creaturePrfb, Vector3 worldPosition)
    {
        GameObject go = Instantiate(creaturePrfb);
        Creature creature = go.GetComponent<Creature>();
        creature.master = this;

        GameManager.current.EmplaceCreature(creature, worldPosition);

        this.creatures.Add(creature);
    }

    public abstract void BeginTurn();
}
