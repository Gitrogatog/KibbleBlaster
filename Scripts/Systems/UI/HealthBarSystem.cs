namespace MyECS.Systems;
using System;
using System.Runtime.Intrinsics.X86;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public class HealthBarSystem : System
{
    string currentText;
    Filter EntityFilter;
    public HealthBarSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder.Include<UIRect>().Include<FillBar>().Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in EntityFilter.Entities)
        {
            if (!HasOutRelation<DisplayHealth>(entity))
            {
                continue;
            }
            Entity target = OutRelationSingleton<DisplayHealth>(entity);
            var health = Get<Health>(target);
            float progress = (float)health.Current / (float)health.Max;
            var uiRect = Get<UIRect>(entity);
            Vector2 maxSize = Get<FillBar>(entity).MaxSize;
            Vector2 finalSize = new Vector2(maxSize.X * progress, maxSize.Y);
            Set(entity, new UIRect(uiRect.Position, finalSize, uiRect.Color));
            // Set(entity, new FillBar(Get<FillBar>(entity).MaxSize, progress));
        }
    }
}