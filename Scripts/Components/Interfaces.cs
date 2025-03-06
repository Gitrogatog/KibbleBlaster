using System.IO;
using Godot;
using MyECS.Components;
public interface TimedComponent<T> where T : unmanaged, TimedComponent<T>
{
    public float Time { get; }
    public T Update(float Time);
}

public interface PriorityComponent<T> where T : unmanaged, PriorityComponent<T>
{
    public int Priority { get; }
}
public interface AccelerationComponent<T> where T : unmanaged, AccelerationComponent<T>
{
    public Vector2 Acceleration { get; }
}