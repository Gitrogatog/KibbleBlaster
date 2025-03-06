namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public sealed class JumpEnemyAISystem : StateMachineSystem<JumperState>
{
    public Filter EntityFilter;
    public Filter PlayerFilter;
    const float walkSpeed = 100f;
    const float airSpeed = 200f;
    const float distanceToBecomeActive = 100f;
    Vector2I targetPosition;
    public JumpEnemyAISystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<JumperState>()
            .Include<Position>()
            .Exclude<AmbushOnPosition>()
            .Build();
        PlayerFilter = FilterBuilder
            .Include<Position>()
            .Include<ControlledByPlayer>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        targetPosition = PlayerFilter.Empty ? Vector2I.Zero : Get<Position>(PlayerFilter.NthEntity(0)).Value;
        foreach (var entity in EntityFilter.Entities)
        {
            JumperState state = Get<JumperState>(entity);
            // GD.Print($"state: {state}");
            UpdateState(entity, state);
        }
    }

    protected override void EnterState(Entity entity, JumperState state)
    {
        switch (state)
        {
            case JumperState.Inactive:
                Set(entity, new Velocity());
                break;
            case JumperState.Walk:
                {
                    // Set(entity, new FacingHorizontalSpeed(walkSpeed));
                    Set(entity, new ContinuousMove(Mathf.Sign(targetPosition.X - Get<Position>(entity).Value.X)));
                    // Set(entity, new FacingHorizontalGroundAirSpeed(walkSpeed, airSpeed));
                    Set(entity, new RetainState(0.5f));
                    break;
                }
            case JumperState.JumpSquat:
                Set(entity, new Velocity());
                Set(entity, new RetainState(0.5f));
                break;
            case JumperState.Jump:
                {
                    Set(entity, new AttemptJumpThisFrame());
                    Set(entity, new ContinuousMove(Mathf.Sign(targetPosition.X - Get<Position>(entity).Value.X)));
                    // int facingSign = Mathf.Sign(targetPosition.X - Get<Position>(entity).Value.X);
                    // Set(entity, new Facing(facingSign == 1));
                    // Set(entity, new Velocity(new Vector2(airSpeed * facingSign, 0)));
                    // Set(entity, new FacingHorizontalGroundAirSpeed(0, airSpeed));
                    break;
                }
            case JumperState.Land:
                {
                    Set(entity, new Velocity());
                    Set(entity, new RetainState(0.5f));
                    break;
                }
        }
        Set(entity, state);
    }

    protected override void ExitState(Entity entity, JumperState state)
    {
        switch (state)
        {
            case JumperState.Inactive:
                break;
            case JumperState.Walk:
                Remove<ContinuousMove>(entity);
                break;
            case JumperState.Jump:
                Remove<ContinuousMove>(entity);
                break;
        }
    }

    protected override void UpdateState(Entity entity, JumperState state)
    {
        switch (state)
        {
            case JumperState.Inactive:
                Set(entity, new Velocity());
                Vector2I position = Get<Position>(entity).Value;
                if (Mathf.Abs(position.X - targetPosition.X) < distanceToBecomeActive)
                {
                    ChangeState(entity, state, JumperState.Walk);
                }
                break;
            case JumperState.Walk:
                if (!Has<RetainState>(entity))
                {
                    ChangeState(entity, state, JumperState.JumpSquat);
                }
                break;
            case JumperState.JumpSquat:
                if (!Has<RetainState>(entity))
                {
                    ChangeState(entity, state, JumperState.Jump);
                }
                break;
            case JumperState.Jump:
                if (Has<Grounded>(entity))
                {
                    ChangeState(entity, state, JumperState.Land);
                }
                break;
            case JumperState.Land:
                if (!Has<RetainState>(entity))
                {
                    ChangeState(entity, state, JumperState.Walk);
                }
                break;
        }
    }
    // void ChangeState(Entity entity, JumperState oldState, JumperState newState)
    // {
    //     ExitState(entity, oldState);
    //     EnterState(entity, newState);
    // }
}

//superiority humor: funny because you feel smarter than the stupid head
// criticism: when you are the one being insulted (feeling liike an idiot) it can still be funny to you
// explains low comedy (watching someone get hit is funny) but doesnt explain high comedy

//relief humor: funny because you are relieved of something
// freud: saw it as a release of "psychic energy" (as laughter)

// 2nd definition of funny: something weird or different than usual

// incongruency humor: brain picks out something weird then figures out why
// funny when somethng unexpectedly makes sense
// ex: comic where jerry hits tom, then jerry goes to jail
// (or doesnt???)
// low comedy: guy slipping on a banana peel and falling over is funny because of the absurdity
// high comedy: something normal is funny when recontextualized in a weird situation
// ex: openning a window to get fresh air... in a submarine

// benign violation humor:
// funny because something is unusual but safe
// needs to be violating AND benign
// ex: tickling: violation but its ok so you laugh to show that its ok
// low comedy: rag doll getting hit is violating but someone isnt actually getting hurt
// high comedy: stand up comedian slams someone verbally but its not you so its ok

// humor for the listener: spotting things that are weird
// humor for joke teller: signal benigness but still presenting something weird

// proposition: funny = smart
// when someone solves a joke its "smart"
