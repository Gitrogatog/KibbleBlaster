using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;
// using Godot.Collections;
using MyECS;
using MyECS.Components;

public static class SpriteAnimationManager
{
    static SpriteAnimationInfo[] Animations;
    static int endAnimIndex = 0;
    static int endFrameIndex = 0;
    static Sprite[] Frames;
    static string[] IDToName;
    static Dictionary<string, int> NameToID;
    static int nextAvailableID = 0;
    // public Sprite[] Frames;
    // public AnimationName Name;
    // public int FrameRate;
    // public int FramesCount;
    // public bool Looping = true;
    // public SpriteAnimationInfo(Sprite[] frames, AnimationName name, int frameRate, bool looping)
    // {
    //     FrameRate = frameRate;
    //     Name = name;
    //     Frames = frames;
    //     FramesCount = frames.Length;
    //     Looping = looping;
    //     animations[(int)name] = this;
    // }
    public static void Init(int animCount, int spriteCount)
    {
        Animations = new SpriteAnimationInfo[animCount];
        Frames = new Sprite[spriteCount];
        IDToName = new string[animCount];
        NameToID = new Dictionary<string, int>(animCount);
    }
    public static void RegisterAnimation(List<Sprite> sprites, string name, int frameRate, bool looping, bool facingRight)
    {
        int frameCount = sprites.Count;
        Animations[endAnimIndex] = new SpriteAnimationInfo(new SpriteAnimInfoID(GetIDFromName(name)), frameRate, frameCount, endFrameIndex, looping, facingRight);
        endAnimIndex++;
        foreach (Sprite sprite in sprites)
        {
            Frames[endFrameIndex] = sprite;
            endFrameIndex++;
        }
    }
    public static int GetIDFromName(string name)
    {
        if (!NameToID.ContainsKey(name))
        {
            return RegisterName(name);
        }
        return NameToID[name];
    }
    public static string GetNameFromID(int id)
    {
        return IDToName[id];
    }
    static int RegisterName(string name)
    {
        IDToName[nextAvailableID] = name;
        NameToID[name] = nextAvailableID;
        nextAvailableID++;
        return nextAvailableID - 1;
    }
    // public SpriteAnimation CreateAnimation()
    // {
    //     return new SpriteAnimation(Name, FrameRate, FramesCount, Looping, 0);
    // }
    // public static SpriteAnimationInfo FromID(AnimationName animID)
    // {
    //     return Animations[(int)animID];
    // }
    public static SpriteAnimationInfo FromID(int ID)
    {
        return Animations[ID];
    }
    public static Sprite GetFrame(int index)
    {
        return Frames[index];
    }
    public static Sprite GetFrame(SpriteAnimation animation)
    {
        return Frames[animation.FrameIndex];
    }
}

public readonly record struct SpriteAnimationInfo(SpriteAnimInfoID NameID, int FrameRate, int FrameCount, int StartIndex, bool Looping, bool FacingRight = false);