namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class PredictScanForGroundSystem : ScanBaseSystem
{
    public Filter EntityFilter;
    // PhysicsShapeQueryParameters2D query;
    // RectangleShape2D shape;

    public PredictScanForGroundSystem(World world, uint wallMask) : base(world, wallMask)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<PredictScanForGround>()
            .Build();
        // query = new PhysicsShapeQueryParameters2D();
        // query.CollisionMask = wallMask;
        // shape = new RectangleShape2D();
        // query.Shape = shape;
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            Vector2 position = Get<Position>(entity).RawValue;
            bool facingRight = true;
            if (Has<Velocity>(entity))
            {
                facingRight = Get<Velocity>(entity).Value.X >= 0;
            }
            var groundScan = Get<PredictScanForGround>(entity);
            var scanDetails = facingRight ? groundScan.Right : groundScan.Left;

            if (!Scan(position, scanDetails))
            {
                EntityMods.Flip(World, entity);
            }
        }
    }
}