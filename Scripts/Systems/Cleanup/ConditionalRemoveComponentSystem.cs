namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;

public class ConditionalRemoveComponent<T1, T2> : System where T1 : unmanaged where T2 : unmanaged
{
    public Filter EntityFilter;
    public ConditionalRemoveComponent(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<T1>()
            .Include<T2>()
            .Build();

    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Remove<T2>(entity);
        }
    }
}