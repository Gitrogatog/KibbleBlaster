namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class ScanForWallSystem : ScanBaseSystem
{
    public Filter EntityFilter;
    // PhysicsShapeQueryParameters2D query;
    // RectangleShape2D shape;

    public ScanForWallSystem(World world, uint wallMask) : base(world, wallMask)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<ScanForWall>()
            .Build();
        // query = new PhysicsShapeQueryParameters2D();
        // query.CollisionMask = wallMask;
        // shape = new RectangleShape2D();
        // query.Shape = shape;
    }
    public override void Update(TimeSpan delta)
    {
        // var spaceState = GlobalNodes.Root.GetWorld2D().DirectSpaceState;
        foreach (var entity in EntityFilter.Entities)
        {
            // GD.Print("running wall system");
            Vector2 position = Get<Position>(entity).RawValue;
            bool facingRight = true;
            if (Has<Velocity>(entity))
            {
                facingRight = Get<Velocity>(entity).Value.X >= 0;
            }
            // GD.Print($"facing toward right: {facingRight}");
            var wallScan = Get<ScanForWall>(entity);
            var scanDetails = facingRight ? wallScan.Right : wallScan.Left;
            // position += scanDetails.Offset;
            // shape.Size = scanDetails.Size;
            // query.Transform = new Transform2D(0, position + scanDetails.Position);
            // var dictionaries = spaceState.IntersectShape(query, 1);

            // if (dictionaries.Count > 0)
            if (Scan(position, scanDetails))
            {
                // Set(entity, new Grounded());

                // GD.Print("grounded");
                EntityMods.Flip(World, entity);
            }
        }
    }
}