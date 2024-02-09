using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Map map { get; protected set; }
    private MapPathFinder pathFinder;

    private MapDisplay display;

    private List<Vector3> worldPathBuffer;
    private List<Vector3> areaBuffer;

    public List<Vector3> humanSpawnPoints;
    public List<Vector3> aiSpawnPoints;

    public List<Vector3> dynamicObstacles;

    private Vector3 mapOffset;

    private void PreConfigure()
    {
        this.humanSpawnPoints = new List<Vector3>();
        this.aiSpawnPoints = new List<Vector3>();

        this.worldPathBuffer = new List<Vector3>();
        this.areaBuffer = new List<Vector3>();

        this.dynamicObstacles = new List<Vector3>();

        this.display = GameObject.FindObjectOfType<MapDisplay>();
    }

    public void ConfigureWithStringData(string mapData)
    {
        this.PreConfigure();

        this.map = this.CreateMapWithStringData(mapData);
        this.display.RenderMapData(this.map);

        this.pathFinder = new MapPathFinder();
        this.pathFinder.ConfigureForMap(this.map);

        this.mapOffset = this.display.transform.position;
    }

    public void ConfigureWithTiledMap(TiledMap tiledMap)
    {
        this.PreConfigure();

        this.map = tiledMap.GenerateMapData();

        this.pathFinder = new MapPathFinder();
        this.pathFinder.ConfigureForMap(this.map);

        this.mapOffset = tiledMap.GetMapOffset();

        foreach (var transf in tiledMap.humanSpawnPoints)
        {
            this.humanSpawnPoints.Add(transf.position);
        }

        foreach (var transf in tiledMap.aiSpawnPoints)
        {
            this.aiSpawnPoints.Add(transf.position);
        }

        this.display.FocusOnMap(this.map, this.mapOffset);
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

        // Invertimos la coordenada Y de los puntos de Spawn
        for (int i = 0; i < this.humanSpawnPoints.Count; i++)
        {
            Vector3 point = this.humanSpawnPoints[i];
            point.y = (mapHeight - 1) - point.y;
            this.humanSpawnPoints[i] = point;
        }

        for (int i = 0; i < this.aiSpawnPoints.Count; i++)
        {
            Vector3 point = this.aiSpawnPoints[i];
            point.y = (mapHeight - 1) - point.y;
            this.aiSpawnPoints[i] = point;
        }

        TileType[,] finalMapTiles = new TileType[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                int invertedY = (mapHeight - 1) - y;
                finalMapTiles[x, y] = flatTilesData[invertedY * mapWidth + x];
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
        Vector3 local = world - this.mapOffset;

        int mapX = Mathf.FloorToInt(local.x);
        int mapY = Mathf.FloorToInt(local.y);

        return new Vector2Int(mapX, mapY);
    }

    public Vector3 LocalToWorld(Vector2Int local)
    {
        Vector3 localF = new Vector3(local.x, local.y, 0);

        return this.mapOffset + localF + (Vector3.one * 0.5f);
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
