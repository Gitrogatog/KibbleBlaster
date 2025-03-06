using System;
using Assets.Godot;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;
using MyECS.Systems;

public partial class TilemapLoadingTest : Node2D
{
    public World World { get; } = new World();
    [Export] int tileSize = 16;
    [Export] string pathToLevelJson;
    [Export] Texture2D tileTexture;
    // [Export] Image tileImage;
    DrawTilemapSystem drawTilemapSystem;
    public override void _Ready()
    {
        var tileMapNode = GetNode<TileMap>("TileMap");
        var cameraNode = GetNode<Camera2D>("Camera2D");
        tileMapNode.Visible = false;
        LoadLevelJSON loadLevelJSON = new LoadLevelJSON(World, new Vector2I(tileSize, tileSize), tileMapNode);
        loadLevelJSON.ReadFile(FilePathUtils.GlobalizePath(pathToLevelJson));
        loadLevelJSON.ReadLevel(0);
        drawTilemapSystem = new DrawTilemapSystem(World, this, cameraNode, tileTexture);
    }
    public override void _Process(double delta)
    {
        World.FinishUpdate();
        QueueRedraw();
    }
    public override void _Draw()
    {
        drawTilemapSystem.Update(Utilities.DeltaToTimeSpan(0));
    }
}
