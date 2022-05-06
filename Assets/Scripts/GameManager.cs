using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject creaturePrfb;

    public TextAsset mapData;

    void Start()
    {
        Map map = Map.CreateWithStringData(this.mapData.text);

        MapDisplay display = GameObject.FindObjectOfType<MapDisplay>();
        display.RenderMapData(map);

        this.SpawnRandomCreatures(3, map, display);
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

            creatureCount--;
        }
    }
}
