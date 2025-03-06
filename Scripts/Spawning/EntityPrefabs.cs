using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;
using MyECS.Relations;

public static class EntityPrefabs
{
    public static Entity CreatePlayer(World World, Vector2 position)
    {
        var (entity, charBody) = CreateGroundedEntity(World, position, new Vector2(0, 3.5f), new Vector2(6, 13));
        World.Remove<Gravity>(entity);
        World.Set(entity, new DestroyOnChangeLevel());
        // World.Set(entity, new ColoredRect(new Vector2(20, 20), Colors.YellowGreen));
        // World.Set(entity, new Position(position));
        // World.Set(entity, new MoveSpeed(20f));
        World.Set(entity, new ControlledByPlayer());

        World.Set(entity, new RiseFallGravity(8f, 30f, 10f)); // 30f, 100f, 30f
        World.Set(entity, new AccelParams(60, 10, 8, 0.4f)); //50, 20, 10, 0.4f
        World.Set(entity, new CanJump(170)); // 500
        World.Set(entity, new MaxFallSpeed(200));
        World.Set(entity, new CanShoot(BulletType.Flame, 0.1f, false));
        World.Set(entity, new Facing(true));
        World.Set(entity, new CameraTarget());
        World.Set(entity, new VerticalShootOrigin(new Vector2I(8, 0), new Vector2I(3, -8)));
        World.Set(entity, new LockFacingWhileShooting());
        World.Set(entity, new Health(10));
        World.Set(entity, new BecomeInvincibleOnDamage(2f));

        World.Set(entity, new AimAnimGroup(
            "PlayerIdleUp", "PlayerIdleDown", "PlayerIdle",
            "PlayerWalkUp", "PlayerWalkDown", "PlayerWalk",
            "PlayerRiseUp", "PlayerFallUp", "PlayerRiseDown", "PlayerFallDown", "PlayerRise", "PlayerFall"
        ));

        AddCollider(World, entity, 0, 3, 7, 13, CollisionLayer.Player);
        // CharBody charBody = AddCharBody(World, entity, position, new Vector2(0, 3.5f), new Vector2(6, 13));
        // World.Set(entity, new ScanForGround(new Vector2(0, 10), new Vector2(20, 1)));
        GD.Print($"size of charbody: {charBody.Box.Size}");
        // World.Set(entity, new ScanForGround(new Vector2(0, charBody.Box.End.Y), new Vector2(charBody.Box.Size.X, 1)));
        World.Set(entity, new ScanForCeiling(SpawnUtils.CreateUpperRect(charBody.Box, 1)));
        // World.Set(entity, new ScanForCeiling(new Rect2(new Vector2())))
        // MessagePrefabs.CreateDisableJump(World, entity, 10f);
        var gunEntity = CreateFollower(World, entity, new Vector2I(3, 3));
        World.Set(gunEntity, new FollowParentAimAnimGroup("GunUp", "GunDown", "GunNeutral"));

        UIPrefabs.HealthBar(World, entity, new Vector2(20, 20), new Vector2(60, 10), Colors.Red);
        return entity;
    }
    // public static Entity CreateFacingBullet(World World, bool right)
    // {
    //     return CreateBullet(World, Vector2.Zero, 200, right ? Vector2.Right : Vector2.Left);
    // }
    public static Entity CreateFollower(World World, Entity parent, Vector2I offset)
    {
        Entity entity = World.CreateEntity();
        if (offset.X != 0 && offset.Y != 0)
        {
            World.Set(entity, new SpriteOffset(offset));
        }
        World.Relate(parent, entity, new FollowPosition());
        World.Set(entity, new NeedInRelation<FollowPosition>());
        return entity;
    }

    public static Entity AddCollider(World World, Entity entity, float xCenter, float yCenter, float width, float height, CollisionLayer layer, CollisionLayer mask = 0)
    {
        Entity collider = World.CreateEntity();
        World.Set(entity, new DestroyOnChangeLevel());
        World.Set(collider, new AABB(xCenter, yCenter, width, height));
        World.Set(collider, new Layer(layer));
        World.Set(collider, new Mask(mask));
        World.Relate(entity, collider, new HasCollider());
        return collider;
    }
    public static CharBody AddCharBody(World World, Entity entity, Vector2 position, Vector2 shapeOffset, Vector2 shapeSize)
    {
        CharacterBody2D charBody = GlobalAssets.charBody.Instantiate<CharacterBody2D>();
        GlobalNodes.Root.AddChild(charBody);
        charBody.GlobalPosition = position;
        CharBody bodyComponent = CharBodyStorage.CreateComponent(charBody, entity, shapeOffset, shapeSize);
        World.Set(entity, bodyComponent);
        GD.Print($"char body is at position {charBody.Position}");
        return bodyComponent;
    }
    public static Entity CreateJumpingEnemy(World World, Vector2 position)
    {
        var entity = World.CreateEntity();
        World.Set(entity, new Position(position));
        var charBody = AddCharBody(World, entity, position, new Vector2(0, 0), new Vector2(10, 10));
        World.Set(entity, new ScanForGround(new Vector2(0, charBody.Box.End.Y), new Vector2(charBody.Box.Size.X, 1)));
        World.Set(entity, new Gravity(10f));
        World.Set(entity, new CanJump(250f));
        World.Set(entity, new MaxFallSpeed(200));
        World.Set(entity, JumperState.Inactive);
        World.Set(entity, new GroundAirMoveSpeed(70f, 100f));
        World.Set(entity, new Health(80));
        World.Set(entity, new IdleAnimGroup("EnemyIdle"));
        World.Set(entity, new WalkAnimGroup("EnemyWalk"));
        World.Set(entity, new JumpAnimGroup("EnemyAir", "EnemyAir"));
        // World.Set(entity, new DisableJumpAfterLanding(1f));
        // World.Set(entity, new ConstantAttemptJump());
        AddCollider(World, entity, 0, 0, 10, 10, CollisionLayer.Enemy);
        return entity;
    }
    public static Entity CreateWalkingEnemy(World World, Vector2 position)
    {
        var entity = CreateEnemy(World, position);
        var charBody = AddCharBody(World, entity, position, Vector2.Zero, new Vector2(10, 10));
        World.Set(entity, new Gravity(10f));
        World.Set(entity, new MaxFallSpeed(250));
        World.Set(entity, new MoveSpeed(70f));
        // World.Set(entity, new ColoredRect(new Vector2(10, 10), Colors.Azure));
        World.Set(entity, new Velocity(Vector2.Left * 70f));
        World.Set(entity, new UITarget());

        // World.Set(entity, new JumpAnimGroup("EnemyRise", "EnemyFall"));
        World.Set(entity, new WalkAnimGroup("EnemyWalk"));
        World.Set(entity, new IdleAnimGroup("EnemyIdle"));

        World.Set(entity, new ScanForGround(new Vector2(0, charBody.Box.End.Y), new Vector2(charBody.Box.Size.X, 1)));
        World.Set(entity, new ScanForWall(SpawnUtils.CreateLeftRect(charBody.Box, 1, 0.5f), SpawnUtils.CreateRightRect(charBody.Box, 1, 0.5f)));
        return entity;
    }
    static Entity CreateEnemy(World World, Vector2 position)
    {
        var entity = World.CreateEntity();
        World.Set(entity, new DestroyOnChangeLevel());
        World.Set(entity, new NewlySpawned());
        World.Set(entity, new Position(position));
        World.Set(entity, new Health(3));
        World.Set(entity, new BecomeInvincibleOnDamage(0.1f));
        AddCollider(World, entity, 0, 0, 10, 10, CollisionLayer.Enemy, CollisionLayer.Player);
        return entity;
    }
    // public static Entity CreateAmbush(World World, ThingType type, Vector2 spawnPosition, Vector2 triggerPosition, bool useYPos)
    // {
    //     var ambush = World.CreateEntity();
    //     World.Set(ambush, new AmbushOnPosition(useYPos ? triggerPosition.Y : triggerPosition.X, useYPos));
    //     World.Set(ambush, new SpawnEnemyOnAmbush(type, spawnPosition));
    //     // Set(entity, new AmbushOnHorizontal())
    //     return ambush;
    // }
    public static Entity CreateThing(World World, ThingType type, Vector2 position, SpawnFacing spawnFacing = SpawnFacing.Left)
    {
        // Entity entity;
        Entity entity = type switch
        {
            ThingType.Bird => CreateFlyingEnemy(World, position),
            ThingType.Radish => CreateRadishEnemy(World, position),
            ThingType.Bush => CreateBushEnemy(World, position),
            ThingType.Ghost => CreateGhostEnemy(World, position),

            _ => default
        };
        switch (spawnFacing)
        {
            case SpawnFacing.Left:
                World.Set(entity, new Facing(false));
                break;
            case SpawnFacing.Right:
                World.Set(entity, new Facing(true));
                break;
            case SpawnFacing.FacePlayer:
                World.Set(entity, new SetFacingToPlayer());
                break;

        }
        World.Set(entity, new NewlySpawned());
        return entity;
    }
    public static Entity CreateRadishEnemy(World World, Vector2 position)
    {
        var entity = CreateEnemy(World, position);
        var charBody = AddCharBody(World, entity, position, new Vector2(0, 1), new Vector2(4, 12));
        World.Set(entity, RadishState.InitialJump);
        World.Set(entity, new CanJump(250f));
        World.Set(entity, new Gravity(10f));
        World.Set(entity, new MaxFallSpeed(250f));
        World.Set(entity, new MoveSpeed(70f));
        World.Set(entity, new IdleAnimGroup("RadishIdle"));
        World.Set(entity, new JumpAnimGroup("RadishJump", "RadishJump"));
        World.Set(entity, new AmbushAnimGroup("RadishHide"));
        World.Set(entity, new ScanForGround(new Vector2(0, charBody.Box.End.Y), new Vector2(charBody.Box.Size.X, 1)));
        return entity;
    }
    public static Entity CreateNoInitJumpRadish(World World, Vector2 position)
    {
        var entity = CreateRadishEnemy(World, position);
        World.Set(entity, RadishState.Jump);
        return entity;
    }
    public static Entity CreateFlyingEnemy(World World, Vector2 position)
    {
        // bool facing = position.X > GlobalData.playerPosition.X;
        var entity = CreateEnemy(World, position);
        World.Set(entity, new Velocity(new Vector2(60, 0)));
        World.Set(entity, new Facing());
        World.Set(entity, new SineWaveMovement(60, 1f));
        World.Set(entity, new SetVelocityToFacePlayer());
        World.Set(entity, new IdleAnimGroup("BirdFly"));
        return entity;
    }
    public static Entity CreateBushEnemy(World World, Vector2 position)
    {
        var (entity, charBody) = CreateGroundedEntity(World, position, Vector2.Zero, new Vector2(10, 10));
        World.Set(entity, BushState.InitialJump);
        World.Set(entity, new AccelParams(70, 2, 2, 0.5f));
        // World.Set()
        World.Set(entity, new Facing());
        World.Set(entity, new IdleAnimGroup("BushRun"));
        World.Set(entity, new AmbushAnimGroup("BushHide"));
        World.Set(entity, new JumpAnimGroup("BushJump", "BushFall"));
        return entity;
    }
    public static Entity CreateGhostEnemy(World World, Vector2 position)
    {
        var (entity, charBody) = CreateGroundedEntity(World, position, new Vector2(0, 4), new Vector2(10, 20));
        World.Set(entity, new Health(100));
        World.Set(entity, new ShootOnTakeDamage());
        World.Set(entity, new CanShoot(BulletType.Wave, 0.5f));
        World.Set(entity, new Facing(true));
        World.Set(entity, new IdleAnimGroup("GhostIdle"));
        World.Set(entity, new ShootAnimGroup("GhostShoot", 1f, 20));
        World.Set(entity, new WalkAnimGroup("GhostWalk"));
        World.Set(entity, new MoveSpeed(20f));
        World.Set(entity, new CanJump(100f));
        World.Set(entity, new DealDamageOnContact(1));
        // World.Set(entity, new FlipVertical());
        // World.Set(entity, );
        // World.Set(entity, new ContinuousMove(Vector2.Right));
        World.Set(entity, GhostState.Idle);
        AddScanForWall(World, entity, charBody.Box, 0.5f);
        AddPredictScanForGround(World, entity, charBody.Box);
        // World.Set(entity, new )
        return entity;
    }
    public static (Entity, CharBody) CreateGroundedEntity(World World, Vector2 position, Vector2 shapeOffset, Vector2 shapeSize)
    {
        var entity = CreateEnemy(World, position);
        var charBody = AddCharBody(World, entity, position, shapeOffset, shapeSize);
        World.Set(entity, new Gravity(10f));
        World.Set(entity, new MaxFallSpeed(250f));
        World.Set(entity, new ScanForGround(new Vector2(charBody.Box.Position.X, charBody.Box.End.Y), new Vector2(charBody.Box.Size.X, 0.5f)));
        return (entity, charBody);
    }
    public static Entity CreateDoor(World World, Vector2 position, int level)
    {
        var entity = World.CreateEntity();
        World.Set(entity, new Position(position));
        World.Set(entity, new SetAnimation("Door", 0));
        World.Set(entity, new ChangeLevelOnCollide(level));
        AddCollider(World, entity, 0, 0, 10, 10, CollisionLayer.None, CollisionLayer.Player);
        return entity;
    }
    static void AddScanForWall(World World, Entity entity, Rect2 box, float percentHeight = 0.5f, float thickness = 1)
    {
        World.Set(entity, new ScanForWall(SpawnUtils.CreateLeftRect(box, thickness, percentHeight), SpawnUtils.CreateRightRect(box, thickness, percentHeight)));
    }
    static void AddPredictScanForGround(World World, Entity entity, Rect2 box, float height = 6)
    {
        float width = box.Size.X - 1;
        Vector2 leftPos = new Vector2(box.Position.X - width / 2, box.Position.Y + box.Size.Y);
        Vector2 rightPos = box.End - new Vector2(width / 2, 0);
        World.Set(entity, new PredictScanForGround(new Rect2(leftPos, width, height), new Rect2(rightPos, width, height)));
    }
    // public static void AddBulletDelay()

    public static Entity CreateTimer(World World, float time)
    {
        var entity = World.CreateEntity();
        World.Set(entity, new MyECS.Components.Timer(time));
        World.Set(entity, new DestroyOnChangeLevel());
        return entity;
    }
}