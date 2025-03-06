namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class CreatePlayerSystem : System
{

    public CreatePlayerSystem(World world) : base(world)
    {

    }
    public override void Update(TimeSpan delta)
    {
        // EntityPrefabs.CreatePlayer(World);
    }
}