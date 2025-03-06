namespace MyECS;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;

public class CameraSystem : System
{
    public Filter EntityFilter;
    Camera2D camera;
    const float maxCameraDistance = 1000f;
    const float decay = 8;
    Vector2 cameraPos;
    public CameraSystem(World world, Camera2D camera) : base(world)
    {
        this.camera = camera;
        EntityFilter = FilterBuilder
            .Include<Position>()
            .Include<CameraTarget>()
            .Build();
    }
    public override void Update(TimeSpan delta)
    {
        if (Input.GetActionRawStrength("lock_camera") > 0)
        {
            return;
        }
        if (!EntityFilter.Empty)
        {
            var entity = EntityFilter.NthEntity(0);
            Vector2 targetPos = Get<Position>(entity).Value;
            camera.Position = targetPos;
            // cameraPos = LerpByDistance(cameraPos, targetPos, (float)delta.TotalSeconds);
            // camera.Position = new Vector2I((int)cameraPos.X, (int)cameraPos.Y);
        }
    }
    Vector2 LerpByDistance(Vector2 a, Vector2 b, float delta)
    {
        float distance = a.DistanceTo(b);
        // float t = Easings.CircularEaseOut(Mathf.Clamp((maxCameraDistance - distance) / maxCameraDistance, 0, 1));
        // return Utilities.Lerp(a, b, t);
        return Utilities.LerpDecay(a, b, decay, delta);
    }
}