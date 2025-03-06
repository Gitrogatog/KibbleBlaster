using System;
using Godot;
namespace MyECS.Components;

public readonly record struct SpriteAnimInfoID(int ID)
{
    public static implicit operator SpriteAnimInfoID(string name) =>
        new SpriteAnimInfoID(SpriteAnimationManager.GetIDFromName(name));
    // public static implicit operator int(SpriteAnimInfoID info) => info.ID;
}
public readonly record struct SetAnimation(SpriteAnimInfoID Name, int Priority, bool SameFrame = false);

public readonly record struct JumpAnimGroup(SpriteAnimInfoID Up, SpriteAnimInfoID Down);
public readonly record struct WalkAnimGroup(SpriteAnimInfoID Walk);
public readonly record struct IdleAnimGroup(SpriteAnimInfoID Idle);
public readonly record struct AimAnimGroup(
    SpriteAnimInfoID IdleUp, SpriteAnimInfoID IdleDown, SpriteAnimInfoID IdleNeutral,
    SpriteAnimInfoID WalkUp, SpriteAnimInfoID WalkDown, SpriteAnimInfoID WalkNeutral,
    SpriteAnimInfoID RiseUp, SpriteAnimInfoID FallUp,
    SpriteAnimInfoID RiseDown, SpriteAnimInfoID FallDown,
    SpriteAnimInfoID RiseNeutral, SpriteAnimInfoID FallNeutral
);
public readonly record struct FollowParentAimAnimGroup(SpriteAnimInfoID Up, SpriteAnimInfoID Down, SpriteAnimInfoID Neutral);
public readonly record struct AmbushAnimGroup(SpriteAnimInfoID Idle);
public readonly record struct ShootAnimGroup(SpriteAnimInfoID Animation, float Seconds, int Priority);
public readonly record struct TimedAnimGroup(SpriteAnimInfoID Animation, int Priority, float Time) : TimedComponent<TimedAnimGroup>
{
    public TimedAnimGroup Update(float t)
    {
        return new TimedAnimGroup(Animation, Priority, t);
    }
}