using System.Security.Cryptography;
using Godot;
using MoonTools.ECS;
using MyECS;
using MyECS.Components;
using MyECS.Relations;

public static class BulletPrefabs
{
    public static Entity CreateBullet(World World, BulletType type, CollisionLayer mask, Vector2 position, Vector2 aimDir)
    {
        GD.Print($"creating bullet of type {type}");
        var entity = type switch
        {
            BulletType.Flame => CreateFlame(World, position, 120, aimDir, mask),
            BulletType.Wave => CreateWave(World, position, 60, aimDir, mask),
            _ => World.CreateEntity()
        };


        // World.Set(entity, new SineWaveMovement(35, 4f));
        // Set(entity, )
        // distance
        // World.Set(entity, layer);
        return entity;
    }
    static Entity CreateFlame(World World, Vector2 position, float moveSpeed, Vector2 direction, CollisionLayer mask)
    {
        var entity = CreateBase(World, position, moveSpeed * direction, mask, 6, 4, 0.4f, true);
        // World.Set(entity, new ColoredRect(new Vector2(10, 10), Colors.Blue));
        World.Set(entity, new SetAnimation("Flame", 0));

        // World.Set(entity, new MoveSpeed(200f));
        World.Set(entity, new DealDamageOnContact(1));
        World.Set(entity, new DestroyOnContact());
        // World.Set(entity, new ContinuousMove(direction));

        return entity;
    }
    static Entity CreateWave(World World, Vector2 position, float moveSpeed, Vector2 direction, CollisionLayer mask)
    {
        var entity = CreateBase(World, position, moveSpeed * direction, mask, 7, 10, 5f);
        World.Set(entity, new SetAnimation("Wave", 0));
        return entity;

    }
    static Entity CreateBase(World World, Vector2 position, Vector2 velocity, CollisionLayer mask, int sizeX, int sizeY, float lifetime = 0, bool destroyOnTouchGround = false)
    {
        var entity = World.CreateEntity();
        World.Set(entity, new Position(position));
        World.Set(entity, new Velocity(velocity));
        EntityPrefabs.AddCollider(World, entity, 0, 0, 6, 4, CollisionLayer.Bullet, mask);
        if (lifetime > 0)
        {
            World.Set(entity, new MyECS.Components.Timer(lifetime));
        }
        if (destroyOnTouchGround)
        {
            World.Set(entity, new ScanForGround(new Vector2(), new Vector2(sizeX, sizeY)));
            World.Set(entity, new DestroyOnTouchGround());
        }
        World.Set(entity, new DealDamageOnContact(1));
        World.Set(entity, new Facing(velocity.X > 0));
        return entity;
    }
}