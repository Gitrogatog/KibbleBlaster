namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public sealed class BushStateSystem : StateMachineSystem<BushState>
{
    public Filter EntityFilter;
    const float walkSpeed = 100f;
    const float airSpeed = 200f;
    const float distanceToBecomeActive = 100f;
    const float landTime = 0.2f;
    public BushStateSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder.Include<BushState>().Include<Position>().Exclude<AmbushOnPosition>().Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in EntityFilter.Entities)
        {
            BushState state = Get<BushState>(entity);
            if (Has<NewlySpawned>(entity))
            {
                GD.Print("newly spawned bush!");
                EnterState(entity, state);
                Remove<NewlySpawned>(entity);
            }
            else
            {
                UpdateState(entity, state);
            }

        }
    }

    protected override void EnterState(Entity entity, BushState state)
    {
        switch (state)
        {
            case BushState.Run:
                var position = Get<Position>(entity).Value;
                Set(entity, new IntendedMove(Mathf.Sign(GlobalData.playerPositionX - position.X)));
                break;
            case BushState.InitialJump:
                Set(entity, new ConstantAttemptJumpUntilSuccessful());
                break;
        }
    }

    protected override void ExitState(Entity entity, BushState state)
    {
        switch (state)
        {

        }
    }

    protected override void UpdateState(Entity entity, BushState state)
    {
        switch (state)
        {
            case BushState.InitialJump:
                if (Has<Grounded>(entity))
                {
                    ChangeState(entity, state, BushState.Run);
                }
                break;

            case BushState.Run:
                var position = Get<Position>(entity).Value;
                Set(entity, new IntendedMove(Mathf.Sign(GlobalData.playerPositionX - position.X)));
                break;
        }
    }
}
