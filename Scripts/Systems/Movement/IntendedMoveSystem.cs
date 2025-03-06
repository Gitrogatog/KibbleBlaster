namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class IntendedMoveSystem : System
{
    public Filter EntityFilter;
    public Filter SpecialMovementFilter;
    public Filter GroundAirFilter;
    float defaultMoveSpeed = 100f;
    public IntendedMoveSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<IntendedMove>()
            .Exclude<AccelParams>()
            .Build();
        GroundAirFilter = FilterBuilder
            .Include<Position>()
            .Include<IntendedMove>()
            .Include<GroundAirMoveSpeed>()
            .Build();
        SpecialMovementFilter = FilterBuilder
            .Include<Position>()
            .Include<IntendedMove>()
            .Include<AccelParams>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            float intendedMove = Get<IntendedMove>(entity).Value;
            float moveSpeed = Has<MoveSpeed>(entity) ? Get<MoveSpeed>(entity).Value : defaultMoveSpeed;
            Vector2 velocity = Has<Velocity>(entity) ? Get<Velocity>(entity).Value : Vector2.Zero;
            Set(entity, new Velocity(moveSpeed * intendedMove, velocity.Y));
        }
        foreach (var entity in GroundAirFilter.Entities)
        {
            float intendedMove = Get<IntendedMove>(entity).Value;
            var groundAirParams = Get<GroundAirMoveSpeed>(entity);
            float moveSpeed = Has<Grounded>(entity) ? groundAirParams.Ground : groundAirParams.Air;
            Vector2 velocity = Has<Velocity>(entity) ? Get<Velocity>(entity).Value : Vector2.Zero;
            Set(entity, new Velocity(moveSpeed * intendedMove, velocity.Y));
        }
        foreach (var entity in SpecialMovementFilter.Entities)
        {
            float intendedMove = Get<IntendedMove>(entity).Value;
            // float moveSpeed = Has<MoveSpeed>(entity) ? Get<MoveSpeed>(entity).Value : defaultMoveSpeed;
            Vector2 velocity = Has<Velocity>(entity) ? Get<Velocity>(entity).Value : Vector2.Zero;
            AccelParams pars = Get<AccelParams>(entity);
            float airMult = Has<Grounded>(entity) ? 1 : pars.airMult;
            if (intendedMove != 0)
            {
                velocity.X = Utilities.LerpDecay(velocity.X, intendedMove * pars.maxSpeed, pars.startAccel, (float)delta.TotalSeconds);
                // velocity.X = Mathf.Lerp(velocity.X, intendedMove.X * pars.maxSpeed, pars.startAccel / pars.maxSpeed * airMult);
            }
            else
            {
                velocity.X = Utilities.LerpDecay(velocity.X, 0, pars.endAccel, (float)delta.TotalSeconds);
                // velocity.X = Mathf.Lerp(velocity.X, 0, pars.endAccel / pars.maxSpeed * airMult);
            }
            Set(entity, new Velocity(velocity));
            // if (intendedMove.X != 0)
            // {
            //     velocity.X = Utilities.Lerp(velocity.X, intendedMove.X * pars.maxSpeed, Has<Grounded>(entity) ? pars.startGroundDecay : pars.startAirDecay, (float)delta.TotalSeconds);
            // }
            // else
            // {
            //     velocity.X = Utilities.Lerp(velocity.X, 0, Has<Grounded>(entity) ? pars.endGroundDecay : pars.endAirDecay, (float)delta.TotalSeconds);
            // }
        }
    }
}