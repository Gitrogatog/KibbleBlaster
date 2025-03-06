namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public class Collision : System
{

    public Collision(World world) : base(world)
    {
    }
    public override void Update(TimeSpan delta)
    {
        Entity entityA;
        Entity entityB;
        // GD.Print("collision data");
        foreach (var pair in Relations<Colliding>())
        {
            entityA = GetCollisionParent(pair.Item1);
            entityB = GetCollisionParent(pair.Item2);
            if (Has<DestroyOnContact>(entityA))
            {
                Set(entityA, new MarkForDestroy());
            }
            if (Has<DealDamageOnContact>(entityA) && Has<Health>(entityB))
            {
                // GD.Print("Dealing damage!");
                int damage = Get<DealDamageOnContact>(entityA).Damage;
                MessagePrefabs.CreateDamage(World, entityB, damage);
            }
            if (Has<ChangeLevelOnCollide>(entityA))
            {
                MessagePrefabs.ChangeLevel(World, Get<ChangeLevelOnCollide>(entityA).Level);
                GD.Print("collision caused level change!");
            }
            // GD.Print($"1: {entityA} 2: {entityB}");
        }
    }
    Entity GetCollisionParent(Entity entity)
    {
        if (HasInRelation<HasCollider>(entity))
        {
            return InRelationSingleton<HasCollider>(entity);
        }
        return entity;
    }
}