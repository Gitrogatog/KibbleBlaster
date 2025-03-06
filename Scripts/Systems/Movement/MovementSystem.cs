namespace MyECS;
using System;
using System.Runtime.Intrinsics.X86;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class MovementSystem : System
{
    public Filter EntityFilter;

    public MovementSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<Velocity>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        bool hasDebugged = false;
        GD.Print("running movement system");
        foreach (var entity in EntityFilter.Entities)
        {
            //     Vector2 intendedMove = Get<IntendedMove>(entity).Value;
            //     float moveSpeed = Has<MoveSpeed>(entity) ? Get<MoveSpeed>(entity).Value : 100f;
            Vector2 velocity = Get<Velocity>(entity).Value;
            var pos = Get<Position>(entity);
            var movement = velocity * ((float)delta.TotalSeconds);
            var targetPos = pos + movement;
            // Vector2 currentPos = Get<Position>(entity).Value;
            if (Has<CharBody>(entity))
            {
                CharacterBody2D body = CharBodyStorage.GetClass(Get<CharBody>(entity).ID);
                body.Velocity = velocity;
                body.MoveAndSlide();
                Set(entity, new Position(body.GlobalPosition));
                // body.GetSlideCollision
            }
            else
            {
                GD.Print("checking debug");
                if (!hasDebugged && Has<DebugPosition>(entity))
                {
                    hasDebugged = true;
                    GD.Print($"pos {pos.RawValue} new pos {targetPos.RawValue} velocity {velocity} movement {movement}");
                }
                // pos += velocity * (float)delta.TotalSeconds;

                Set(entity, targetPos);
            }

            // GD.Print($"current pos: {currentPos}");
            Remove<IntendedMove>(entity);
        }
    }
}