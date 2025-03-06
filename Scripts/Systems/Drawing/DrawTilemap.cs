namespace MyECS;
using System;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DrawTilemapSystem : System
{
    CanvasItem drawNode;
    Camera2D camera;
    Texture2D tileTexture;
    public DrawTilemapSystem(World world, CanvasItem drawNode, Camera2D camera, Texture2D tileTexture) : base(world)
    {
        this.drawNode = drawNode;
        this.camera = camera;
        this.tileTexture = tileTexture;
    }
    public override void Update(TimeSpan delta)
    {
        Tilemap tilemap = LevelInfo.tilemap;
        Rect2 viewportRect = drawNode.GetViewport().GetVisibleRect();
        viewportRect.Position += camera.GlobalPosition - viewportRect.Size * 0.5f;
        // GD.Print($"viewport rect: {viewportRect}");
        Rect2I visibleRect = tilemap.GetTilesWithinBox(viewportRect);
        Vector2I tileSize = tilemap.tileSize;
        // visibleRect = new Rect2I(0, 0, tilemap.XTiles, tilemap.YTiles);
        for (int x = visibleRect.Position.X; x < visibleRect.Size.X; x++)
        {
            for (int y = visibleRect.Position.Y; y < visibleRect.Size.Y; y++)
            {
                int id = tilemap.GetIndex(x, y);
                if (id > tilemap.Sprites.Length)
                {
                    GD.Print("grabbing beyond tile!");
                }
                Vector2 atlasPos = tilemap.Sprites[id];
                if (atlasPos.X < 0 || atlasPos.Y < 0)
                {
                    continue;
                }
                Rect2 atlasRect = new Rect2(atlasPos, tileSize);
                Rect2 positionRect = new Rect2(new Vector2(x, y) * tileSize, tileSize);
                drawNode.DrawTextureRectRegion(tileTexture, positionRect, atlasRect);
            }
        }

        // foreach (var entity in EntityFilter.Entities)
        // {

        //     ColoredRect coloredRect = Get<ColoredRect>(entity);
        //     Vector2 position = Get<Position>(entity).Value;
        //     Rect2 rect = new(position - coloredRect.Size / 2, coloredRect.Size);
        //     drawNode.DrawRect(rect, coloredRect.Color);
        // }
    }
}