
using System;
using System.Collections.Generic;
using Godot;
using MyECS.Components;
namespace Assets.Godot;
[GlobalClass]
public partial class AnimationArrayResource : Resource
{
    [Export] AnimationResource[] animations;
    List<Sprite> sprites = new List<Sprite>(10);
    public void Load()
    {
        // SpriteAnimationInfo[] infos = new SpriteAnimationInfo[(int)AnimationName.MAX];
        // int totalFrames = 0;
        // foreach (var animResource in animations)
        // {
        //     int rows = animResource.Rows;
        //     int cols = animResource.Columns;
        //     int frameCount = animResource.FrameCount;
        //     if (frameCount == 0)
        //     {
        //         frameCount = cols * rows;
        //     }
        //     totalFrames += frameCount;
        // }
        SpriteAnimationManager.Init(animations.Length, 100);
        foreach (var animResource in animations)
        {
            var texture = animResource.Frame;

            int rows = animResource.Rows;
            int cols = animResource.Columns;
            int frameCount = animResource.FrameCount;
            if (frameCount == 0)
            {
                frameCount = cols * rows;
            }
            sprites.Clear();
            Rect2 region = texture.Region;
            Vector2 spriteSize = new Vector2(region.Size.X / cols, region.Size.Y / rows);
            Vector2 position = region.Position; //+ texture.Margin.Position;
            if (texture.Atlas is AtlasTexture srcAtlas)
            {
                position += srcAtlas.Region.Position;
                position -= srcAtlas.Margin.Position;
            }
            for (int i = 0; i < frameCount; i++)
            {
                int x = i % cols;
                int y = i / cols;
                Sprite sprite = new Sprite(new Rect2(position + new Vector2(x, y) * spriteSize, spriteSize));
                sprites.Add(sprite);
                // var texture = textures[i];
                // sprites[i] = new Sprite(texture.Region);
            }
            string name = animResource.Name;
            SpriteAnimationManager.RegisterAnimation(sprites, name, animResource.FrameRate, animResource.Looping, animResource.FacingRight);
            // new SpriteAnimationInfo();
            // infos[(int)name] = new SpriteAnimationInfo(sprites, name, animResource.FrameRate, animResource.Looping);
        }
    }
}