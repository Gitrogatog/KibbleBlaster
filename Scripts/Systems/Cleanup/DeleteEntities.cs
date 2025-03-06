namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DeleteEntities<T> : System where T : unmanaged
{
    public Filter EntityFilter;
    public DeleteEntities(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<T>()
            .Build();

    }
    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in EntityFilter.Entities)
        {
            Set(entity, new MarkForDestroy());
        }
    }
}

public class DeleteEntities<T1, T2> : System where T1 : unmanaged where T2 : unmanaged
{
    public Filter EntityFilter;
    public DeleteEntities(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<T1>()
            .Include<T2>()
            .Build();

    }
    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in EntityFilter.Entities)
        {
            Set(entity, new MarkForDestroy());
        }
    }
}

public class DeleteEntitiesWithoutInRelation<T> : System where T : unmanaged
{
    public Filter EntityFilter;
    public DeleteEntitiesWithoutInRelation(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<NeedInRelation<T>>()
            .Build();

    }
    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in EntityFilter.Entities)
        {
            if (!HasInRelation<T>(entity))
                Set(entity, new MarkForDestroy());
        }
    }
}

public class DeleteEntitiesWithoutOutRelation<T> : System where T : unmanaged
{
    public Filter EntityFilter;
    public DeleteEntitiesWithoutOutRelation(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<NeedOutRelation<T>>()
            .Build();

    }
    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in EntityFilter.Entities)
        {
            if (!HasOutRelation<T>(entity))
                Set(entity, new MarkForDestroy());
        }
    }
}