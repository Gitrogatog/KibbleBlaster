namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class TimedAnimationSystem : System
{
    public Filter EntityFilter;

    public TimedAnimationSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<TimedAnimGroup>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            var anim = Get<TimedAnimGroup>(entity);
            EntityMods.SetAnimation(World, entity, anim.Animation, anim.Priority);
        }
    }
}