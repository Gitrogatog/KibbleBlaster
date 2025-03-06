using System;
using System.Collections.Generic;
using Godot;
namespace Assets.Godot;
[GlobalClass]
public partial class CharBodyArrayResource : Resource
{
    [Export] CharBodyResource[] bodies;
    public void Load()
    {
        int maxLength = -1;
        for (int i = 0; i < bodies.Length; i++)
        {
            maxLength = Math.Max(maxLength, (int)bodies[i].group);
        }
        GlobalAssets.charBodies = new PackedScene[maxLength + 1];
        for (int i = 0; i < bodies.Length; i++)
        {
            var body = bodies[i];
            GlobalAssets.charBodies[(int)body.group] = body.body;
        }
        // StaticAssets.charBodies[]
    }
}


