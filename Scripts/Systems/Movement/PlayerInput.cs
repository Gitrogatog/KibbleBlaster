namespace MyECS;
using System;
using System.Security.Cryptography;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class PlayerInputSystem : System
{
    public Filter EntityFilter;

    public PlayerInputSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<ControlledByPlayer>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        // float horizontalMove = Input.GetAxis("move_left", "move_right");
        // float verticalMove = Input.GetAxis("move_up", "move_down");
        // bool jump = Input.GetActionRawStrength("jump") > .5f;
        // bool shoot = Input.GetActionRawStrength("shoot") > .5f;s
        var currentInput = GlobalInput.CurrentInput;
        Vector2 move = currentInput.Direction;
        bool jumpPress = currentInput.Jump.Press;
        bool jumpHold = currentInput.Jump.Hold;
        bool shoot = currentInput.Shoot.Hold;
        foreach (var entity in EntityFilter.Entities)
        {
            // Vector2 move = new Vector2(horizontalMove, verticalMove).Normalized();
            Set(entity, new IntendedMove(move.X));
            if (jumpPress)
            {
                // GD.Print("inputting jump");
                Set(entity, new AttemptJumpThisFrame());
            }
            if (jumpHold)
            {
                Set(entity, new HoldJumpThisFrame());
            }
            if (shoot)
            {
                Set(entity, new AttemptShootThisFrame());
            }
            // else
            {
                Vertical vertical = move.Y switch
                {
                    > 0 => Vertical.Down,
                    < 0 => Vertical.Up,
                    _ => Vertical.Neutral
                };
                Set(entity, new VerticalAimDirection(vertical));
            }

            // jank
            var pos = Get<Position>(entity).Value;
            GlobalData.playerPositionX = pos.X;
            GlobalData.playerPositionY = pos.Y;
        }

    }
}