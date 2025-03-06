namespace MyECS;
using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;


public class Motion : System
{
    Filter LayerFilter;
    Filter MaskFilter;
    Filter InteractFilter;
    Filter VelocityFilter;
    const int CellSize = 16;
    const float CellReciprocal = 1.0f / CellSize;
    SpatialHash<Entity> InteractHash = new SpatialHash<Entity>(0, 0, 1000, 1000, 32);
    SpatialHash<Entity> SolidHash = new SpatialHash<Entity>(0, 0, 1000, 1000, 32);
    // Dictionary<(int, int), HashSet<Entity>> SpatialHash = new Dictionary<(int, int), HashSet<Entity>>();
    void ClearInteractHash()
    {
        InteractHash.Clear();
    }

    void ClearSolidHash()
    {
        SolidHash.Clear();
    }

    public Motion(World world) : base(world)
    {
        InteractFilter = FilterBuilder.Include<AABB>().Build();
        LayerFilter = FilterBuilder.Include<AABB>().Include<Layer>().Build();
        MaskFilter = FilterBuilder.Include<AABB>().Include<Mask>().Build();
        VelocityFilter = FilterBuilder.Include<Position>().Include<Velocity>().Build();
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
        // clear the spatial hashes
        ClearInteractHash();
        ClearSolidHash();

        // move the entities
        foreach (var entity in VelocityFilter.Entities)
        {
            //     Vector2 intendedMove = Get<IntendedMove>(entity).Value;
            //     float moveSpeed = Has<MoveSpeed>(entity) ? Get<MoveSpeed>(entity).Value : 100f;
            Vector2 velocity = Get<Velocity>(entity).Value;
            var currentPos = Get<Position>(entity);
            var movement = velocity * (float)delta.TotalSeconds;
            var targetPos = currentPos + movement;
            if (Has<CharBody>(entity))
            {
                CharacterBody2D body = CharBodyStorage.GetClass(Get<CharBody>(entity).ID);
                body.Velocity = velocity;
                body.MoveAndSlide();
                Set(entity, new Position(body.GlobalPosition));
                // body.GetSlideCollision
            }
            else
            {
                Set(entity, targetPos);
            }

            // GD.Print($"current pos: {currentPos}");
            // Remove<IntendedMove>(entity);
        }
        // refill the hash and remove colliding relations from last frame
        foreach (var entity in LayerFilter.Entities)
        {
            var aabb = GetColliderData(entity);
            // var position = Get<Position>(entity).Value;
            // var aabb = Get<AABB>(entity).ToWorld(position);
            // var ogEntity = entity;

            // if (HasInRelation<HasCollider>(entity))
            // {
            //     var parent = InRelationSingleton<HasCollider>(entity);
            //     aabb = aabb.ToWorld(Get<Position>(parent).Value);
            //     ogEntity = parent;
            // }

            InteractHash.Insert(entity, aabb);
            // remove collision relations
            foreach (var other in OutRelations<Colliding>(entity))
            {
                Unrelate<Colliding>(entity, other);
            }
        }

        foreach (var entity in MaskFilter.Entities)
        {
            var aabb = GetColliderData(entity);

            foreach (var (other, otherRect) in InteractHash.Retrieve(entity, aabb))
            {
                var layer = Get<Layer>(other).Value;
                var mask = Get<Mask>(entity).Value;

                if ((layer & mask) != 0 && aabb.SimpleOverlap(otherRect))
                {
                    Relate(entity, other, new Colliding());
                }
            }
        }
        // foreach (var entity in MaskFilter.Entities)
        // {
        //     CollisionLayer mask = Get<Mask>(entity).Value;
        //     if (mask == 0) continue;
        //     AABB box1 = Get<AABB>(entity);
        //     foreach (var otherEntity in LayerFilter.Entities)
        //     {
        //         if (entity == otherEntity) continue;

        //     }
        // }
    }
}