using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Plugin.Chart;

namespace TaiCombo.Helper;

static class EasingHelper
{
    const float C1 = 1.70158f;
    const float C3 = C1 + 1;

    public static float EaseOutBack(float value)
    {
        return 1 + C3 * MathF.Pow(value - 1, 3) + C1 * MathF.Pow(value - 1, 2);
    }

    public static float EaseInBack(float value)
    {
        return C3 * value * value * value - C1 * value * value;
    }

    public static float BasicInOut(float value)
    {
        return (value < 0.5f ? value : (1.0f - value)) * 2;
    }

    public static float ResultNumberIn(float value, float ext)
    {
        return 1 + (ext * (1.0f - value));
    }

    public static float ResultScoreNumberIn(float value, float ext1, float ext2, float timing)
    {
        float scale = 1;
        if (value < timing)
        {
            float value1 = value / timing;
            scale += MathF.Sin(value1 * MathF.PI) * ext1;
        }
        else if (value < 1) 
        {
            float value2 = (value - timing) / (1.0f - timing);
            scale += BasicInOut(value2) * ext2;
        }
        return scale;
    }

    public static float ResultScoreRankIn(float value, float ext1, float ext2, float timing)
    {
        float scale = 1;
        if (value < timing)
        {
            float value1 = MathF.Cos((value / timing) * MathF.PI / 2.0f);
            scale += (value1 * ext1) + ((1.0f - value1) * ext2);
        }
        else if (value < 1) 
        {
            float value2 = (value - timing) / (1.0f - timing);
            scale += (1.0f - value2) * ext2;
        }
        return scale;
    }
}