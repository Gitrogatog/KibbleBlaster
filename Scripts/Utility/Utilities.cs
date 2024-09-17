
using System;
public static class Utilities
{
    public static TimeSpan DeltaToTimeSpan(double delta)
    {
        long ticks = (long)(delta * 10000000);
        TimeSpan span = new TimeSpan(ticks);
        return span;
    }
}