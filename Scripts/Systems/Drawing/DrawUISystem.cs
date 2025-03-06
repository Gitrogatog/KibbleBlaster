namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DrawUISystem : System
{
    public Filter EntityFilter;
    CanvasItem drawNode;
    Camera2D offsetNode;

    public DrawUISystem(World world, CanvasItem drawNode, Camera2D offsetNode) : base(world)
    {
        this.offsetNode = offsetNode;
        this.drawNode = drawNode;
        EntityFilter = FilterBuilder
            // .Include<Position>()
            .Include<UIRect>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        Vector2 offsetPos = offsetNode.Position - GlobalData.screenSize / 2;
        foreach (var entity in EntityFilter.Entities)
        {
            var uiRect = Get<UIRect>(entity);
            Rect2 rect = new Rect2(uiRect.Position + offsetPos, uiRect.Scale);
            // ColoredRect coloredRect = Get<ColoredRect>(entity);
            // Vector2 position = Get<Position>(entity).Value;
            // Rect2 rect = new(position - coloredRect.Size / 2, coloredRect.Size);
            drawNode.DrawRect(rect, uiRect.Color);
        }
    }
}