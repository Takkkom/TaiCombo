using System.Text;
using NLua;
using TaiCombo.Engine.Enums;

namespace TaiCombo.Engine;

public class LuaFuncs
{
    private Dictionary<string, Sprite> Sprites = new();

    private string BaseDir;

    public LuaFuncs(string fileName)
    {
        BaseDir = $"{fileName}{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}";
    }

    public void AddSprite(string fileName)
    {
        string truePath = fileName.Replace('/', Path.DirectorySeparatorChar);
        Sprites.Add(fileName, new Sprite($"{BaseDir}{truePath}"));
    }

    public void DrawSpriteOpacity(float x, float y, float opacity, string fileName)
    {
        Sprite sprite = Sprites[fileName];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, opacity, "Left_Up", "Normal", fileName);
    }

    public void DrawSpriteBlendOpacity(float x, float y, float opacity, string blend, string fileName)
    {
        Sprite sprite = Sprites[fileName];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, opacity, "Left_Up", blend, fileName);
    }

    public void DrawSprite(float x, float y, string fileName)
    {
        Sprite sprite = Sprites[fileName];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, 1, "Left_Up", "Normal", fileName);
    }

    public void DrawSpriteFull(
        float x, float y, 
        float scaleX, float scaleY, 
        float rectX, float rectY, float rectWidth, float rectHeight, 
        bool flipX, bool flipY, 
        float rotation, 
        float red, float green, float blue,  float alpha,
        string drawOrigin, string blend,
        string fileName)
    {
        Sprite sprite = Sprites[fileName];

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

    public int GetSpriteWidth(string fileName)
    {
        Sprite sprite = Sprites[fileName];
        return sprite.TextureSize.Width;
    }

    public int GetSpriteHeight(string fileName)
    {
        Sprite sprite = Sprites[fileName];
        return sprite.TextureSize.Height;
    }
}