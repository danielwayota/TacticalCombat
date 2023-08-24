using System.Collections.Generic;
using UnityEngine;

public abstract class Master : MonoBehaviour
{
    public string masterName = "";

    public List<Creature> creatures { get; protected set; } = new List<Creature>();

    public void SpawnCreatures(List<Vector3> spawnPoints, CreatureData[] creatures)
    {
        for (int i = 0; i < creatures.Length; i++)
        {
            if (i >= spawnPoints.Count)
            {
                Debug.Log("No more spawn points!");
                break;
            }

            CreatureData data = creatures[i];
            this.CreateCreature(data, spawnPoints[i]);
        }
    }

    protected void BeginTurnToAllCreatures()
    {
        foreach (var creature in this.creatures)
        {
            creature.BeginTurn();
        }
    }

    protected void CreateCreature(CreatureData creatureData, Vector3 worldPosition)
    {
        GameObject go = Instantiate(creatureData.prefab);
        Creature creature = go.GetComponent<Creature>();

        creature.transform.position = worldPosition;

        creature.AddInnerData(creatureData);

        this.AdoptCreature(creature);
        BattleManager.current.EmplaceCreature(creature);
    }

    public void AdoptCreature(Creature creature)
    {
        creature.SetMaster(this);
        this.creatures.Add(creature);
    }

    public void RemoveCreature(Creature creature)
    {
        this.creatures.Remove(creature);
    }

    public void OnCreatureDeath(Creature creature)
    {
        this.creatures.Remove(creature);
    }

    public bool HasAliveCreatures()
    {
        return this.creatures.Count != 0;
    }

    public abstract void BeginTurn();
}
