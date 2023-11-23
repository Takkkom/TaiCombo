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
}