
using System;
using Godot;
public static class Utilities
{
    public static TimeSpan DeltaToTimeSpan(double delta)
    {
        long ticks = (long)(delta * 10000000);
        TimeSpan span = new TimeSpan(ticks);
        return span;
    }
    public static float LerpDecay(float a, float b, float decay, float dt)
    {
        return b + (a - b) * MathF.Exp(-decay * dt);
    }
    public static Vector2 LerpDecay(Vector2 a, Vector2 b, float decay, float dt)
    {
        return b + (a - b) * MathF.Exp(-decay * dt);
    }
    public static Rect2 AABBtoRect(AABB aabb)
    {
        return new Rect2(aabb.TopLeft, aabb.Width, aabb.Height);
    }
    public static Rect2 CenterRect(Rect2 rect)
    {
        return new Rect2(rect.Position - rect.Size * 0.5f, rect.Size);
    }
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return (a * t) + (b - a) * (1 - t);
    }
    public static float Lerp(float a, float b, float t)
    {
        return (a * t) + (b - a) * (1 - t);
    }
}

public static class RandomUtils
{
    static MoonTools.ECS.Random random = new MoonTools.ECS.Random();
    public static float RandomF()
    {
        return random.NextSingle();
    }
    public static float RandomF(float max)
    {
        return max * random.NextSingle();
    }
    public static float RandomF(float min, float max)
    {
        return min + (max - min) * random.NextSingle();
    }
    public static int Random()
    {
        return random.Next();
    }
    public static int Random(int max)
    {
        return random.Next(max);
    }
    public static int Random(int min, int max)
    {
        return random.Next(min, max);
    }
}