namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DrawSpriteAnimation : System
{
    public Filter EntityFilter;
    CanvasItem drawNode;
    Texture2D spriteTexture;

    public DrawSpriteAnimation(World world, CanvasItem drawNode, Texture2D spriteTexture) : base(world)
    {
        this.drawNode = drawNode;
        this.spriteTexture = spriteTexture;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<SpriteAnimation>()
            .Exclude<WhiteFlash>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            var spriteAnim = Get<SpriteAnimation>(entity);
            Vector2 position = Get<Position>(entity).Value;
            bool facing = Has<Facing>(entity) && Get<Facing>(entity).Right;
            if (spriteAnim.FacingRight) facing = !facing;
            DrawUtils.DrawSpriteAnim(position, spriteAnim, drawNode, spriteTexture, facing, Has<FlipVertical>(entity));
            // var displayRect = Utilities.CenterRect(new Rect2(position, sprite.Rect.Size));
            // if ()
            // {
            //     displayRect.Size = new Vector2(-displayRect.Size.X, displayRect.Size.Y);
            // }
            // drawNode.DrawTextureRectRegion(spriteTexture, displayRect, sprite.Rect);
            // Rect2 rect = new(position - coloredRect.Size / 2, coloredRect.Size);
            // drawNode.DrawRect(rect, coloredRect.Color);
        }
    }
}