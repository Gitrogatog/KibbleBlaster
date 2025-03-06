using Godot;

public class SpriteAtlasBuilder
{
    Texture2D texture = new Texture2D();
    void ReadTexture(Texture2D tex)
    {
        Image image = new Image();
        Image im = tex.GetImage();
        im.Free();
        // ImageTexture t = new ImageTexture();
        // t.Update(Image)
    }

}