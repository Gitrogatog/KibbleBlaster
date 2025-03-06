using System.Data;
using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;
using MyECS.Relations;

public static class AmbushPrefabs
{
    public static Entity CreateX(World World, ThingType thing, Vector2 triggerPos, Vector2 spawnPos, SpawnFacing facing)
    {
        GD.Print("made ambush x");
        var entity = World.CreateEntity();
        World.Set(entity, new SpawnEnemyOnAmbush(thing, spawnPos, facing));
        World.Set(entity, new AmbushOnPosition(triggerPos.X, false));
        return entity;
    }
    public static Entity CreateY(World World, ThingType thing, Vector2 triggerPos, Vector2 spawnPos, SpawnFacing facing)
    {
        GD.Print("made ambush y");
        var entity = World.CreateEntity();
        World.Set(entity, new SpawnEnemyOnAmbush(thing, spawnPos, facing));
        World.Set(entity, new AmbushOnPosition(triggerPos.Y, true));
        return entity;
    }
}