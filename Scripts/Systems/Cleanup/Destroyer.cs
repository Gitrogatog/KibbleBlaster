namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public class Destroyer : System
{
    public Filter GroundedFilter;
    public Destroyer(World world) : base(world)
    {
        GroundedFilter = FilterBuilder
            .Include<MarkForDestroy>()
            .Build();

    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in GroundedFilter.Entities)
        {
            foreach (var collider in OutRelations<HasCollider>(entity))
                Destroy(collider);
            Destroy(entity);
        }
    }
}