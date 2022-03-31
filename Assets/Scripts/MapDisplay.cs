using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct MapTilePair
{
    public TileType type;
    public Tile visualTile;
}

public class MapDisplay : MonoBehaviour
{
    public MapTilePair[] mapTilePairs;
    public Tilemap targetTilemap;

    public Camera gameCamera;
    public Transform cursor;

    private Map map;

    public void RenderMapData(Map mapdata)
    {
        this.map = mapdata;

        for (int x = 0; x < this.map.width; x++)
        {
            for (int y = 0; y < this.map.height; y++)
            {
                TileType type = this.map.GetTileType(x, y);

                Tile tile = this.GetTileForType(type);

                this.targetTilemap.SetTile(new Vector3Int(x, -y, 0), tile);
            }
        }
    }

    private Tile GetTileForType(TileType type)
    {
        foreach (var pair in this.mapTilePairs)
        {
            if (pair.type == type)
                return pair.visualTile;
        }

        Debug.LogError("No hay tile para: " + type);
        return null;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Vector3 world = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
            var local = this.WorldToLocal(world);

            if (this.map.GetTileType(local.x, local.y) == TileType.GROUND)
            {
                this.cursor.gameObject.SetActive(true);
                this.cursor.position = this.LocalToWorld(local);
            }
            else
            {
                this.cursor.gameObject.SetActive(false);
            }
        }
    }

    private Vector2Int WorldToLocal(Vector3 world)
    {
        Vector3 local = world - this.transform.position;

        int mapX = Mathf.FloorToInt(local.x);
        int mapY = Mathf.FloorToInt(local.y);

        return new Vector2Int(mapX, -mapY);
    }

    private Vector3 LocalToWorld(Vector2Int local)
    {
        Vector3 localF = new Vector3(local.x, -local.y, 0);

        return this.transform.position + localF + (Vector3.one * 0.5f);
    }
}