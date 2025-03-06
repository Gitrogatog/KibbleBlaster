namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class FacingSystem : System
{
    public Filter EntityFilter;

    public FacingSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Facing>()
            .Include<IntendedMove>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            if (Has<LockFacingWhileShooting>(entity) && Has<AttemptShootThisFrame>(entity)) continue;
            float intendedMove = Get<IntendedMove>(entity).Value;
            if (intendedMove != 0)
            {
                Set(entity, new Facing(intendedMove > 0));
            }
        }
    }
}