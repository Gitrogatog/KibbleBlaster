namespace MyECS.Systems;
using System;
using System.Collections.Generic;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Messages;

public class PlayerHealthBarSystem : System
{
    RichTextLabel textNode;
    string currentText;
    Filter PlayerFilter;
    Entity[] HealthSprites = new Entity[20];
    int currentHealth;
    int maxHealth;
    Vector2 screenOffset = new Vector2(20, 20);
    Vector2 spriteOffset = new Vector2(12, 0);
    public PlayerHealthBarSystem(World world, RichTextLabel textNode) : base(world)
    {
        this.textNode = textNode;
        PlayerFilter = FilterBuilder.Include<ControlledByPlayer>().Include<Health>().Build();
    }
    public override void Update(TimeSpan delta)
    {
        foreach (Entity entity in PlayerFilter.Entities)
        {
            var health = Get<Health>(entity);
            int maxDiff = health.Max - maxHealth;
            if (health.Max > maxHealth)
            {
                for (int i = maxHealth; i < health.Max; i++)
                {
                    Entity e = CreateEntity();
                    Set(entity, new Position(screenOffset + i * spriteOffset));
                    Set(entity, new SetAnimation("HeartFull", 0));
                    HealthSprites[i] = e;
                }
            }
            else if (health.Max < maxHealth)
            {
                for (int i = maxHealth - 1; i >= health.Max; i++)
                {
                    Destroy(HealthSprites[i]);
                }
            }
            maxHealth = health.Max;

            if (health.Current > currentHealth)
            {

            }
            else if (health.Current < currentHealth)
            {

            }
            currentHealth = health.Current;

        }
    }
}