namespace MyECS.Systems;
using System;
using System.Dynamic;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public sealed class RadishStateSystem : StateMachineSystem<RadishState>
{
    public Filter EntityFilter;
    const float walkSpeed = 100f;
    const float airSpeed = 200f;
    const float distanceToBecomeActive = 100f;
    const float landTime = 0.2f;
    public RadishStateSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder.Include<RadishState>().Include<Position>().Exclude<AmbushOnPosition>().Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in EntityFilter.Entities)
        {
            RadishState state = Get<RadishState>(entity);
            if (Has<NewlySpawned>(entity))
            {
                GD.Print("newly spawned radish!");
                EnterState(entity, state);
                Remove<NewlySpawned>(entity);
            }
            else
            {
                UpdateState(entity, state);
            }

        }
    }

    protected override void EnterState(Entity entity, RadishState state)
    {
        switch (state)
        {
            case RadishState.Jump:
                var position = Get<Position>(entity).Value;
                Set(entity, new AttemptJumpThisFrame());
                Set(entity, new ContinuousMove(Mathf.Sign(GlobalData.playerPositionX - position.X)));
                break;
            case RadishState.InitialJump:
                Set(entity, new ConstantAttemptJumpUntilSuccessful());
                break;
            case RadishState.Land:
                Set(entity, new Velocity());
                Set(entity, new RetainState(landTime));
                break;
        }
    }

    protected override void ExitState(Entity entity, RadishState state)
    {
        switch (state)
        {
            case RadishState.Jump:
                Remove<ContinuousMove>(entity);
                break;
            case RadishState.InitialJump:
                // Remove<ConstantAttemptJumpUntilSuccessful>(entity);
                break;
        }
    }

    protected override void UpdateState(Entity entity, RadishState state)
    {
        // GD.Print($"radish in state {state}");
        // GD.Print($"velocity: {Get<Velocity>(entity).Value}");
        // GD.Print($"has grounded: {Has<Grounded>(entity)}");
        switch (state)
        {
            case RadishState.Jump:
                if (Has<Grounded>(entity))
                {
                    ChangeState(entity, state, RadishState.Land);
                }
                break;

            case RadishState.InitialJump:
                if (Has<Grounded>(entity))
                {
                    GD.Print("swapping state!");
                    ChangeState(entity, state, RadishState.Land);
                }
                break;
            case RadishState.Land:
                if (!Has<RetainState>(entity) && Has<Grounded>(entity))
                {
                    ChangeState(entity, state, RadishState.Jump);
                }
                break;
        }
    }
}
