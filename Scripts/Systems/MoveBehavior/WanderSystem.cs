namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class WanderSystem : System
{
    public Filter EntityFilter;

    public WanderSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<WanderMovement>()
            .Exclude<RetainWander>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            var data = Get<WanderMovement>(entity);
            if (data.Walking)
            {
                EntityMods.SetPriority(World, entity, new ContinuousMove());
                Set(entity, new RetainWander(RandomUtils.RandomF(data.MinStopTime, data.MaxStopTime)));
            }
            else if (data.RetainFacing)
            {
                bool facing = Get<Facing>(entity).Right;
                EntityMods.SetPriority(World, entity, new ContinuousMove(facing ? 1 : -1));
                Set(entity, new RetainWander(RandomUtils.RandomF(data.MinWalkTime, data.MaxWalkTime)));
            }
            else
            {
                EntityMods.SetPriority(World, entity, new ContinuousMove());
                Set(entity, new RetainWander(RandomUtils.RandomF(data.MinWalkTime, data.MaxWalkTime)));
            }
            Set(entity, data.Flip());
        }
    }
}