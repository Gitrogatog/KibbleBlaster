namespace MyECS.Systems;
using System;
using System.Runtime.Intrinsics.X86;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Messages;

public class PlayerVelocityUISystem : System
{
    RichTextLabel textNode;
    string currentText;
    Filter EntityFilter;
    public PlayerVelocityUISystem(World world, RichTextLabel textNode) : base(world)
    {
        this.textNode = textNode;
        EntityFilter = FilterBuilder.Include<Velocity>().Include<UITarget>().Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in EntityFilter.Entities)
        {
            Vector2 velocity = Get<Velocity>(entity).Value;
            textNode.Text = $"X: {velocity.X}\nY: {velocity.Y}";
        }
    }
}