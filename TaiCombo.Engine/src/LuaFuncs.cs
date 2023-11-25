using System.Text;
using NLua;
using TaiCombo.Engine.Enums;

namespace TaiCombo.Engine;

public class LuaFuncs : IDisposable
{
    private List<Sprite> Sprites = new();

    private string BaseDir;

    public LuaFuncs(string fileName)
    {
        BaseDir = $"{fileName}{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}";
    }

    public int AddSprite(string fileName)
    {
        string truePath = fileName.Replace('/', Path.DirectorySeparatorChar);
        Sprites.Add(new Sprite($"{BaseDir}{truePath}"));

        return Sprites.Count - 1;
    }

    public void DrawSpriteOpacity(float x, float y, float opacity, int index)
    {
        Sprite sprite = Sprites[index];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, opacity, "Left_Up", "Normal", index);
    }

    public void DrawSpriteBlendOpacity(float x, float y, float opacity, string blend, int index)
    {
        Sprite sprite = Sprites[index];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, opacity, "Left_Up", blend, index);
    }

    public void DrawSprite(float x, float y, int index)
    {
        Sprite sprite = Sprites[index];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, 1, "Left_Up", "Normal", index);
    }

    public void DrawSpriteOrigin(float x, float y, string drawOrigin, int index)
    {
        Sprite sprite = Sprites[index];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, 1, drawOrigin, "Normal", index);
    }

    public void DrawSpriteOriginScale(float x, float y, float scaleX, float scaleY, string drawOrigin, int index)
    {
        Sprite sprite = Sprites[index];
        DrawSpriteFull(x, y, scaleX, scaleY, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, 1, drawOrigin, "Normal", index);
    }

    public void DrawSpriteOriginScaleAlpha(float x, float y, float scaleX, float scaleY, float alpha, string drawOrigin, int index)
    {
        Sprite sprite = Sprites[index];
        DrawSpriteFull(x, y, scaleX, scaleY, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, alpha, drawOrigin, "Normal", index);
    }

    public void DrawSpriteOriginRotation(float x, float y, float rotation, string drawOrigin, int index)
    {
        Sprite sprite = Sprites[index];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, rotation, 1, 1, 1, 1, drawOrigin, "Normal", index);
    }

    public void DrawSpriteFull(
        float x, float y, 
        float scaleX, float scaleY, 
        float rectX, float rectY, float rectWidth, float rectHeight, 
        bool flipX, bool flipY, 
        float rotation, 
        float red, float green, float blue,  float alpha,
        string drawOrigin, string blend,
        int index)
    {
        Sprite sprite = Sprites[index];

        DrawOriginType drawOriginType;
        switch(drawOrigin)
        {
            case "Left_Up":
            drawOriginType = DrawOriginType.Left_Up;
            break;
            case "Left":
            drawOriginType = DrawOriginType.Left;
            break;
            case "Left_Down":
            drawOriginType = DrawOriginType.Left_Down;
            break;
            case "Up":
            drawOriginType = DrawOriginType.Up;
            break;
            case "Center":
            drawOriginType = DrawOriginType.Center;
            break;
            case "Down":
            drawOriginType = DrawOriginType.Down;
            break;
            case "Right_Up":
            drawOriginType = DrawOriginType.Right_Up;
            break;
            case "Right":
            drawOriginType = DrawOriginType.Right;
            break;
            case "Right_Down":
            drawOriginType = DrawOriginType.Right_Down;
            break;
            default:
            drawOriginType = DrawOriginType.Left_Up;
            break;
        }

        BlendType blendType;
        switch(blend)
        {
            case "Normal":
            blendType = BlendType.Normal;
            break;
            case "Add":
            blendType = BlendType.Add;
            break;
            case "Screen":
            blendType = BlendType.Screen;
            break;
            case "Multi":
            blendType = BlendType.Multi;
            break;
            case "Sub":
            blendType = BlendType.Sub;
            break;
            default:
            blendType = BlendType.Normal;
            break;
        }

        sprite.Draw(x, y, scaleX, scaleY, new System.Drawing.RectangleF(rectX, rectY, rectWidth, rectHeight), flipX, flipY, rotation, new Struct.Color4(red, green, blue, alpha), drawOriginType, blendType);
    }

    public int GetSpriteWidth(int index)
    {
        Sprite sprite = Sprites[index];
        return sprite.TextureSize.Width;
    }

    public int GetSpriteHeight(int index)
    {
        Sprite sprite = Sprites[index];
        return sprite.TextureSize.Height;
    }

    public void Dispose()
    {
        for(int i = 0; i < Sprites.Count; i++)
        {
            Sprites[i].Dispose();
        }
        Sprites.Clear();
    }
}