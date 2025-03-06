namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;


public class LoadPlayerInfoSystem : System
{
    Filter PlayerFilter;
    public LoadPlayerInfoSystem(World world) : base(world)
    {
        PlayerFilter = FilterBuilder.Include<ControlledByPlayer>().Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in PlayerFilter.Entities)
        {
            int maxHealth = Get<Health>(entity).Max;
            Set(entity, new Health(SaveData.PlayerHealth, maxHealth));
        }
    }
}