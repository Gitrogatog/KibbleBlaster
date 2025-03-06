namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class GravitySystem : System
{
    public Filter EntityFilter;
    public Filter RiseFallFilter;

    public GravitySystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<Gravity>()
            .Exclude<Grounded>()
            .Build();
        RiseFallFilter = FilterBuilder
            .Include<Position>()
            .Include<RiseFallGravity>()
            .Exclude<Grounded>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in RiseFallFilter.Entities)
        {
            Vector2 velocity = Has<Velocity>(entity) ? Get<Velocity>(entity).Value : Vector2.Zero;
            var riseFallGrav = Get<RiseFallGravity>(entity);
            float grav;
            if (velocity.Y < 0)
            {
                if (Has<IsJumping>(entity))
                {
                    grav = riseFallGrav.Rise;
                }
                else
                {
                    grav = riseFallGrav.JumpEndEarly;
                }

            }
            else
            {
                grav = riseFallGrav.Fall;
            }
            Set(entity, new Gravity(grav));
        }
        foreach (var entity in EntityFilter.Entities)
        {
            float gravity = Get<Gravity>(entity).Value;
            Vector2 velocity = Has<Velocity>(entity) ? Get<Velocity>(entity).Value : Vector2.Zero;
            velocity.Y += gravity;
            if (World.TryGetComponent<MaxFallSpeed>(entity, out MaxFallSpeed maxFallSpeed) && velocity.Y > maxFallSpeed.Value)
            {
                velocity.Y = maxFallSpeed.Value;
            }
            Set(entity, new Velocity(velocity));
        }

    }

}