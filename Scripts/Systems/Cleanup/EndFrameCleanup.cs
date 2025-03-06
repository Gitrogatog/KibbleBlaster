namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class EndFrameCleanup : System
{
    public Filter GroundedFilter;
    public EndFrameCleanup(World world) : base(world)
    {
        GroundedFilter = FilterBuilder
            .Include<Grounded>()
            .Build();

    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in GroundedFilter.Entities)
        {
            Remove<Grounded>(entity);
        }
    }
}