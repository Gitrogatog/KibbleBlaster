
using System;
using System.Collections.Generic;
using Godot;
using MyECS.Components;
namespace Assets.Godot;
[GlobalClass]
public partial class SoundArrayResource : Resource
{
    [Export] SoundResource[] sounds;
    public void Load(Node audioParent)
    {
        // SpriteAnimationInfo[] infos = new SpriteAnimationInfo[(int)AnimationName.MAX];
        foreach (var sound in sounds)
        {
            AudioStreamPlayer2D audioNode = new AudioStreamPlayer2D
            {
                Stream = sound.Audio,
            };
            SoundInfo.RegisterSound(sound.Name, audioNode);
            audioParent.AddChild(audioNode);
            // var textures = animResource.Frames;
            // Sprite[] sprites = new Sprite[textures.Length];
            // for (int i = 0; i < textures.Length; i++)
            // {
            //     var texture = textures[i];
            //     sprites[i] = new Sprite(texture.Region);
            // }
            // AnimationName name = animResource.Name;
            // new SpriteAnimationInfo(sprites, name, animResource.FrameRate, animResource.Looping);
            // infos[(int)name] = new SpriteAnimationInfo(sprites, name, animResource.FrameRate, animResource.Looping);
        }
    }
}