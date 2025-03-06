
namespace MyECS;
using System;
using System.Threading;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public abstract class ScanBaseSystem : System
{
    protected PhysicsShapeQueryParameters2D query;
    protected RectangleShape2D shape;

    public ScanBaseSystem(World world, uint groundMask) : base(world)
    {
        query = new PhysicsShapeQueryParameters2D();
        query.CollisionMask = groundMask;
        shape = new RectangleShape2D();
        query.Shape = shape;
    }

    public bool Scan(Vector2 position, Rect2 scanRect)
    {
        shape.Size = scanRect.Size;
        query.Transform = new Transform2D(0, position + scanRect.Position + scanRect.Size / 2);
        var results = GlobalNodes.StateSpace.IntersectShape(query, 1);
        return results.Count > 0;
    }
}