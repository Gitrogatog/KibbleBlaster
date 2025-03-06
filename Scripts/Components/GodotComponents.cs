
using System;
using Godot;
namespace MyECS.Components;

public readonly record struct CharBody(int ID, Rect2 Box);
public readonly record struct StaticBody(int ID);