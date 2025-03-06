namespace MyECS;
using System;
using System.Runtime;
using MoonTools.ECS;

public static class MoontoolsECSExtensions
{
    public static bool TryGetComponent<T>(this World world, Entity entity, out T component) where T : unmanaged
    {
        if (world.Has<T>(entity))
        {
            component = world.Get<T>(entity);
            return true;
        }
        component = default;
        return false;
    }
    public static bool TryGetOutRelation<T>(this World world, Entity entity, out Entity target, out T relation) where T : unmanaged
    {
        if (world.HasOutRelation<T>(entity))
        {
            target = world.OutRelationSingleton<T>(entity);
            relation = world.GetRelationData<T>(entity, target);
            return true;
        }
        target = default;
        relation = default;
        return false;
    }
    public static bool TryGetOutRelation<T>(this World world, Entity entity, out Entity target) where T : unmanaged
    {
        if (world.HasOutRelation<T>(entity))
        {
            target = world.OutRelationSingleton<T>(entity);
            return true;
        }
        target = default;
        return false;
    }
    public static bool TryGetInRelation<T>(this World world, Entity entity, out Entity target, out T relation) where T : unmanaged
    {
        if (world.HasInRelation<T>(entity))
        {
            target = world.InRelationSingleton<T>(entity);
            relation = world.GetRelationData<T>(target, entity);
            return true;
        }
        target = default;
        relation = default;
        return false;
    }
    public static bool TryGetInRelation<T>(this World world, Entity entity, out Entity target) where T : unmanaged
    {
        if (world.HasInRelation<T>(entity))
        {
            target = world.InRelationSingleton<T>(entity);
            return true;
        }
        target = default;
        return false;
    }
}
