using Godot;
namespace MyECS.Components;

public readonly record struct FillBar(Vector2 MaxSize, float Progress);
public readonly record struct UIRect(Vector2 Position, Vector2 Scale, Color Color);