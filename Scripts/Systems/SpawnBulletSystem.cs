namespace MyECS;
using System;
using System.Security.Cryptography;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public class SpawnBulletSystem : System
{
    public Filter EntityFilter;

    public SpawnBulletSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<AttemptShootThisFrame>()
            .Include<CanShoot>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {
            if (!HasInRelation<DisableShoot>(entity))
            {
                var position = Get<Position>(entity).Value;
                bool facing = !Has<Facing>(entity) || Get<Facing>(entity).Right;
                Vector2 direction = facing ? Vector2.Right : Vector2.Left;
                if (World.TryGetComponent(entity, out VerticalAimDirection vert))
                {
                    if (vert.Value == Vertical.Up)
                    {
                        direction = Vector2.Up;
                    }
                }

                Vector2I origin = GetOrigin(entity);
                if (!facing)
                {
                    origin = new Vector2I(-origin.X, origin.Y);
                }
                position += origin;

                var shootData = Get<CanShoot>(entity);
                BulletPrefabs.CreateBullet(World, shootData.Bullet, shootData.IsEnemy ? CollisionLayer.Player : CollisionLayer.Enemy, position, direction);
                var timer = EntityPrefabs.CreateTimer(World, shootData.Delay);
                Relate(timer, entity, new DisableShoot());
                if (Has<ShootAnimGroup>(entity))
                {
                    var anim = Get<ShootAnimGroup>(entity);
                    EntityMods.SetTimedAnimation(World, entity, anim.Animation, anim.Priority, anim.Seconds);
                }
            }
        }
    }
    Vector2I GetOrigin(Entity entity)
    {
        if (Has<ShootOrigin>(entity))
        {
            return Get<ShootOrigin>(entity).Value;
        }
        else if (Has<VerticalShootOrigin>(entity))
        {
            var origin = Get<VerticalShootOrigin>(entity);
            if (Has<VerticalAimDirection>(entity))
            {
                return Get<VerticalAimDirection>(entity).Value switch
                {
                    Vertical.Up => origin.Up,
                    _ => origin.Neutral
                };
            }
            else
            {
                return origin.Neutral;
            }
        }
        return Vector2I.Zero;
    }
}