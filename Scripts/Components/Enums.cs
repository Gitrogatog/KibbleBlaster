using System;
public enum CharBodyGroup
{
    Player, Goomba, Platform
}
public enum Vertical
{
    Up, Down, Neutral
}
public enum SpawnFacing
{
    Left, Right, FacePlayer
}
public enum BulletType
{
    Slow, Fast, Bounce, Flame, Wave
}
public enum ThingType
{
    Radish, Bird, Bomb, Bush, Ghost,
}
public enum AnimationName
{
    PlayerIdle, PlayerWalk, PlayerJump, PlayerFall, PlayerHurt,
    PlayerIdleUp, PlayerIdleDown, PlayerIdleNeutral,
    PlayerWalkUp, PlayerWalkDown, PlayerWalkNeutral,
    PlayerRiseUp, PlayerFallUp, PlayerRiseDown, PlayerFallDown, PlayerRiseNeutral, PlayerFallNeutral,
    GoombaIdle, GoombaWalk, GoomaJump, GoombaFall, GoomaHurt,
    BulletIdle,
    CoinIdle, CoinCollect,

    MAX
}
public enum SoundName
{
    PlayerJump, PlayerShoot, PlayerHurt,
    EnemyJump, EnemyShoot, EnemyHurt,

    MAX
}

public enum CollisionTileType
{
    Empty,
    Full,
    Ledge,
    DownRightSlope,
    DownLeftSlope,
    UpRightSlope,
    UpLeftSlope,
    DownRightTallFatSlope,
    DownRightTallThinSlope,
    DownLeftTallFatSlope,
    DownLeftTallThinSlope,
    UpRightTallFatSlope,
    UpRightTallThinSlope,
    UpLeftTallFatSlope,
    UpLeftTallThinSlope,
    DownRightWideFatSlope,
    DownRightWideThinSlope,
    DownLeftWideFatSlope,
    DownLeftWideThinSlope,
    UpRightWideFatSlope,
    UpRightWideThinSlope,
    UpLeftWideFatSlope,
    UpLeftWideThinSlope,
}

[Flags]
public enum CollisionLayer
{
    None = 0,
    Level = 1,
    Actor = 2,
    Player = 4,
    Enemy = 8,
    Bullet = 16,
    Pickup = 32,
    PlayerActor = Player | Actor | Level | Pickup,
    EnemyActor = Enemy | Actor,
    PlayerBullet = Enemy | Bullet,
    EnemyBullet = Player | Bullet
}