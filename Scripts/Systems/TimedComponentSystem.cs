namespace MyECS.Systems;
using System;
using MoonTools.ECS;
using MyECS.Components;
public class TimedComponentSystem<T> : System where T : unmanaged, TimedComponent<T>
{
    public Filter TimerFilter;

    public TimedComponentSystem(World world) : base(world)
    {
        TimerFilter = FilterBuilder
                        .Include<T>()
                        .Build();
    }

    public override void Update(TimeSpan delta)
    {
        foreach (var entity in TimerFilter.Entities)
        {
            var timer = Get<T>(entity);
            var t = timer.Time - (float)delta.TotalSeconds;

            if (t <= 0.0f)
                Remove<T>(entity);
            else
                Set<T>(entity, timer.Update(t));
        }
    }
}