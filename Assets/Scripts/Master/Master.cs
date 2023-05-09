using System.Collections.Generic;
using UnityEngine;

public abstract class Master : MonoBehaviour
{
    public string masterName = "";

    protected List<Creature> creatures = new List<Creature>();

    public void SpawnCreatures(List<Vector3> spawnPoints, GameObject[] creaturePrfbs)
    {
        for (int i = 0; i < creaturePrfbs.Length; i++)
        {
            if (i >= spawnPoints.Count)
            {
                Debug.Log("No more spawn points!");
                break;
            }

            GameObject prfb = creaturePrfbs[i];
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

        creature.transform.position = worldPosition;

        this.AdoptCreature(creature);
    }

    public void AdoptCreature(Creature creature)
    {
        creature.master = this;
        this.creatures.Add(creature);

        BattleManager.current.EmplaceCreature(creature, creature.transform.position);
    }

    public void OnCreatureDeath(Creature creature)
    {
        this.creatures.Remove(creature);

        BattleManager.current.OnCreatureDeath(creature);
    }

    public bool HasAliveCreatures()
    {
        return this.creatures.Count != 0;
    }

    public abstract void BeginTurn();
}
