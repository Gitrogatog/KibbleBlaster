using Godot;
namespace CustomTilemap;
public class Tilemap
{
    int xTiles;
    int yTiles;
    int totalTiles;
    public int XTiles => xTiles;
    public int YTiles => yTiles;
    public int TotalTiles => totalTiles;
    public Rect2I Bounds => new Rect2I(0, 0, xTiles, yTiles);
    public Vector2I tileSize;
    Vector2 flippedSize;
    public SimpleList<Vector2I> Sprites;
    public Tilemap(int maxTiles, Vector2I tileSize)
    {
        GD.Print($"max tiles: {maxTiles} tile size {tileSize}");
        Sprites = new SimpleList<Vector2I>(maxTiles);
        this.tileSize = tileSize;
        this.flippedSize = new Vector2(1f / tileSize.X, 1f / tileSize.Y);
    }
    public void Resize(int x, int y)
    {
        xTiles = x;
        yTiles = y;
        totalTiles = x * y;
        Sprites.SetLength(totalTiles);
    }
    public void ClearSprites()
    {
        for (int i = 0; i < totalTiles; i++)
        {
            Sprites[i] = -Vector2I.One;
        }
    }
    public int GetIndex(int x, int y)
    {
        return (y * xTiles) + x;
    }
    public Vector2I GetXY(int idx)
    {
        return new Vector2I(idx % xTiles, idx / xTiles);
    }
    public void SetSprite(int x, int y, Vector2I spritePosOnSheet)
    {
        int id = GetIndex(x, y);
        Sprites[id] = spritePosOnSheet;
    }
    public Rect2I GetTilesWithinBox(Rect2 box)
    {
        var scaledRect = new Rect2I(
            Mathf.Max(Mathf.FloorToInt(box.Position.X * flippedSize.X) - 1, 0),
            Mathf.Max(Mathf.FloorToInt(box.Position.Y * flippedSize.Y) - 1, 0),
            Mathf.Min(Mathf.FloorToInt(box.End.X * flippedSize.X) + 1, xTiles),
           Mathf.Min(Mathf.FloorToInt(box.End.Y * flippedSize.Y) + 1, yTiles)
        );
        return scaledRect;
    }
}