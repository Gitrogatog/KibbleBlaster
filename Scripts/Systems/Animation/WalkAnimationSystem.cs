namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class WalkAnimationSystem : System
{
    public Filter EntityFilter;
    const float minSpeed = 1f;
    public WalkAnimationSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<WalkAnimGroup>()
            .Include<Velocity>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            if (Mathf.Abs(Get<Velocity>(entity).Value.X) > minSpeed)
            {
                var name = Get<WalkAnimGroup>(entity).Walk;
                EntityMods.SetAnimation(World, entity, name, 2);
            }

        }
    }
}