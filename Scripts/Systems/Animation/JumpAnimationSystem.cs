namespace MyECS;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class JumpAnimationSystem : System
{
    public Filter EntityFilter;
    public Filter VerticalFilter;
    const int priority = 3;

    public JumpAnimationSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<JumpAnimGroup>()
            .Include<Velocity>()
            .Exclude<Grounded>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            float ySpeed = Get<Velocity>(entity).Value.Y;
            if (ySpeed <= 0)
            {
                var name = Get<JumpAnimGroup>(entity).Up;
                EntityMods.SetAnimation(World, entity, name, priority);
            }
            else
            {
                var name = Get<JumpAnimGroup>(entity).Down;
                EntityMods.SetAnimation(World, entity, name, priority);
            }
        }
    }
}