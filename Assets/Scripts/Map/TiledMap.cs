using UnityEngine;
using UnityEngine.Tilemaps;

public class TiledMap : MonoBehaviour
{
    public Tilemap tilemapToUse;

    public Transform[] humanSpawnPoints;
    public Transform[] aiSpawnPoints;

    public Map GenerateMapData()
    {
        int w = this.tilemapToUse.size.x;
        int h = this.tilemapToUse.size.y;

        TileType[,] matrix = new TileType[w, h];

        Vector3 offset = this.GetMapOffset();

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector3 local = new Vector3(x, y, 0);
                Vector3Int tileWorldPos = Vector3Int.FloorToInt(offset + local);

                TileBase tile = this.tilemapToUse.GetTile(tileWorldPos);

                if (tile != null)
                {
                    matrix[x, y] = TileType.WALL;
                }
                else
                {
                    matrix[x, y] = TileType.GROUND;
                }
            }
        }

        return new Map(matrix);
    }

    public Vector3 GetMapOffset()
    {
        return this.tilemapToUse.localBounds.min;
    }
}