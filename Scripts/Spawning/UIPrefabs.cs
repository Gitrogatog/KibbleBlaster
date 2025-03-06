using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;
using MyECS.Relations;

public static class UIPrefabs
{
    static Vector2 backgroundOffset = new Vector2(2, 2);
    public static Entity HealthBar(World World, Entity target, Vector2 position, Vector2 maxSize, Color color)
    {
        Entity healthBar = DependentBar(World, target, position, maxSize, color);
        Entity backFill = DependentBar(World, target, position, maxSize, Colors.Black);
        Entity background = DependentBar(World, target, position - backgroundOffset, maxSize + backgroundOffset * 2, Colors.White);
        // World.Set(entity, new UIRect(position, maxSize, color));
        World.Set(healthBar, new FillBar(maxSize, 1));
        World.Relate(healthBar, target, new DisplayHealth());
        return healthBar;
    }
    static Entity DependentBar(World World, Entity target, Vector2 position, Vector2 maxSize, Color color)
    {
        Entity entity = World.CreateEntity();
        World.Set(entity, new UIRect(position, maxSize, color));
        World.Set(entity, new DestroyOnChangeLevel());
        World.Set(entity, new NeedOutRelation<DestroyOnNotPresent>());
        World.Relate(entity, target, new DestroyOnNotPresent());
        return entity;
    }
}