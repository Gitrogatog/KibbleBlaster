namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DrawScanForGroundBox : System
{
    public Filter EntityFilter;
    CanvasItem drawNode;

    public DrawScanForGroundBox(World world, CanvasItem drawNode) : base(world)
    {
        this.drawNode = drawNode;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<ScanForGround>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {

            // ColoredRect coloredRect = Get<ColoredRect>(entity);
            var scanForGround = Get<ScanForGround>(entity).Rect;
            Vector2 position = Get<Position>(entity).Value;
            Rect2 rect = new(position + scanForGround.Position, scanForGround.Size);
            drawNode.DrawRect(rect, Has<Grounded>(entity) ? Colors.Green : Colors.LightBlue);
        }
    }
}