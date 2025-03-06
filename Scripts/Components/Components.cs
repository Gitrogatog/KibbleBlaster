using System;
using Godot;
namespace MyECS.Components;

public readonly record struct Sprite(Rect2 Rect);
public readonly record struct ColoredRect(Vector2 Size, Color Color, float Depth = 0);

public readonly record struct Rotation(float Value);
public readonly record struct Scale(Vector2 Value);
public readonly record struct Velocity(Vector2 Value)
{
    public Velocity(float X, float Y) : this(new Vector2(X, Y)) { }
}
public readonly record struct ControlledByPlayer();
public readonly record struct IntendedMove(float Value);
public readonly record struct MoveSpeed(float Value);
public readonly record struct GroundAirMoveSpeed(float Ground, float Air);
public readonly record struct Layer(CollisionLayer Value);
public readonly record struct Mask(CollisionLayer Value);
public readonly record struct ScanForGround(Rect2 Rect)
{
    public ScanForGround(Vector2 Position, Vector2 Size) : this(new Rect2(Position, Size)) { }
}
public readonly record struct ScanForCeiling(Rect2 Rect);
public readonly record struct ScanForWall(Rect2 Left, Rect2 Right);
public readonly record struct PredictScanForGround(Rect2 Left, Rect2 Right);
public readonly record struct NoPredictGround();
public readonly record struct DestroyOnTouchGround();
public readonly record struct DealDamageOnContact(int Damage);
public readonly record struct DestroyOnContact();
public readonly record struct MarkForDestroy();
public readonly record struct DestroyOnChangeLevel();
public readonly record struct ChangeLevelOnCollide(int Level);
public readonly record struct Facing(bool Right);
public readonly record struct FlipVertical();
public readonly record struct LockFacingWhileShooting();
public readonly record struct BecomeInvincibleOnDamage(float Time);
public readonly record struct WhiteFlash(float Time) : TimedComponent<WhiteFlash>
{
    public WhiteFlash Update(float t)
    {
        return new WhiteFlash(t);
    }
}
public readonly record struct RetainWander(float Time) : TimedComponent<RetainWander>
{
    public RetainWander Update(float t)
    {
        return new RetainWander(t);
    }
}
public readonly record struct TookDamageThisFrame();
public readonly record struct ContinuousMove(float Value, int Priority = 0) : PriorityComponent<ContinuousMove>;
public readonly record struct IgnoreIntendedMove();
public readonly record struct WanderMovement(float MinWalkTime, float MaxWalkTime, float MinStopTime, float MaxStopTime, bool RetainFacing, bool Walking = false)
{
    public WanderMovement Flip()
    {
        return new WanderMovement(MinWalkTime, MaxWalkTime, MinStopTime, MaxStopTime, RetainFacing, !Walking);
    }
}
public readonly record struct ChangeLevel(int ID);
public readonly record struct AddVelocityThisFrame(Vector2 Value);
public readonly record struct FacingHorizontalSpeed(float Value);
public readonly record struct FacingHorizontalGroundAirSpeed(float Ground, float Air);
public readonly record struct Accel(Vector2 Value);
public readonly record struct Grounded();
public readonly record struct WasGroundedLastFrame();
public readonly record struct BonkedCeilingThisFrame();
public readonly record struct Gravity(float Value);
public readonly record struct RiseFallGravity(float Rise, float JumpEndEarly, float Fall);
public readonly record struct Friction(float Value);
public readonly record struct AccelDecayParams(float maxSpeed, float startGroundDecay, float startAirDecay, float endGroundDecay, float endAirDecay);
public readonly record struct AccelParams(float maxSpeed, float startAccel, float endAccel, float airMult);
public readonly record struct MaxFallSpeed(float Value);
public readonly record struct CanJump(float Velocity);
public readonly record struct IsJumping();
public readonly record struct DidJumpThisFrame();
public readonly record struct AttemptJumpThisFrame();
public readonly record struct HoldJumpThisFrame();
public readonly record struct ConstantAttemptJump();
public readonly record struct ConstantAttemptJumpUntilSuccessful();
public readonly record struct CanShoot(BulletType Bullet, float Delay, bool IsEnemy = true);
public readonly record struct VerticalShootOrigin(Vector2I Neutral, Vector2I Up);
public readonly record struct ShootOrigin(Vector2I Value);
public readonly record struct AttemptShootThisFrame();
public readonly record struct DisableJumpAfterLanding(float Time);
public readonly record struct CameraTarget();
public readonly record struct VerticalAimDirection(Vertical Value);
public readonly record struct UITarget();
public readonly record struct SpriteOffset(Vector2I Value);
public readonly record struct DestroyOnLevelLoad();
public readonly record struct ShootOnTakeDamage();
public readonly record struct NeedInRelation<T>() where T : unmanaged;
public readonly record struct NeedOutRelation<T>() where T : unmanaged;
public readonly record struct RetainState(float Time) : TimedComponent<RetainState>
{
    public RetainState Update(float t)
    {
        return new RetainState(t);
    }
}

public readonly record struct WaitForAmbush();
public readonly record struct SpawnEnemyOnAmbush(ThingType Enemy, Vector2 Position, SpawnFacing Facing);
public readonly record struct AmbushOnPosition(float Position, bool UseYPos);
public readonly record struct SineWaveMovement(float Height, float Speed, float Progress = 0);
public readonly record struct SetVelocityToFacePlayer();
public readonly record struct SetFacingToPlayer();
public readonly record struct DebugPosition();
public readonly record struct NewlySpawned();