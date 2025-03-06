namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DrawPredictGroundScanSystem : System
{
    public Filter EntityFilter;
    CanvasItem drawNode;

    public DrawPredictGroundScanSystem(World world, CanvasItem drawNode) : base(world)
    {
        this.drawNode = drawNode;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<PredictScanForGround>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {

            // ColoredRect coloredRect = Get<ColoredRect>(entity);
            var scanForGround = Get<PredictScanForGround>(entity);
            Vector2 position = Get<Position>(entity).Value;
            Rect2 left = new(position + scanForGround.Left.Position, scanForGround.Left.Size);
            Rect2 right = new(position + scanForGround.Right.Position, scanForGround.Right.Size);
            drawNode.DrawRect(left, Colors.LightBlue);
            drawNode.DrawRect(right, Colors.LightGreen);
        }
    }
}