namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public class HealthSystem : System
{
    public Filter EntityFilter;
    const float flashLenght = 0.2f;

    public HealthSystem(World world) : base(world)
    {
        EntityFilter = FilterBuilder
            .Include<Damage>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {

        foreach (var pair in Relations<Damage>())
        {
            // GD.Print("Doing damage calcs");
            var entity = pair.Item2;
            if (HasInRelation<Invincible>(entity))
            {
                // GD.Print("invincible!!!");
                continue;
            }
            int damage = GetRelationData<Damage>(pair.Item1, entity).Value;
            Health health = Get<Health>(entity);
            int amount = health.Current - damage;
            Set(entity, new Health(amount, health.Max));
            Set(entity, new WhiteFlash(flashLenght));
            Set(entity, new TookDamageThisFrame());
            GD.Print($"damage: {damage} current health: {amount}");
            if (amount <= 0)
            {
                // this entity is dead
                GD.Print("killing this entity!");
                Set(entity, new MarkForDestroy());
            }
            else if (Has<BecomeInvincibleOnDamage>(entity))
            {
                float time = Get<BecomeInvincibleOnDamage>(entity).Time;
                MessagePrefabs.CreateInvincibility(World, entity, time);
            }


        }

    }
}