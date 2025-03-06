namespace MyECS;
using System;
using System.Collections.Generic;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public static class CharBodyStorage
{
    static Dictionary<CharacterBody2D, int> ClassToID = new Dictionary<CharacterBody2D, int>();
    static CharacterBody2D[] IDToClass = new CharacterBody2D[256];
    // static Stack<int> OpenIDs = new Stack<int>();
    static int NextID = 0;

    public static CharacterBody2D GetClass(int id)
    {
        return IDToClass[id];
    }

    public static int GetID(CharacterBody2D ogClass)
    {
        if (!ClassToID.ContainsKey(ogClass))
        {
            RegisterClass(ogClass);
        }

        return ClassToID[ogClass];
    }

    private static void RegisterClass(CharacterBody2D ogClass)
    {
        if (NextID >= IDToClass.Length)
        {
            Array.Resize(ref IDToClass, IDToClass.Length * 2);
        }
        ClassToID[ogClass] = NextID;
        IDToClass[NextID] = ogClass;
        NextID += 1;
        // if (OpenIDs.Count == 0)
        // {

        // }
        // else
        // {
        //     ClassToID[ogClass] = OpenIDs.Pop();
        // }
    }
    public static CharBody CreateComponent(CharacterBody2D ogClass, Entity entity, Vector2 center, Vector2 size)
    {
        ogClass.SetMeta("Entity", entity.ID);
        int id = GetID(ogClass);
        CollisionShape2D collisionShape = ogClass.GetNode<CollisionShape2D>("CollisionShape2D");
        collisionShape.Shape = RectangleShapeResourceStorage.GetClass(size);
        collisionShape.Position = center;
        Rect2 box = new Rect2(center - size * 0.5f, size);
        // Rect2 box = collisionShape.Shape.GetRect();
        // GD.Print($"collision box position pre: {box.Position}");
        // box.Position = collisionShape.Position;
        // GD.Print($"collision box position post: {box.Position}");
        return new CharBody(id, box);
    }
    public static void DestroyAllCharBodies()
    {
        foreach (CharacterBody2D charBody in ClassToID.Keys)
        {
            charBody.QueueFree();
        }
        NextID = 0;
        ClassToID.Clear();
    }
    public static void DestroyBody(int ID)
    {
        var body = IDToClass[ID];
        if (body == null) return;
        ClassToID.Remove(body);
        body.QueueFree();
    }
}

public static class RectangleShapeResourceStorage
{
    // static Dictionary<RectangleShape2D, Vector2I> ClassToSize = new Dictionary<RectangleShape2D, Vector2I>();
    static Dictionary<Vector2, RectangleShape2D> SizeToClass = new Dictionary<Vector2, RectangleShape2D>();
    public static RectangleShape2D GetClass(Vector2 size)
    {
        if (!SizeToClass.ContainsKey(size))
        {
            RectangleShape2D shape = new RectangleShape2D();
            shape.Size = size;
            SizeToClass[size] = shape;
            return shape;
        }
        return SizeToClass[size];
    }

}