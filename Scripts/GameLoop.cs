using System;
using Godot;
using MoonTools.ECS;
using MyECS;

public partial class GameLoop : Node2D
{
    public World World { get; } = new World();
    SystemGroup readyGroup;
    SystemGroup processGroup;
    SystemGroup drawGroup;
    public override void _Ready()
    {
        readyGroup = new SystemGroup()
            .Add(new CreatePlayerSystem(World));
        processGroup = new SystemGroup()
            .Add(new PlayerMovementSystem(World))
            .Add(new MovementSystem(World))
            ;
        drawGroup = new SystemGroup()
            .Add(new DrawColoredRectSystem(World, this));

        readyGroup.Run(Utilities.DeltaToTimeSpan(0));

    }
    public override void _Process(double delta)
    {
        processGroup.Run(Utilities.DeltaToTimeSpan(delta));
        QueueRedraw();
    }
    public override void _Draw()
    {
        drawGroup.Run(Utilities.DeltaToTimeSpan(0));

    }
}
