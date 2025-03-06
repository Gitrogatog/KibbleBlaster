namespace MyECS.Components;

public struct Timer
{
    public float Time { get; private set; }
    public float Max { get; }
    public float Remaining
    {
        get
        {
            return Time / Max;
        }
    }

    public Timer(float time)
    {
        Time = Max = time;
    }

    public Timer Update(float newTime)
    {
        Time = newTime;
        return this;
    }
}