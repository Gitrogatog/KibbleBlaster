namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class TriggerAmbushSystem : System
{
    public Filter AmbushPositionFilter;
    const float positionDifferenceToTriggerAmbush = 1f;

    public TriggerAmbushSystem(World world) : base(world)
    {
        AmbushPositionFilter = FilterBuilder
            .Include<AmbushOnPosition>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in AmbushPositionFilter.Entities)
        {
            var ambush = Get<AmbushOnPosition>(entity);
            float targetPos = ambush.UseYPos ? GlobalData.playerPositionY : GlobalData.playerPositionX;
            if (MathF.Abs(targetPos - ambush.Position) < positionDifferenceToTriggerAmbush)
            {
                Remove<AmbushOnPosition>(entity);
                if (Has<SpawnEnemyOnAmbush>(entity))
                {
                    var spawnData = Get<SpawnEnemyOnAmbush>(entity);
                    GD.Print($"ambush spawn {spawnData.Enemy} at {spawnData.Position}");
                    EntityPrefabs.CreateThing(World, spawnData.Enemy, spawnData.Position, spawnData.Facing);
                    Set(entity, new MarkForDestroy());
                }
            }
        }
    }
}