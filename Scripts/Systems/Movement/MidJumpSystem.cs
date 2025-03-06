namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class MidJumpSystem : System
{
    public Filter EntityFilter;

    public MidJumpSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<IsJumping>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            if (!Has<HoldJumpThisFrame>(entity) || Get<Velocity>(entity).Value.Y > 0)
            {
                // jump ended
                Remove<IsJumping>(entity);
                //     if (Has<RiseFallGravity>(entity))
                //     {
                //         float fallGrav = Get<RiseFallGravity>(entity).FallValue;
                //         Set(entity, new Gravity(fallGrav));
                //     }
                // }
            }
        }
    }
}