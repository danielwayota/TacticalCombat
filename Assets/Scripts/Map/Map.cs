using System.IO;
using System.Collections.Generic;

public enum TileType
{
    WALL,
    GROUND,
}

public class Map
{
    public int width { get; protected set; }
    public int height { get; protected set; }

    private TileType[,] tilesData;

    protected Map(TileType[,] tilesData)
    {
        this.tilesData = tilesData;
        this.width = this.tilesData.GetLength(0);  // 0 es el ancho.
        this.height = this.tilesData.GetLength(1); // 1 es el alto.
    }

    public TileType GetTileType(int x, int y)
    {
        if (x < 0 || y < 0)
        {
            return TileType.WALL;
        }

        if (x >= this.width || y >= this.height)
        {
            return TileType.WALL;
        }

        return this.tilesData[x, y];
    }

    public static Map CreateWithStringData(string mapData)
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
                }
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
}