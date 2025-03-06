namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class AmbushAnimationSystem : System
{
    public Filter EntityFilter;

    public AmbushAnimationSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<AmbushAnimGroup>()
            .Include<AmbushOnPosition>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            var name = Get<AmbushAnimGroup>(entity).Idle;
            EntityMods.SetAnimation(World, entity, name, 10);
        }
    }
}