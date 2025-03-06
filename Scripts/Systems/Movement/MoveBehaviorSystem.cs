namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class MoveBehaviorSystem : System
{
    public Filter ContinuousMoveFilter;
    Filter SineWaveMoveFilter;

    public MoveBehaviorSystem(World world) : base(world)
    {
        ContinuousMoveFilter = FilterBuilder
            .Include<Position>()
            .Include<ContinuousMove>()
            .Build();
        SineWaveMoveFilter = FilterBuilder
            .Include<Position>()
            .Include<Velocity>()
            .Include<SineWaveMovement>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in ContinuousMoveFilter.Entities)
        {
            float moveDir = Get<ContinuousMove>(entity).Value;
            Set(entity, new IntendedMove(moveDir));
        }
        foreach (var entity in SineWaveMoveFilter.Entities)
        {
            var sineData = Get<SineWaveMovement>(entity);
            float nextProgress = (sineData.Progress + (float)delta.TotalSeconds * sineData.Speed) % 1;
            float ySpeed = Mathf.Sin(2 * MathF.PI * nextProgress);
            Vector2 velocity = Get<Velocity>(entity).Value;
            velocity.Y = ySpeed * sineData.Height;
            Set(entity, new Velocity(velocity));
            Set(entity, new SineWaveMovement(sineData.Height, sineData.Speed, nextProgress));
            // GD.Print($"progress: {nextProgress}");
        }
    }
}