using Godot;

public static class SpawnUtils
{
    public static Rect2 CreateLowerRect(Rect2 source, float thickness)
    {
        return new Rect2(new Vector2(source.Position.X, source.End.Y), new Vector2(source.Size.X, thickness));
    }
    public static Rect2 CreateUpperRect(Rect2 source, float thickness)
    {
        return new Rect2(new Vector2(source.Position.X, source.Position.Y - thickness), new Vector2(source.Size.X, thickness));
    }
    public static Rect2 CreateLeftRect(Rect2 source, float thickness, float percentLength = 1)
    {
        return new Rect2(new Vector2(source.Position.X - thickness, source.Position.Y), new Vector2(thickness, source.Size.Y * percentLength));
    }
    public static Rect2 CreateRightRect(Rect2 source, float thickness, float percentLength = 1)
    {
        return new Rect2(new Vector2(source.End.X, source.Position.Y), new Vector2(thickness, source.Size.Y * percentLength));
    }
}