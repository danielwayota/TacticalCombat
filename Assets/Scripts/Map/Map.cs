
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

    public Map(TileType[,] tilesData)
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
}