using SkiaSharp;
using TaiCombo.Engine.Struct;

namespace TaiCombo.Engine;

public class FontRenderer : IDisposable
{
    private SKPaint Paint;
    private int Size;

    public FontRenderer(string filePath)
    {
        Paint = new();
        Paint.Typeface = SKFontManager.Default.CreateTypeface(filePath);
        Paint.IsAntialias = true;
    }


    public SKBitmap GenEdgeText(string text, Color4 color, float size, float edgeSize)
    {
        Paint.TextSize = size;
        Paint.ColorF = new(color.R, color.G, color.B, color.A);
        Paint.Style = SKPaintStyle.Stroke;
        Paint.StrokeWidth = edgeSize;

        float addSize = edgeSize * 2;
        int width = (int)Math.Ceiling(Paint.MeasureText(text) + addSize);
        int height = (int)Math.Ceiling(Paint.FontMetrics.CapHeight + addSize);
        SKBitmap bitmap = new(width, height);
        using SKCanvas canvas = new(bitmap);

        canvas.DrawText(text, 0 + edgeSize, Paint.FontMetrics.CapHeight + edgeSize, Paint);
        
        return bitmap;
    }
    public SKBitmap GenText(string text, Color4 color, float size)
    {
        Paint.TextSize = size;
        Paint.ColorF = new(color.R, color.G, color.B, color.A);
        Paint.Style = SKPaintStyle.Fill;

        int addSize = (int)MathF.Ceiling(size * 0.2f);
        int width = (int)MathF.Ceiling(Paint.MeasureText(text));
        int height = (int)MathF.Ceiling(Paint.FontMetrics.CapHeight);
        SKBitmap bitmap = new(width + addSize, height + addSize);
        using SKCanvas canvas = new(bitmap);

        int offset = (int)(addSize / 2.0f);
        canvas.DrawText(text, offset, Paint.FontMetrics.CapHeight + offset, Paint);
        
        return bitmap;
    }

    public SKBitmap GenText(string text, Color4 color, float size, EdgeInfo[]? edges = null)
    {
        List<SKBitmap> edgeBitmaps = new();
        SKBitmap baseBitmap = GenText(text, color, size);

        int width = baseBitmap.Width;
        int height = baseBitmap.Height;

        if (edges != null)
        {
            for(int i = 0; i < edges.Length; i++)
            {
                var edge = edges[i];
                int edgeSize = (int)(size * edge.Ratio);
                var edgeBitmap = GenEdgeText(text, edge.Color, size, edgeSize);
                edgeBitmaps.Add(edgeBitmap);

                width = Math.Max(width, edgeBitmap.Width);
                height = Math.Max(height, edgeBitmap.Height);
            }
        }

        float center_x = width / 2.0f;
        float center_y = height / 2.0f;
        SKBitmap bitmap = new(width, height);
        using SKCanvas canvas = new(bitmap);

        if (edges != null)
        {
            for(int i = edges.Length - 1; i >= 0; i--)
            {
                canvas.DrawBitmap(edgeBitmaps[i], center_x - (edgeBitmaps[i].Width / 2.0f), center_y - (edgeBitmaps[i].Height / 2.0f));
            }
        }
        canvas.DrawBitmap(baseBitmap, center_x - (baseBitmap.Width / 2.0f), center_y - (baseBitmap.Height / 2.0f));

        return bitmap;
    }

    public Sprite GenSpriteText(string text, Color4 color, float size, EdgeInfo[]? edges = null)
    {
        using SKBitmap bitmap = GenText(text, color, size, edges);
        return new Sprite(bitmap);
    }

    public void Dispose()
    {
        Paint.Dispose();
    }
}