namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public abstract class StateMachineSystem<T> : System where T : unmanaged, Enum
{
    protected StateMachineSystem(World world) : base(world)
    {
    }

    protected void ChangeState(Entity entity, T oldState, T newState)
    {
        Set(entity, newState);
        ExitState(entity, oldState);
        EnterState(entity, newState);
    }
    protected abstract void EnterState(Entity entity, T state);
    protected abstract void ExitState(Entity entity, T state);
    protected abstract void UpdateState(Entity entity, T state);
}