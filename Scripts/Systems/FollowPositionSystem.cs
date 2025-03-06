namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Relations;

public class FollowPositionSystem : System
{

    public FollowPositionSystem(World world) : base(world)
    {

    }
    public override void Update(TimeSpan delta)
    {
        foreach (var pair in Relations<FollowPosition>())
        {
            var targetPosition = Get<Position>(pair.Item1).RawValue;
            bool facing = Has<Facing>(pair.Item1) && !Get<Facing>(pair.Item1).Right;
            Vertical vert = Has<VerticalAimDirection>(pair.Item1) ? Get<VerticalAimDirection>(pair.Item1).Value : Vertical.Neutral;
            Set(pair.Item2, new Facing(facing));
            if (Has<SpriteOffset>(pair.Item2))
            {
                Vector2I offset = Get<SpriteOffset>(pair.Item2).Value;
                if (vert == Vertical.Up)
                {
                    offset += new Vector2I(-1, -2);
                }
                if (facing)
                {
                    offset = new Vector2I(-offset.X, offset.Y);
                }
                targetPosition += offset;
            }
            Set(pair.Item2, new Position(targetPosition));

            if (Has<FollowParentAimAnimGroup>(pair.Item2))
            {
                var animData = Get<FollowParentAimAnimGroup>(pair.Item2);

                var animID = vert switch
                {
                    Vertical.Up => animData.Up,
                    Vertical.Neutral => animData.Neutral,
                    Vertical.Down => animData.Down,
                    _ => animData.Neutral
                };
                EntityMods.SetAnimation(World, pair.Item2, animID, 0);
            }
        }
    }
}