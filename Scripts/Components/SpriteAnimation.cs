using System;
using System.Reflection.Metadata.Ecma335;
using Godot;
namespace MyECS.Components;

public struct SpriteAnimation
{
    public SpriteAnimInfoID Name { get; }
    public int FrameRate { get; }
    public int TotalFrames { get; }
    public bool Loop { get; }
    public float RawFrameIndex { get; }
    public bool FacingRight;

    public int FrameIndex
    {
        get
        {
            var integerIndex = (int)(MathF.Sign(RawFrameIndex) * MathF.Floor(MathF.Abs((RawFrameIndex))));
            // var framesLength = SpriteAnimationInfo.Frames.Length;
            if (Loop)
            {
                return ((integerIndex % TotalFrames) + TotalFrames) % TotalFrames;
            }
            else
            {
                return Mathf.Clamp(integerIndex, 0, TotalFrames - 1);
            }
        }
    }
    public SpriteAnimationInfo SpriteAnimationInfo => SpriteAnimationManager.FromID(Name.ID);
    public Sprite CurrentSprite => SpriteAnimationManager.GetFrame(FrameIndex + SpriteAnimationInfo.StartIndex);
    public bool Finished => !Loop && FrameRate != 0 && RawFrameIndex >= TotalFrames - 1;
    public float TotalTime => (float)TotalFrames / FrameRate;

    public int TimeOf(int frame)
    {
        return frame / FrameRate;
    }
    // public SpriteAnimation(string name) : this(name, 0)
    // {

    // }
    public SpriteAnimation(SpriteAnimInfoID nameID) : this(nameID, 0)
    {

    }
    // public SpriteAnimation(string name, float rawFrameIndex) : this(SpriteAnimationManager.GetIDFromName(name), rawFrameIndex)
    // {


    // }

    public SpriteAnimation(SpriteAnimInfoID nameID, float rawFrameIndex)
    {
        Name = nameID;
        var info = SpriteAnimationInfo;
        TotalFrames = info.FrameCount;
        FrameRate = info.FrameRate;
        Loop = info.Looping;
        RawFrameIndex = rawFrameIndex;
        FacingRight = info.FacingRight;
    }

    // public SpriteAnimation(string name, int frameRate, int totalFrames, bool loop, float rawFrameIndex = 0)
    //     : this(SpriteAnimationManager.GetIDFromName(name), frameRate, totalFrames, loop, rawFrameIndex)
    // {

    // }
    public SpriteAnimation(SpriteAnimInfoID nameID, int frameRate, int totalFrames, bool loop, float rawFrameIndex = 0, bool facingRight = false)
    {
        Name = nameID;
        FrameRate = frameRate;
        TotalFrames = totalFrames;
        Loop = loop;
        RawFrameIndex = rawFrameIndex;
        FacingRight = facingRight;
    }
    public SpriteAnimation Update(float dt)
    {
        return new SpriteAnimation(
            Name,
            FrameRate,
            TotalFrames,
            Loop,
            RawFrameIndex + (FrameRate * dt),
            FacingRight
        );
    }
}