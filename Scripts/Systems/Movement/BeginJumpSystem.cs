namespace MyECS;
using System;
using System.Runtime.Intrinsics.X86;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Messages;
using MyECS.Relations;

public class BeginJump : System
{
    public Filter EntityFilter;

    public BeginJump(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<CanJump>()
            .Include<Grounded>()
            .Include<AttemptJumpThisFrame>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            if (HasInRelation<DisableJump>(entity))
            {
                continue;
            }
            // GD.Print("Attempting jump!");
            float jumpForce = Get<CanJump>(entity).Velocity;
            Vector2 velocity = Has<Velocity>(entity) ? Get<Velocity>(entity).Value : Vector2.Zero;
            velocity.Y = -jumpForce;
            Set(entity, new Velocity(velocity));
            Set(entity, new DidJumpThisFrame());
            Set(entity, new IsJumping());
            Remove<Grounded>(entity);
            Send(new PlaySoundMessage(SoundName.PlayerJump));
            // if (Has<RiseFallGravity>(entity))
            // {
            //     float riseGrav = Get<RiseFallGravity>(entity).RiseValue;
            //     Set(entity, new Gravity(riseGrav));
            // }
        }
    }
}