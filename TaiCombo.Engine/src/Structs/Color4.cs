
namespace TaiCombo.Engine.Struct;

public struct Color4 
{
    public static Color4 White = new Color4() { R = 1.0f, G = 1.0f, B = 1.0f, A = 1.0f };

    public Color4(float r, float g, float b, float a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }
    public float A { get; set; }
}