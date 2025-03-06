
using System;
using System.Collections.Generic;
using Godot;
namespace Assets.Godot;
[GlobalClass]
public partial class SoundResource : Resource
{
    [Export] public SoundName Name;
    [Export] public AudioStream Audio;

}