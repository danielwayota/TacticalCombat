using UnityEngine;

using System.Collections.Generic;

public class MapPathFinder
{
    private int[,] distanceMap;
    private List<Vector2Int> path;
    private List<Vector2Int> area;

    private int mapWidth;
    private int mapHeight;

    public MapPathFinder()
    {
        this.path = new List<Vector2Int>();
        this.area = new List<Vector2Int>();
    }

    public void ConfigureForMap(Map map)
    {
        this.mapWidth = map.width;
        this.mapHeight = map.height;

        this.distanceMap = new int[map.width, map.height];

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                // Obtenemos el Tile en esta posición
                var tile = map.GetTileType(x, y);

                // Asignamos el valor de "onda"
                if (tile == TileType.WALL)
                    this.distanceMap[x, y] = -1;
                else
                    this.distanceMap[x, y] = 0;
            }
        }
    }

    private void ClearPathData()
    {
        // Clear map
        for (int x = 0; x < this.mapWidth; x++)
        {
            for (int y = 0; y < this.mapHeight; y++)
            {
                if (this.distanceMap[x, y] != -1)
                    this.distanceMap[x, y] = 0;
            }
        }

        this.path.Clear();
    }

    private void Visit(Queue<Vector2Int> visitedCells, int distance, int x, int y)
    {
        if (x < 0 || x >= this.mapWidth)
            return;

        if (y < 0 || y >= this.mapHeight)
            return;

        if (this.distanceMap[x, y] == 0)
        {
            this.distanceMap[x, y] = distance;
            visitedCells.Enqueue(new Vector2Int(x, y));
        }
    }

    private bool IsOutsideOfTheMap(int x, int y)
    {
        if (x < 0 || x >= this.mapWidth)
            return true;

        if (y < 0 || y >= this.mapHeight)
            return true;

        return false;
    }

    public List<Vector2Int> GetPath(int startX, int startY, int endX, int endY)
    {
        if (this.IsOutsideOfTheMap(startX, startY))
            return this.path;

        if (this.IsOutsideOfTheMap(endX, endY))
            return this.path;

        this.ClearPathData();
        this.ComputeDistanceMap(startX, startY, endX, endY);
        this.TracePath(new Vector2Int(startX, startY));

        return this.path;
    }

    private void ComputeDistanceMap(int startX, int startY, int endX, int endY)
    {
        if (this.distanceMap[endX, endY] != 0 || this.distanceMap[startX, startY] != 0)
        {
            // Sin solución
            return;
        }

        int distance = 1;

        Queue<Vector2Int> visitedCells = new Queue<Vector2Int>();
        this.distanceMap[endX, endY] = distance;
        visitedCells.Enqueue(new Vector2Int(endX, endY));

        while (visitedCells.Count != 0 && visitedCells.Count < 1000)
        {
            distance += 1;

            var cell = visitedCells.Dequeue();
            this.Visit(visitedCells, distance, cell.x, cell.y + 1); // UP
            this.Visit(visitedCells, distance, cell.x, cell.y - 1); // Down
            this.Visit(visitedCells, distance, cell.x + 1, cell.y); // Right
            this.Visit(visitedCells, distance, cell.x - 1, cell.y); // Left
        }
    }

    private void TracePath(Vector2Int localStart)
    {
        Vector2Int currentLocalTile = localStart;

        bool first = true;

        bool working = true;
        while (working)
        {
            var (x, y) = ((int)currentLocalTile.x, (int)currentLocalTile.y);
            var d = this.distanceMap[x, y];

            if (d == -1)
            {
                // Invalid
                return;
            }

            if (!first)
            {
                path.Add(currentLocalTile);
            }
            else
            {
                first = false;
            }

            working = false;
            if (this.distanceMap[x, y + 1] < d && this.distanceMap[x, y + 1] != -1)
            {
                currentLocalTile.Set(x, y + 1);
                working = true;
                continue;
            }
            if (this.distanceMap[x, y - 1] < d && this.distanceMap[x, y - 1] != -1)
            {
                currentLocalTile.Set(x, y - 1);
                working = true;
                continue;
            }
            if (this.distanceMap[x + 1, y] < d && this.distanceMap[x + 1, y] != -1)
            {
                currentLocalTile.Set(x + 1, y);
                working = true;
                continue;
            }
            if (this.distanceMap[x - 1, y] < d && this.distanceMap[x - 1, y] != -1)
            {
                currentLocalTile.Set(x - 1, y);
                working = true;
                continue;
            }
        }
    }

    public List<Vector2Int> GetReachableArea(Vector2Int center, float radius)
    {
        int range = Mathf.RoundToInt(radius);

        this.area.Clear();

        List<Vector2Int> mountainTiles = new List<Vector2Int>();
        this.OverlowMountain(center, range, mountainTiles);

        foreach (var point in mountainTiles)
        {
            if (Vector2Int.Distance(center, point) <= radius)
            {
                this.area.Add(point);
            }
        }

        return this.area;
    }

    private void OverlowMountain(Vector2Int point, int range, List<Vector2Int> result)
    {
        if (this.distanceMap[point.x, point.y] == -1) return;

        result.Add(point);

        if (range <= 0) return;

        this.OverlowMountain(point + Vector2Int.up, range - 1, result);
        this.OverlowMountain(point + Vector2Int.down, range - 1, result);
        this.OverlowMountain(point + Vector2Int.left, range - 1, result);
        this.OverlowMountain(point + Vector2Int.right, range - 1, result);
    }
}