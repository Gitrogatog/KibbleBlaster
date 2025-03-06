namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DisableJumpAfterLandingSystem : System
{
    public Filter EntityFilter;

    public DisableJumpAfterLandingSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<CanJump>()
            .Include<DisableJumpAfterLanding>()
            .Include<Grounded>()
            .Exclude<WasGroundedLastFrame>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            float disableJumpTime = Get<DisableJumpAfterLanding>(entity).Time;
            MessagePrefabs.CreateDisableJump(World, entity, disableJumpTime);
        }
    }
}