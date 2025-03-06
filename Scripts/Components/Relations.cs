using MoonTools.ECS;
namespace MyECS.Relations;

public readonly record struct HasCollider();
public readonly record struct Colliding();
public readonly record struct DisableShoot();
public readonly record struct DisableJump();
public readonly record struct Damage(int Value);
public readonly record struct Invincible();
// public readonly record struct RetainState();
public readonly record struct FollowPosition();
public readonly record struct DestroyOnNotPresent();
public readonly record struct DisplayHealth();