namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DrawWallScanSystem : System
{
    public Filter EntityFilter;
    CanvasItem drawNode;

    public DrawWallScanSystem(World world, CanvasItem drawNode) : base(world)
    {
        this.drawNode = drawNode;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<ScanForWall>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {

            // ColoredRect coloredRect = Get<ColoredRect>(entity);
            var scan = Get<ScanForWall>(entity);
            Vector2 position = Get<Position>(entity).Value;
            Rect2 rect = new(position + scan.Left.Position, scan.Left.Size);
            drawNode.DrawRect(rect, Colors.DarkRed);
            Rect2 rightRect = new(position + scan.Right.Position, scan.Right.Size);
            drawNode.DrawRect(rightRect, Colors.Red);
            // GD.Print($"left: {rect}, right: {rightRect}");
        }
    }
}