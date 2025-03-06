namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class FacingHorizontalSpeedSystem : System
{
    public Filter NormalFacingSystem;
    public Filter GroundAirSystem;

    public FacingHorizontalSpeedSystem(World world) : base(world)
    {
        NormalFacingSystem = FilterBuilder
            .Include<Facing>()
            .Include<FacingHorizontalSpeed>()
            .Build();
        GroundAirSystem = FilterBuilder
            .Include<Facing>()
            .Include<FacingHorizontalGroundAirSpeed>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in NormalFacingSystem.Entities)
        {
            bool facing = Get<Facing>(entity).Right;
            var speed = Get<FacingHorizontalSpeed>(entity).Value;
            Vector2 velocity = Has<Velocity>(entity) ? Get<Velocity>(entity).Value : Vector2.Zero;
            velocity.X = speed * (facing ? 1 : -1);
            Set(entity, new Velocity(velocity));
        }
        foreach (var entity in GroundAirSystem.Entities)
        {
            GD.Print("running ground air facing speed!");
            bool facing = Get<Facing>(entity).Right;
            bool grounded = Has<Grounded>(entity);
            var gaSpeedData = Get<FacingHorizontalGroundAirSpeed>(entity);
            Vector2 velocity = Has<Velocity>(entity) ? Get<Velocity>(entity).Value : Vector2.Zero;
            velocity.X = (grounded ? gaSpeedData.Ground : gaSpeedData.Air) * (facing ? 1 : -1);
            Set(entity, new Velocity(velocity));
        }
    }
}