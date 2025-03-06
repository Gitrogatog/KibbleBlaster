using Godot;
using MyECS.Components;
public static class DrawUtils
{
    public static void DrawSpriteAnim(Vector2 position, SpriteAnimation spriteAnim, CanvasItem drawNode, Texture2D texture, bool flip = false, bool flipVert = false)
    {
        var spriteRect = spriteAnim.CurrentSprite.Rect;
        var displayRect = Utilities.CenterRect(new Rect2(position, spriteRect.Size));
        if (flip)
        {
            displayRect.Size = new Vector2(-displayRect.Size.X, displayRect.Size.Y);
        }
        if (flipVert)
        {
            displayRect.Size = new Vector2(displayRect.Size.X, -displayRect.Size.Y);
        }
        drawNode.DrawTextureRectRegion(texture, displayRect, spriteRect);
    }
}