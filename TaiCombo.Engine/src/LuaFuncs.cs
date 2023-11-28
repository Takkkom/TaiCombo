using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Struct;

namespace TaiCombo.Engine;

public class LuaFuncs : IDisposable
{
    private List<Sprite> Sprites = new();

    private FontRenderer MainFont;
    private FontRenderer SubFont;

    private string BaseDir;

    public LuaFuncs(string fileName, FontRenderer mainFont, FontRenderer subFont)
    {
        BaseDir = $"{fileName}{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}";
        MainFont = mainFont;
        SubFont = subFont;
    }

    public int AddSprite(string fileName)
    {
        string truePath = fileName.Replace('/', Path.DirectorySeparatorChar);
        Sprites.Add(new Sprite($"{BaseDir}{truePath}"));

        return Sprites.Count;
    }
    
    public int AddMainFontSprite(string text, float r, float g, float b, float size)
    {
        Sprites.Add(MainFont.GenSpriteText(text, new (r, g, b, 1), size));

        return Sprites.Count;
    }

    public int AddMainFontEdgeSprite(string text, float r, float g, float b, float size, float edge_r, float edge_g, float edge_b, float edgeRatio)
    {
        Sprites.Add(MainFont.GenSpriteText(text, new (r, g, b, 1), size, new EdgeInfo[] { new EdgeInfo() { Color = new (edge_r, edge_g, edge_b, 1), Ratio = edgeRatio } }));

        return Sprites.Count;
    }
    
    public int AddSubFontSprite(string text, float r, float g, float b, float size)
    {
        Sprites.Add(SubFont.GenSpriteText(text, new (r, g, b, 1), size));

        return Sprites.Count;
    }

    public int AddSubFontEdgeSprite(string text, float r, float g, float b, float size, float edge_r, float edge_g, float edge_b, float edgeRatio)
    {
        Sprites.Add(SubFont.GenSpriteText(text, new (r, g, b, 1), size, new EdgeInfo[] { new EdgeInfo() { Color = new (edge_r, edge_g, edge_b, 1), Ratio = edgeRatio } }));

        return Sprites.Count;
    }

    public void DrawSpriteOpacity(float x, float y, float opacity, int index)
    {
        Sprite sprite = Sprites[index - 1];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, opacity, "Left_Up", "Normal", index);
    }

    public void DrawSpriteBlendOpacity(float x, float y, float opacity, string blend, int index)
    {
        Sprite sprite = Sprites[index - 1];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, opacity, "Left_Up", blend, index);
    }

    public void DrawSprite(float x, float y, int index)
    {
        Sprite sprite = Sprites[index - 1];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, 1, "Left_Up", "Normal", index);
    }

    public void DrawSpriteOrigin(float x, float y, string drawOrigin, int index)
    {
        Sprite sprite = Sprites[index - 1];
        DrawSpriteFull(x, y, 1, 1, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, 1, drawOrigin, "Normal", index);
    }

    public void DrawSpriteOriginScale(float x, float y, float scaleX, float scaleY, string drawOrigin, int index)
    {
        Sprite sprite = Sprites[index - 1];
        DrawSpriteFull(x, y, scaleX, scaleY, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, 1, drawOrigin, "Normal", index);
    }

    public void DrawSpriteOriginScaleAlpha(float x, float y, float scaleX, float scaleY, float alpha, string drawOrigin, int index)
    {
        Sprite sprite = Sprites[index - 1];
        DrawSpriteFull(x, y, scaleX, scaleY, 0, 0, sprite.TextureSize.Width, sprite.TextureSize.Height, false, false, 0, 1, 1, 1, alpha, drawOrigin, "Normal", index);
    }

    public void DrawSpriteOriginRotation(float x, float y, float rotation, string drawOrigin, int index)
    {
        Sprite sprite = Sprites[index - 1];
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
        Sprite sprite = Sprites[index - 1];

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
        Sprite sprite = Sprites[index - 1];
        return sprite.TextureSize.Width;
    }

    public int GetSpriteHeight(int index)
    {
        Sprite sprite = Sprites[index - 1];
        return sprite.TextureSize.Height;
    }

    public int GetFileCount(string dirPath, string searchPattern)
    {
        string truePath = dirPath.Replace('/', Path.DirectorySeparatorChar);
        string[] files = Directory.GetFiles($"{BaseDir}{truePath}", searchPattern);
        return files.Length;
    }

    public void DisposeSprite(int index)
    {
        if (index < 1) return;
        Sprites[index - 1].Dispose();
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