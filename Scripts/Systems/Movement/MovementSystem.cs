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
            .Include<IntendedMove>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Vector2 intendedMove = Get<IntendedMove>(entity).Value;
            float moveSpeed = Has<MoveSpeed>(entity) ? Get<MoveSpeed>(entity).Value : 100f;
            Vector2 currentPos = Get<Position>(entity).Value;
            currentPos += moveSpeed * (float)delta.TotalSeconds * intendedMove;
            Set(entity, new Position(currentPos));
            GD.Print($"current pos: {currentPos}");
            Remove<IntendedMove>(entity);
        }
    }
}