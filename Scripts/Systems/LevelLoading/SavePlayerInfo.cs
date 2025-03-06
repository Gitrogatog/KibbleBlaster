namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;


public class SavePlayerInfoSystem : System
{
    Filter PlayerFilter;
    public SavePlayerInfoSystem(World world) : base(world)
    {
        PlayerFilter = FilterBuilder.Include<ControlledByPlayer>().Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in PlayerFilter.Entities)
        {
            int currentHealth = Get<Health>(entity).Current;
            SaveData.PlayerHealth = currentHealth;
        }
    }
}