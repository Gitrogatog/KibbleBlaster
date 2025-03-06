namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Messages;

public class ChangeLevelSystem : System
{
    public Filter EntityFilter;
    Action<int> levelChangeAction;
    public ChangeLevelSystem(World world, Action<int> levelChangeAction) : base(world)
    {
        this.levelChangeAction = levelChangeAction;
        EntityFilter = FilterBuilder
            .Include<ChangeLevel>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        if (EntityFilter.Count == 0) return;
        int targetLevel = 0;
        foreach (var entity in EntityFilter.Entities)
        {
            targetLevel = Get<ChangeLevel>(entity).ID;
            Destroy(entity);
        }
        levelChangeAction(targetLevel);
    }
}