namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class ScanForCeilingSystem : ScanBaseSystem
{
    public Filter EntityFilter;
    // PhysicsShapeQueryParameters2D query;
    // RectangleShape2D shape;

    public ScanForCeilingSystem(World world, uint groundMask) : base(world, groundMask)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<ScanForCeiling>()
            .Build();
        // query = new PhysicsShapeQueryParameters2D();
        // query.CollisionMask = groundMask;
        // shape = new RectangleShape2D();
        // query.Shape = shape;
    }
    public override void Update(TimeSpan delta)
    {
        // var spaceState = GlobalNodes.Root.GetWorld2D().DirectSpaceState;
        foreach (var entity in EntityFilter.Entities)
        {
            Vector2 position = Get<Position>(entity).RawValue;
            var scanDetails = Get<ScanForCeiling>(entity).Rect;
            // position += scanDetails.Offset;
            // shape.Size = scanDetails.Size;
            // query.Transform = new Transform2D(0, position + scanDetails.Position);
            // var dictionaries = spaceState.IntersectShape(query, 1);
            // if (dictionaries.Count > 0)
            if (Scan(position, scanDetails))
            {
                // Set(entity, new Grounded());

                // GD.Print("grounded");
                if (Has<Velocity>(entity))
                {
                    Vector2 velocity = Get<Velocity>(entity).Value;
                    if (velocity.Y < 0)
                    {
                        velocity.Y = 0;
                        Set(entity, new Velocity(velocity));
                        Set(entity, new BonkedCeilingThisFrame());
                    }
                }
            }
        }
    }
}