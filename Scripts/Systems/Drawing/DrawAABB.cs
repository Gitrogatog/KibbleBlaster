namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public class DrawAABB : System
{
    public Filter EntityFilter;
    CanvasItem drawNode;

    public DrawAABB(World world, CanvasItem drawNode) : base(world)
    {
        this.drawNode = drawNode;
        EntityFilter = FilterBuilder
            .Include<AABB>()
            .Build();
    }
    AABB GetColliderData(Entity entity)
    {
        var aabb = Get<AABB>(entity);
        var ogEntity = entity;

        if (HasInRelation<HasCollider>(entity))
        {
            var parent = InRelationSingleton<HasCollider>(entity);
            aabb = aabb.ToWorld(Get<Position>(parent).Value);
            ogEntity = parent;
        }
        return aabb;
    }
    public override void Update(TimeSpan delta)
    {
        foreach (var entity in EntityFilter.Entities)
        {

            // ColoredRect coloredRect = Get<ColoredRect>(entity);
            var aabb = GetColliderData(entity);
            // Vector2 position = Get<Position>(entity).Value;

            Rect2 rect = Utilities.AABBtoRect(aabb); //new(position + scanForGround.Offset - scanForGround.Size / 2, scanForGround.Size);
            drawNode.DrawRect(rect, Colors.White);
        }
    }
}