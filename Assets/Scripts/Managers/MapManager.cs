using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public TextAsset mapData;

    public Map map { get; protected set; }
    private MapPathFinder pathFinder;

    private MapDisplay display;

    private List<Vector3> worldPathBuffer;
    private List<Vector3> areaBuffer;

    public List<Vector3> humanSpawnPoints;
    public List<Vector3> aiSpawnPoints;

    public List<Vector3> dynamicObstacles;

    public void Configure()
    {
        this.humanSpawnPoints = new List<Vector3>();
        this.aiSpawnPoints = new List<Vector3>();

        this.worldPathBuffer = new List<Vector3>();
        this.areaBuffer = new List<Vector3>();

        this.dynamicObstacles = new List<Vector3>();

        this.display = GameObject.FindObjectOfType<MapDisplay>();

        this.map = this.CreateMapWithStringData(this.mapData.text);
        this.display.RenderMapData(this.map);

        this.pathFinder = new MapPathFinder();
        this.pathFinder.ConfigureForMap(this.map);
    }

    public Map CreateMapWithStringData(string mapData)
    {
        StringReader reader = new StringReader(mapData);

        int mapWidth = 0;
        int mapHeight = 0;

        List<TileType> flatTilesData = new List<TileType>();

        while (true)
        {
            string line = reader.ReadLine();
            if (line == null)
                break;

            line = line.Trim();
            // Línea vacía. Ignorar.
            if (line.Length == 0)
                continue;

            mapWidth = line.Length;
            mapHeight++;

            int x = 0;
            foreach (var letter in line)
            {
                switch (letter)
                {
                    case '#':
                        flatTilesData.Add(TileType.WALL);
                        break;
                    case '.':
                        flatTilesData.Add(TileType.GROUND);
                        break;
                    case 'H':
                        flatTilesData.Add(TileType.GROUND);
                        this.humanSpawnPoints.Add(this.LocalToWorld(new Vector2Int(x, mapHeight - 1)));
                        break;
                    case 'E':
                        flatTilesData.Add(TileType.GROUND);
                        this.aiSpawnPoints.Add(this.LocalToWorld(new Vector2Int(x, mapHeight - 1)));
                        break;
                    default:
                        throw new System.Exception("Invalid map data character: " + letter);
                }

                x++;
            }
        }

        TileType[,] finalMapTiles = new TileType[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                finalMapTiles[x, y] = flatTilesData[y * mapWidth + x];
            }
        }

        return new Map(finalMapTiles);
    }

    public List<Vector3> PredictWorldPathFor(Vector3 worldStart, Vector3 worldTarget)
    {
        foreach (var obstacle in this.dynamicObstacles)
        {
            Vector2Int local = this.WorldToLocal(obstacle);
            this.pathFinder.PutObstacle(local);
        }

        Vector2Int localStart = this.WorldToLocal(worldStart);
        Vector2Int localTarget = this.WorldToLocal(worldTarget);

        List<Vector2Int> path = this.pathFinder.GetPath(localStart.x, localStart.y, localTarget.x, localTarget.y);

        this.worldPathBuffer.Clear();
        foreach (var point in path)
        {
            this.worldPathBuffer.Add(this.LocalToWorld(point));
        }

        foreach (var obstacle in this.dynamicObstacles)
        {
            Vector2Int local = this.WorldToLocal(obstacle);
            this.pathFinder.RemoveObstacle(local);
        }

        return this.worldPathBuffer;
    }

    public List<Vector3> PredictAreaFor(Vector3 worldCenter, float radius)
    {
        Vector2Int localCenter = this.WorldToLocal(worldCenter);
        List<Vector2Int> localArea = this.pathFinder.GetReachableArea(localCenter, radius);

        this.areaBuffer.Clear();
        foreach (var localPoint in localArea)
        {
            this.areaBuffer.Add(this.LocalToWorld(localPoint));
        }

        return this.areaBuffer;
    }

    public bool IsInsideArea(List<Vector3> area, Vector3 world)
    {
        Vector3 worldTile = this.SnapToTile(world);

        foreach (var point in area)
        {
            if (worldTile == point)
            {
                return true;
            }
        }

        return false;
    }

    public Vector2Int WorldToLocal(Vector3 world)
    {
        Vector3 local = world - this.display.transform.position;

        int mapX = Mathf.FloorToInt(local.x);
        int mapY = Mathf.FloorToInt(local.y);

        return new Vector2Int(mapX, -mapY);
    }

    public Vector3 LocalToWorld(Vector2Int local)
    {
        Vector3 localF = new Vector3(local.x, -local.y, 0);

        return this.display.transform.position + localF + (Vector3.one * 0.5f);
    }

    public bool IsAGroundTile(Vector3 worldPoint)
    {
        var local = this.WorldToLocal(worldPoint);
        return this.map.GetTileType(local.x, local.y) == TileType.GROUND;
    }

    public Vector3 SnapToTile(Vector3 worldPoint)
    {
        var local = this.WorldToLocal(worldPoint);
        return this.LocalToWorld(local);
    }

    public bool AreSameTile(Vector3 a, Vector3 b)
    {
        Vector2Int aa = this.WorldToLocal(a);
        Vector2Int bb = this.WorldToLocal(b);

        return aa == bb;
    }
}
