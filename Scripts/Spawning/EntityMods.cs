
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;
using MyECS.Relations;

public static class EntityMods
{
    public static bool SetAnimation(World World, Entity entity, SpriteAnimInfoID animationName, int priority, bool keepFrame = false)
    {
        SetAnimation targetAnimation = new SetAnimation(animationName, priority, keepFrame);
        if (World.Has<SetAnimation>(entity))
        {
            SetAnimation currentAnimation = World.Get<SetAnimation>(entity);
            if (currentAnimation.Priority > targetAnimation.Priority)
            {
                return false;
            }
        }

        World.Set<SetAnimation>(entity, targetAnimation);
        return true;
    }
    public static bool SetTimedAnimation(World World, Entity entity, SpriteAnimInfoID animationName, int priority, float seconds)
    {
        if (World.Has<TimedAnimGroup>(entity))
        {
            var currentTimedGroup = World.Get<TimedAnimGroup>(entity);
            if (currentTimedGroup.Priority > priority)
            {
                return false;
            }
        }
        World.Set(entity, new TimedAnimGroup(animationName, priority, seconds));
        return true;
    }
    public static bool SetPriority<T>(World World, Entity entity, T component) where T : unmanaged, PriorityComponent<T>
    {
        if (World.Has<T>(entity))
        {
            T curr = World.Get<T>(entity);
            if (curr.Priority > component.Priority)
            {
                return false;
            }
        }
        World.Set(entity, component);
        return true;
    }
    public static void Flip(World World, Entity entity)
    {
        if (World.Has<Velocity>(entity))
        {
            Vector2 velocity = World.Get<Velocity>(entity).Value;
            World.Set(entity, new Velocity(new Vector2(-velocity.X, velocity.Y)));
        }
        if (World.Has<Facing>(entity))
        {
            bool facing = World.Get<Facing>(entity).Right;
            World.Set(entity, new Facing(!facing));
        }
        if (World.Has<ContinuousMove>(entity))
        {
            float moveDir = World.Get<ContinuousMove>(entity).Value;
            World.Set(entity, new ContinuousMove(-moveDir));
        }
    }
}