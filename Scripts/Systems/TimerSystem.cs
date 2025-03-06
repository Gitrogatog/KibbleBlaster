namespace MyECS.Systems;
using System;
using MoonTools.ECS;
using MyECS.Components;
public class TimerSystem : MoonTools.ECS.System
{
    public Filter TimerFilter;

    public TimerSystem(World world) : base(world)
    {
        TimerFilter = FilterBuilder
                        .Include<Timer>()
                        .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var entity in TimerFilter.Entities)
        {
            var timer = Get<Timer>(entity);
            var t = timer.Time - (float)delta.TotalSeconds;

            if (t <= 0.0f)
                Set(entity, new MarkForDestroy());
            else
                Set(entity, timer.Update(t));
        }
    }
}