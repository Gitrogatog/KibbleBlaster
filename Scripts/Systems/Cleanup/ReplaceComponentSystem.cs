namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;

public class ReplaceComponent<T1, T2> : System where T1 : unmanaged where T2 : unmanaged
{
    public Filter EntityFilter;
    public ReplaceComponent(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<T1>()
            .Build();

    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Remove<T1>(entity);
            Set(entity, new T2());
        }
    }
}