using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;
using MyECS.Relations;

public static class MessagePrefabs
{
    public static Entity CreateMessage(World World)
    {
        var entity = World.CreateEntity();
        World.Set(entity, new MarkForDestroy());
        return entity;
    }
    public static Entity CreateDamage(World World, Entity target, int amount)
    {
        var entity = CreateMessage(World);
        World.Relate(entity, target, new Damage(amount));
        return entity;
    }
    public static Entity CreateInvincibility(World World, Entity target, float time)
    {
        var entity = EntityPrefabs.CreateTimer(World, time);
        World.Relate(entity, target, new Invincible());
        return entity;
    }
    public static Entity CreateDisableJump(World World, Entity target, float time)
    {
        var entity = EntityPrefabs.CreateTimer(World, time);
        World.Relate(entity, target, new DisableJump());
        return entity;
    }
    public static Entity ChangeLevel(World World, int level)
    {
        var entity = World.CreateEntity();
        World.Set(entity, new ChangeLevel(level));
        return entity;
    }
    // public static Entity CreateRetainState(World World, Entity target, float time)
    // {
    //     var entity = EntityPrefabs.CreateTimer(World, time);
    //     World.Relate(entity, target, new RetainState());
    //     return entity;
    // }
}