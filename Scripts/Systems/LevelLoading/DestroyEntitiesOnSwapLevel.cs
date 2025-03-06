namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DestroyEntitiesOnChangeLevel : System
{
    public Filter EntityFilter;

    public DestroyEntitiesOnChangeLevel(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<DestroyOnChangeLevel>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Destroy(entity);
        }
    }
}