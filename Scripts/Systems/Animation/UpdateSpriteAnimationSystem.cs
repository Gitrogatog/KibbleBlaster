namespace MyECS;
using System;
using System.Collections.Generic;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class UpdateSpriteAnimationSystem : System
{
    public Filter EntityFilter;
    List<SpriteAnimationInfo> animations;

    public UpdateSpriteAnimationSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<SpriteAnimation>()
            .Include<Position>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        SpriteAnimationInfo animData;
        foreach (var entity in EntityFilter.Entities)
        {
            UpdateSpriteAnimation(entity, (float)delta.TotalSeconds);
        }
    }
    int GetFrame(float progress, int framesCount)
    {
        int frame = Mathf.FloorToInt(progress * framesCount);
        frame = Mathf.Clamp(frame, 0, framesCount - 1);
        return frame;
    }
    public void UpdateSpriteAnimation(Entity entity, float dt)
    {
        var spriteAnimation = Get<SpriteAnimation>(entity).Update(dt);
        Set(entity, spriteAnimation);

        if (spriteAnimation.Finished)
        {
            /*
			if (Has<DestroyOnAnimationFinish>(entity))
			{
				Destroy(entity);
			}
			*/
        }
    }
}