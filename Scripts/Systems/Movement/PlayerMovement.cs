namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class PlayerMovementSystem : System
{
    public Filter EntityFilter;

    public PlayerMovementSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<ControlledByPlayer>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        float horizontalMove = Input.GetAxis("move_left", "move_right");
        float verticalMove = Input.GetAxis("move_up", "move_down");
        if (horizontalMove != 0 || verticalMove != 0)
        {
            foreach (var entity in EntityFilter.Entities)
            {
                Vector2 move = new Vector2(horizontalMove, verticalMove).Normalized();
                Set(entity, new IntendedMove(move));
            }
        }

    }
}