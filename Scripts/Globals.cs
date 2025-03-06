using System;
using System.Collections.Generic;
using Godot;
public static class GlobalAssets
{
    public static PackedScene[] charBodies;
    public static PackedScene charBody;
    public static PackedScene GetBody(CharBodyGroup group)
    {
        int id = (int)group;
        return charBodies[id];
    }
}

public static class GlobalNodes
{
    public static Node2D Root;
    public static Viewport Viewport;
    public static PhysicsDirectSpaceState2D StateSpace;
}

public static class GlobalData
{

    public static int tileSize;
    public static Vector2 screenSize;
    public static float playerPositionX;
    public static float playerPositionY;
    // public static void InitRandom

}

