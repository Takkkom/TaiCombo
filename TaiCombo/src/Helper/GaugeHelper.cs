using System.Drawing;
using TaiCombo.Plugin.Chart;
using TaiCombo.Common;
using TaiCombo.Structs;
using TaiCombo.Enums;
using TaiCombo.Plugin.Enums;

namespace TaiCombo.Helper;

static class GaugeHelper
{
    public static Dictionary<GaugeType, float> ClearLine = new Dictionary<GaugeType, float>
    {
        { GaugeType.Level0, 60},
        { GaugeType.Level1, 70},
        { GaugeType.Level2, 80},
        { GaugeType.Hard, 0.1f},
        { GaugeType.Extreme, 0.1f},
        { GaugeType.ExHard, 0.1f},
        { GaugeType.Dan, 101},
    };
    public static AddGauges GetAddGauge(List<IChip> chips, CourseType courseType, float level)
    {
        AddGauges addScores = new();

        addScores.Perfect = 1.0f;
        addScores.Ok = 0.05f;
        addScores.Miss = -0.1f;

        return addScores;
    }
}