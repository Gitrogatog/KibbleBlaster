using System;
using System.Diagnostics;
using Assets.Godot;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;
using MyECS.Relations;
using MyECS.Systems;

public partial class GameLoop : Node2D
{

    public World World { get; } = new World();
    SystemGroup readyGroup;
    SystemGroup processGroup;
    SystemGroup drawGroup;
    SystemGroup preLoadLevelGroup;
    SystemGroup postLoadLevelGroup;
    TileMap tileMapNode;
    TilemapReader tilemapReader;

    LoadLevelJSON loadLevelJSON;
    int currentLevel;
    [Export] int tileSize = 8;
    [Export] string pathToLevelJson;
    [Export] string pathToDialogCSV;

    [Export] AnimationArrayResource animationArrayResource;
    [Export] SoundArrayResource soundArrayResource;
    [Export] PackedScene charBody;
    [Export] Texture2D spriteTexture;
    [Export] Texture2D tileTexture;
    [Export(PropertyHint.Layers2DPhysics)] uint groundMask;
    [Export(PropertyHint.Layers2DPhysics)] uint ceilingMask;
    [Export(PropertyHint.Layers2DPhysics)] uint wallMask;
    // private Godot.AABB

    public override void _Ready()
    {
        GlobalNodes.Root = this;
        GlobalNodes.Viewport = GetViewport();
        GlobalNodes.StateSpace = GetWorld2D().DirectSpaceState;
        GlobalData.tileSize = tileSize;
        GlobalData.screenSize = GetViewport().GetVisibleRect().Size;

        GlobalAssets.charBody = charBody;


        animationArrayResource.Load();
        soundArrayResource.Load(this);

        Camera2D camera = GetNode<Camera2D>("Camera2D");
        Control dialogUI = GetNode<Control>("ScreenSpaceUILayer/DialogUI");
        RichTextLabel dialogText = dialogUI.GetNode<RichTextLabel>("Text");
        RichTextLabel playerVelocityText = GetNode<RichTextLabel>("ScreenSpaceUILayer/Debug/PlayerVelocity");
        DrawCallback pureWhiteNode = GetNode<DrawCallback>("PureWhiteDraw");
        tileMapNode = GetNode<TileMap>("TileMap");
        tileMapNode.Clear();
        tileMapNode.Visible = false;


        loadLevelJSON = new LoadLevelJSON(World, new Vector2I(tileSize, tileSize), tileMapNode);
        loadLevelJSON.ReadFile(FilePathUtils.GlobalizePath(pathToLevelJson));
        // tilemapReader = new TilemapReader(tileMapNode, World);

        readyGroup = new SystemGroup()
            ;
        processGroup = new SystemGroup()
            .Add(new RemoveComponent<WasGroundedLastFrame>(World))
            .Add(new ReplaceComponent<Grounded, WasGroundedLastFrame>(World))
            .Add(new TimerSystem(World))
            .Add(new TimedComponentSystem<WhiteFlash>(World))
            .Add(new TimedComponentSystem<TimedAnimGroup>(World))
            .Add(new TimedComponentSystem<RetainState>(World))
            .Add(new TimedComponentSystem<RetainWander>(World))

            .Add(new ChangeLevelSystem(World, LoadLevel))

            .Add(new PlayerInputSystem(World))

            .Add(new FaceVelocityToPlayerSystem(World))
            .Add(new WanderSystem(World))

            .Add(new ScanForGroundSystem(World, groundMask))
            .Add(new ScanForCeilingSystem(World, ceilingMask))
            .Add(new ScanForWallSystem(World, groundMask))
            .Add(new PredictScanForGroundSystem(World, groundMask))
            .Add(new DeleteEntities<Grounded, DestroyOnTouchGround>(World))
            .Add(new DeleteEntitiesWithoutInRelation<FollowPosition>(World))
            .Add(new Destroyer(World))


            .Add(new TriggerAmbushSystem(World))
            // ai
            .Add(new JumpEnemyAISystem(World))
            .Add(new RadishStateSystem(World))
            .Add(new BushStateSystem(World))
            .Add(new GhostStateSystem(World))

            // .Add(new FacingHorizontalSpeedSystem(World))
            .Add(new AddComponent<TookDamageThisFrame, ShootOnTakeDamage, AttemptShootThisFrame>(World))
            .Add(new AddComponent<ConstantAttemptJump, AttemptJumpThisFrame>(World))
            .Add(new AddComponent<ConstantAttemptJumpUntilSuccessful, AttemptJumpThisFrame>(World))
            .Add(new MoveBehaviorSystem(World))
            .Add(new FacingSystem(World))

            // .Add(new JumpInPlaceSystem(World))
            .Add(new SpawnBulletSystem(World))
            .Add(new DisableJumpAfterLandingSystem(World))

            .Add(new IntendedMoveSystem(World))
            .Add(new MidJumpSystem(World))
            .Add(new BeginJump(World))

            .Add(new GravitySystem(World))

            .Add(new Motion(World))
            .Add(new Collision(World))

            .Add(new FollowPositionSystem(World))

            .Add(new RemoveComponent<TookDamageThisFrame>(World))
            .Add(new HealthSystem(World))

            // ui
            .Add(new PlayerVelocityUISystem(World, playerVelocityText))
            .Add(new HealthBarSystem(World))
            // .Add(new DialogSystem(World, dialogText))

            .Add(new CameraSystem(World, camera))

            // animation 
            .Add(new TimedAnimationSystem(World))
            .Add(new IdleAnimationSystem(World))
            .Add(new WalkAnimationSystem(World))
            .Add(new JumpAnimationSystem(World))
            .Add(new AimAnimationSystem(World))
            .Add(new AmbushAnimationSystem(World))
            // .Add(new MovementSystem(World))
            .Add(new SetSpriteAnimationSystem(World))
            .Add(new UpdateSpriteAnimationSystem(World))

            .Add(new PlaySoundEffectSystem(World))

            .Add(new ConditionalRemoveComponent<DidJumpThisFrame, ConstantAttemptJumpUntilSuccessful>(World))
            .Add(new RemoveComponent<IntendedMove>(World))
            .Add(new RemoveComponent<AttemptJumpThisFrame>(World))
            .Add(new RemoveComponent<HoldJumpThisFrame>(World))
            .Add(new RemoveComponent<DidJumpThisFrame>(World))
            .Add(new RemoveComponent<AttemptShootThisFrame>(World))
            .Add(new RemoveComponent<SetAnimation>(World))
            .Add(new RemoveComponent<NewlySpawned>(World))
            ;
        drawGroup = new SystemGroup()
            // .Add(new DrawAABB(World, this))
            // .Add(new DrawColoredRectSystem(World, this))
            .Add(new DrawScanForGroundBox(World, this))
            // 
            .Add(new DrawSpriteAnimation(World, this, spriteTexture))
            .Add(new DrawTilemapSystem(World, this, camera, tileTexture))

            .Add(new DrawUISystem(World, this, camera))
            // .Add(new DrawCharBodySystem(World, this))
            // 
            // .Add(new DrawWallScanSystem(World, this))
            .Add(new DrawPredictGroundScanSystem(World, this))
            ;

        SystemGroup whiteDrawGroup = new SystemGroup().Add(new DrawFlashAnimSystem(World, pureWhiteNode, spriteTexture));
        pureWhiteNode.OnDraw += () => whiteDrawGroup.Run(new TimeSpan(0));
        preLoadLevelGroup = new SystemGroup()
            .Add(new SavePlayerInfoSystem(World))
            .Add(new DeleteEntities<DestroyOnChangeLevel>(World))
            ;
        postLoadLevelGroup = new SystemGroup()
            .Add(new LoadPlayerInfoSystem(World))
            ;
        // tilemapReader.ReadEntityLayer();
        loadLevelJSON.ReadLevel(0);

        readyGroup.Run(Utilities.DeltaToTimeSpan(0));

        // int animID = SpriteAnimationManager.GetIDFromName("Flame");
        // SpriteAnimationInfo info = SpriteAnimationManager.FromID(animID);


        // EntityPrefabs.CreateEnemy(World, new Vector2(140, 160));

    }
    public override void _Process(double delta)
    {
        long startTime = Stopwatch.GetTimestamp();
        UpdateInput();
        processGroup.Run(Utilities.DeltaToTimeSpan(delta));
        World.FinishUpdate();
        QueueRedraw();
        double elapsed = Stopwatch.GetElapsedTime(startTime).TotalSeconds;
        // GD.Print($"process took {elapsed} seconds");
    }
    public override void _Draw()
    {
        long startTime = Stopwatch.GetTimestamp();
        drawGroup.Run(Utilities.DeltaToTimeSpan(0));
        double elapsed = Stopwatch.GetElapsedTime(startTime).TotalSeconds;
        // GD.Print($"draw took {elapsed} seconds");
    }
    void UpdateInput()
    {
        float horizontalMove = Input.GetAxis("move_left", "move_right");
        float verticalMove = Input.GetAxis("move_up", "move_down");
        bool jump = Input.GetActionRawStrength("jump") > .5f;
        bool shoot = Input.GetActionRawStrength("shoot") > .5f;
        GlobalInput.UpdateInput(new Vector2(horizontalMove, verticalMove), jump, shoot);
    }
    void LoadLevel(int level)
    {
        preLoadLevelGroup.Run(new TimeSpan(0));
        CharBodyStorage.DestroyAllCharBodies();
        currentLevel = level;
        loadLevelJSON.ReadLevel(level);
        postLoadLevelGroup.Run(new TimeSpan(0));
    }
}
