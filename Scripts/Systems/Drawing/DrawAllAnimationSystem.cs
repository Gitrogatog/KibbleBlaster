namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DrawAllAnimationSystem : System
{
    public Filter EntityFilter;
    CanvasItem drawNode;
    Texture2D spriteTexture;

    public DrawAllAnimationSystem(World world, CanvasItem drawNode, Texture2D spriteTexture) : base(world)
    {
        this.drawNode = drawNode;
        this.spriteTexture = spriteTexture;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<SpriteAnimation>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            var spriteAnim = Get<SpriteAnimation>(entity);
            Vector2 position = Get<Position>(entity).Value;
            bool facing = Has<Facing>(entity) && Get<Facing>(entity).Right;
            DrawUtils.DrawSpriteAnim(position, spriteAnim, drawNode, spriteTexture, facing);
        }
    }
}