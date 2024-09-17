namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DrawColoredRectSystem : System
{
    public Filter EntityFilter;
    CanvasItem drawNode;

    public DrawColoredRectSystem(World world, CanvasItem drawNode) : base(world)
    {
        this.drawNode = drawNode;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<ColoredRect>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {

            ColoredRect coloredRect = Get<ColoredRect>(entity);
            Vector2 position = Get<Position>(entity).Value;
            Rect2 rect = new(position, coloredRect.Size);
            drawNode.DrawRect(rect, coloredRect.Color);
        }
    }
}