namespace MyECS.Components;

public readonly record struct Health
{
    public int Current { get; }
    public int Max { get; }
    public Health(int amount, int max)
    {
        Current = amount;
        Max = max;
    }
    public Health(int amount)
    {
        Current = amount;
        Max = amount;
    }
    public Health DealDamage(int amount)
    {
        return new Health(Current - amount);
    }
}