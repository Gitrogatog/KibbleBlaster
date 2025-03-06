
namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class SetSpriteAnimationSystem : System
{
    public Filter EntityFilter;

    public SetSpriteAnimationSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<SetAnimation>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            var targetAnim = Get<SetAnimation>(entity);
            // GD.Print($"setting animation {targetAnim.Name}");
            if (Has<SpriteAnimation>(entity))
            {
                var currentAnim = Get<SpriteAnimation>(entity);

                if (currentAnim.Name != targetAnim.Name)
                {
                    if (targetAnim.SameFrame)
                    {
                        // GD.Print($"Setting animation: {targetAnim.Name}");
                        Set(entity, new SpriteAnimation(targetAnim.Name, currentAnim.FrameIndex));
                    }
                    else
                    {
                        // GD.Print($"Setting animation: {targetAnim.Name}");
                        Set(entity, new SpriteAnimation(targetAnim.Name));
                    }

                }
            }
            else
            {
                // GD.Print($"Setting animation: {targetAnim.Name}");
                Set(entity, new SpriteAnimation(targetAnim.Name));
            }
        }
    }
}