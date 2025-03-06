using Godot;
public static class GlobalInput
{
    public static FrameInput PrevInput;
    public static FrameInput CurrentInput;
    public static void UpdateInput(Vector2 direction, bool jump, bool shoot)
    {
        ButtonInput jumpButton = new ButtonInput(CurrentInput.Jump.Hold, jump);
        ButtonInput shootButton = new ButtonInput(CurrentInput.Shoot.Hold, shoot);
        PrevInput = CurrentInput;
        CurrentInput = new FrameInput(direction, jumpButton, shootButton);
    }
}

public record struct FrameInput(Vector2 Direction, ButtonInput Jump, ButtonInput Shoot);
public record struct ButtonInput(bool Hold, bool Press, bool Release)
{
    public ButtonInput(bool prev, bool current) : this(current, !prev && current, prev && !current)
    {

    }
}