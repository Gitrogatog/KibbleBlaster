namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public sealed class GhostStateSystem : StateMachineSystem<GhostState>
{
    public Filter EntityFilter;
    const float walkSpeed = 100f;
    const float airSpeed = 200f;
    const float distanceToBecomeActive = 100f;
    const float wanderTime = 1f;
    WanderMovement wander = new WanderMovement(1f, 3f, 1.5f, 5f, true);
    public GhostStateSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder.Include<GhostState>().Include<Position>().Exclude<AmbushOnPosition>().Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in EntityFilter.Entities)
        {
            GhostState state = Get<GhostState>(entity);
            if (Has<NewlySpawned>(entity))
            {
                EnterState(entity, state);
                Remove<NewlySpawned>(entity);
            }
            else
            {
                UpdateState(entity, state);
            }

        }
    }

    protected override void EnterState(Entity entity, GhostState state)
    {
        switch (state)
        {
            case GhostState.Idle:
                {
                    Remove<ContinuousMove>(entity);
                    Set(entity, wander);
                    Remove<RetainWander>(entity);
                    // var position = Get<Position>(entity).Value;
                    // Set(entity, new ContinuousMove(Mathf.Sign(GlobalData.playerPositionX - position.X)));
                }
                break;
            case GhostState.Shoot:
                {
                    Remove<WanderMovement>(entity);
                    var position = Get<Position>(entity).Value;
                    Set(entity, new Facing((Mathf.Sign(GlobalData.playerPositionX - position.X) > 0)));
                    Set(entity, new AttemptJumpThisFrame());
                    Set(entity, new RetainState(1f));
                    Set(entity, new AttemptShootThisFrame());
                    Set(entity, new ContinuousMove());
                }
                break;
        }
    }

    protected override void ExitState(Entity entity, GhostState state)
    {
        switch (state)
        {

        }

    }

    protected override void UpdateState(Entity entity, GhostState state)
    {
        // GD.Print($"radish in state {state}");
        // GD.Print($"velocity: {Get<Velocity>(entity).Value}");
        // GD.Print($"has grounded: {Has<Grounded>(entity)}");
        switch (state)
        {
            case GhostState.Idle:
                if (Has<TookDamageThisFrame>(entity))
                {
                    ChangeState(entity, state, GhostState.Shoot);
                }
                break;

            case GhostState.Shoot:
                if (Has<TookDamageThisFrame>(entity))
                {
                    ChangeState(entity, state, GhostState.Shoot);
                }
                else if (!Has<RetainState>(entity))
                {
                    ChangeState(entity, state, GhostState.Idle);
                }
                break;
        }
    }
}
