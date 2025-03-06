namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class IdleAnimationSystem : System
{
    public Filter EntityFilter;

    public IdleAnimationSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<IdleAnimGroup>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            var name = Get<IdleAnimGroup>(entity).Idle;
            EntityMods.SetAnimation(World, entity, name, 1);
        }
    }
}