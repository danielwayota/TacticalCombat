using System.Collections.Generic;
using UnityEngine;

public abstract class Master : MonoBehaviour
{
    public GameObject[] creatureTeamPrfbs;

    public string masterName = "";

    protected List<Creature> creatures = new List<Creature>();

    public void SpawnCreatures(List<Vector3> spawnPoints)
    {
        for (int i = 0; i < this.creatureTeamPrfbs.Length; i++)
        {
            if (i >= spawnPoints.Count)
            {
                Debug.Log("No more spawn points!");
                break;
            }

            GameObject prfb = this.creatureTeamPrfbs[i];
            this.CreateCreature(prfb, spawnPoints[i]);
        }
    }

    protected void BeginTurnToAllCreatures()
    {
        foreach (var creature in this.creatures)
        {
            creature.BeginTurn();
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

    public void OnCreatureDeath(Creature creature)
    {
        this.creatures.Remove(creature);

        GameManager.current.OnCreatureDeath(creature);
    }

    public bool HasAliveCreatures()
    {
        return this.creatures.Count != 0;
    }

    public abstract void BeginTurn();
}
