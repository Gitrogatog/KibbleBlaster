namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Messages;

public class PlaySoundEffectSystem : System
{
    public Filter EntityFilter;

    public PlaySoundEffectSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var message in ReadMessages<PlaySoundMessage>())
        {
            var name = message.Name;
            var soundInfo = SoundInfo.FromName(name);
            soundInfo.AudioNode.Play();
        }
    }
}