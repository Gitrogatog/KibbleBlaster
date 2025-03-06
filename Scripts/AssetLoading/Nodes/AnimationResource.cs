
using System;
using System.Collections.Generic;
using Godot;
namespace Assets.Godot;
[GlobalClass]
[Tool]
public partial class AnimationResource : Resource
{
    [Export]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            ResourceName = value;
        }
    }
    string _name;
    [Export] public AtlasTexture Frame;
    [Export] public int Columns = 1;
    [Export] public int Rows = 1;
    [Export] public int FrameCount = 0;
    [Export] public int FrameRate;
    [Export] public bool Looping = true;
    [Export] public bool FacingRight;


}