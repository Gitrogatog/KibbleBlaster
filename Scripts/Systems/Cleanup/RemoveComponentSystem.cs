namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;

public class RemoveComponent<T> : System where T : unmanaged
{
    public Filter EntityFilter;
    public RemoveComponent(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<T>()
            .Build();

    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Remove<T>(entity);
        }
    }
}