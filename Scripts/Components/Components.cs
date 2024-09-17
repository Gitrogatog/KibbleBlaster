using Godot;
namespace MyECS.Components;

public readonly record struct Sprite(Rect2 Rect, float Depth);
public readonly record struct ColoredRect(Vector2 Size, Color Color, float Depth = 0);
public readonly record struct Position(Vector2 Value);
public readonly record struct Rotation(float Value);
public readonly record struct Scale(Vector2 Value);
public readonly record struct ControlledByPlayer();
public readonly record struct IntendedMove(Vector2 Value);
public readonly record struct MoveSpeed(float Value);