namespace MyECS;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class AimAnimationSystem : System
{
    public Filter EntityFilter;
    const int PRIORITY = 3;
    const float MINWALKSPEED = 5f;
    const float MINFALLSPEED = 20f;

    public AimAnimationSystem(World world) : base(world)
    {

        EntityFilter = FilterBuilder
            .Include<AimAnimGroup>()
            .Include<Velocity>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Vector2 velocity = Get<Velocity>(entity).Value;
            Vertical aimDir = Has<VerticalAimDirection>(entity) ? Get<VerticalAimDirection>(entity).Value : Vertical.Neutral;
            var animGroup = Get<AimAnimGroup>(entity);
            SpriteAnimInfoID name;
            if (!Has<Grounded>(entity))
            {
                // GD.Print($"rise.! velocity is: ")
                bool rising = velocity.Y < MINFALLSPEED;
                name = aimDir switch
                {
                    Vertical.Up => rising ? animGroup.RiseUp : animGroup.FallUp,
                    Vertical.Down => rising ? animGroup.RiseDown : animGroup.FallDown,
                    Vertical.Neutral => rising ? animGroup.RiseNeutral : animGroup.FallNeutral,
                    _ => animGroup.RiseNeutral
                };

            }
            else if (Mathf.Abs(velocity.X) > MINWALKSPEED)
            {
                name = aimDir switch
                {
                    Vertical.Up => animGroup.WalkUp,
                    Vertical.Down => animGroup.WalkDown,
                    _ => animGroup.WalkNeutral
                };
            }
            else
            {
                name = aimDir switch
                {
                    Vertical.Up => animGroup.IdleUp,
                    Vertical.Down => animGroup.IdleDown,
                    _ => animGroup.IdleNeutral
                };
            }
            EntityMods.SetAnimation(World, entity, name, PRIORITY);
        }
    }
}