
namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class FaceVelocityToPlayerSystem : System
{
    public Filter EntityFilter;
    public Filter FacingFilter;

    public FaceVelocityToPlayerSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<SetVelocityToFacePlayer>()
            .Build();
        FacingFilter = FilterBuilder
            .Include<Position>()
            .Include<SetFacingToPlayer>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Vector2 position = Get<Position>(entity).RawValue;
            bool facing = position.X < GlobalData.playerPositionX;
            if (Has<Velocity>(entity))
            {
                var velocity = Get<Velocity>(entity).Value;
                velocity.X = MathF.Abs(velocity.X) * (facing ? 1 : -1);
                Set<Velocity>(entity, new Velocity(velocity));
            }
            if (Has<Facing>(entity))
            {
                Set(entity, new Facing(facing));
            }
            Remove<SetVelocityToFacePlayer>(entity);
        }
        foreach (var entity in FacingFilter.Entities)
        {
            Vector2 position = Get<Position>(entity).RawValue;
            bool facing = position.X < GlobalData.playerPositionX;
            Set(entity, new Facing(facing));
            Remove<SetFacingToPlayer>(entity);
        }
    }
}