using System;
using Assets.Godot;
using CustomTilemap;
using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;
using MyECS.Systems;
public partial class AnimationTestLoop : Node2D
{
    [Export] AnimationArrayResource animationArrayResource;
    public override void _Ready()
    {
        animationArrayResource.Load();
        // drawGroup = new SystemGroup().Add(new DrawSpriteAnimation)
    }
}
