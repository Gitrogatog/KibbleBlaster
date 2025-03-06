namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class DrawCharBodySystem : System
{
    public Filter EntityFilter;
    CanvasItem drawNode;

    public DrawCharBodySystem(World world, CanvasItem drawNode) : base(world)
    {
        this.drawNode = drawNode;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<CharBody>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Vector2 position = Get<Position>(entity).Value;
            CharBody body = Get<CharBody>(entity);


            Rect2 rect = body.Box;
            rect.Position += position;
            drawNode.DrawRect(rect, Colors.DimGray);
        }
    }
}