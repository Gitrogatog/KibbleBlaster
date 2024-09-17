using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public static class EntityPrefabs
{
    public static Entity CreatePlayer(World World)
    {
        var entity = World.CreateEntity();
        World.Set(entity, new ColoredRect(new Vector2(30, 30), Colors.YellowGreen));
        World.Set(entity, new Position(new Vector2(200, 100)));
        World.Set(entity, new MoveSpeed(20f));
        World.Set(entity, new ControlledByPlayer());
        AddCollider(World, entity, 0, 0, 30, 30);
        return entity;
    }
    public static Entity AddCollider(World World, Entity entity, float xCenter, float yCenter, float width, float height)
    {
        Entity collider = World.CreateEntity();
        World.Set(collider, new AABB(xCenter, yCenter, width, height));
        World.Relate(entity, collider, new HasCollider());
        return collider;
    }
}