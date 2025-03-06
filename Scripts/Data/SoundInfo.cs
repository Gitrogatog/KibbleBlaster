
using Godot;
using MyECS.Components;

public struct SoundInfo
{
    static SoundInfo[] sounds = new SoundInfo[(int)SoundName.MAX];
    public SoundName Name;
    public AudioStreamPlayer2D AudioNode;
    public static void RegisterSound(SoundName name, AudioStreamPlayer2D node)
    {
        sounds[(int)name] = new SoundInfo(name, node);
    }
    public static SoundInfo FromName(SoundName name)
    {
        return sounds[(int)name];
    }
    SoundInfo(SoundName name, AudioStreamPlayer2D node)
    {
        Name = name;
        AudioNode = node;
    }
}