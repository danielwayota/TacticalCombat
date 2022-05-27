using UnityEngine;

using System.Collections.Generic;

public class HumanMaster : Master
{
    public GameObject creaturePrfb;

    protected MapDisplay mapDisplay;

    protected override void Init()
    {
        this.mapDisplay = FindObjectOfType<MapDisplay>();

        this.SpawnRandomCreatures(3, this.map, this.mapDisplay);
    }

    public override void BeginTurn()
    {
        this.RechargeAllCreatures();
    }

    private void SpawnRandomCreatures(int count, Map map, MapDisplay display)
    {
        int creatureCount = count;
        int attempts = 0;

        List<Vector2Int> usedPositions = new List<Vector2Int>();

        while (creatureCount > 0 && attempts < 100)
        {
            attempts++;

            int x = Random.Range(0, map.width);
            int y = Random.Range(0, map.height);

            if (map.GetTileType(x, y) != TileType.GROUND)
                continue;

            bool positionUsed = false;

            foreach (var pos in usedPositions)
            {
                if (pos.x == x && pos.y == y)
                {
                    positionUsed = true;
                    break;
                }
            }

            if (positionUsed)
                continue;

            Vector2Int spawnPoint = new Vector2Int(x, y);
            usedPositions.Add(spawnPoint);

            GameObject go = Instantiate(this.creaturePrfb);
            Creature creature = go.GetComponent<Creature>();
            display.EmplaceCreature(creature, spawnPoint);

            this.creatures.Add(creature);

            creatureCount--;
        }
    }
}