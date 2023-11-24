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

    public static AddGauges GetNormalGauge(CourseType courseType, int level, int noteCount)
    {
        AddGauges addScores = new();

        float baseRate = (int)(10000 / noteCount) / 100.0f;

        switch(courseType)
        {
            case CourseType.Easy:
            {
                float perfectRate;
                if (level <= 1)
                {
                    perfectRate = baseRate / 0.60f;
                }
                else if (level >= 2 && level <= 3)
                {
                    perfectRate = baseRate / 0.63333f;
                }
                else
                {
                    perfectRate = baseRate / 0.73333f;
                }

                addScores.Perfect = perfectRate;
                addScores.Ok = perfectRate * (3.0f / 4.0f);
                addScores.Miss = perfectRate * (-1.0f / 2.0f);
            }
            break;
            case CourseType.Normal:
            {
                float perfectRate;
                if (level <= 2)
                {
                    perfectRate = baseRate / 0.65664f;
                    addScores.Miss = perfectRate * (-1.0f / 2.0f);
                }
                else if (level == 3)
                {
                    perfectRate = baseRate / 0.69565f;
                    addScores.Miss = perfectRate * (-1.0f / 2.0f);
                }
                else if (level == 4)
                {
                    perfectRate = baseRate / 0.70358f;
                    addScores.Miss = perfectRate * (-3.0f / 4.0f);
                }
                else
                {
                    perfectRate = baseRate / 0.75f;
                    addScores.Miss = perfectRate * -1;
                }

                addScores.Perfect = perfectRate;
                addScores.Ok = perfectRate * (3.0f / 4.0f);
            }
            break;
            case CourseType.Hard:
            {
                float perfectRate;
                if (level <= 2)
                {
                    perfectRate = baseRate / 0.77919f;
                    addScores.Miss = perfectRate * (-3.0f / 4.0f);
                }
                else if (level == 3)
                {
                    perfectRate = baseRate / 0.72572f;
                    addScores.Miss = perfectRate * -1;
                }
                else if (level == 4)
                {
                    perfectRate = baseRate / 0.69214f;
                    addScores.Miss = perfectRate * (-7.0f / 6.0f);
                }
                else if (level == 5)
                {
                    perfectRate = baseRate / 0.67512f;
                    addScores.Miss = perfectRate * (-5.0f / 4.0f);
                }
                else 
                {
                    perfectRate = baseRate / 0.68757f;
                    addScores.Miss = perfectRate * (-5.0f / 4.0f);
                }

                addScores.Perfect = perfectRate;
                addScores.Ok = perfectRate * (3.0f / 4.0f);
            }
            break;
            case CourseType.Oni:
            case CourseType.Edit:
            {
                float perfectRate;
                if (level <= 7)
                {
                    perfectRate = baseRate / 0.70772f;
                    addScores.Ok = perfectRate * (1.0f / 2.0f); 
                    addScores.Miss = perfectRate * (-8.0f / 5.0f);
                }
                else if (level == 8)
                {
                    perfectRate = baseRate / 0.70f;
                    addScores.Ok = perfectRate * (1.0f / 2.0f); 
                    addScores.Miss = perfectRate * -2;
                }
                else 
                {
                    perfectRate = baseRate / 0.79239f;
                    addScores.Ok = perfectRate * (1.0f / 2.0f); 
                    addScores.Miss = perfectRate * -2;
                }

                addScores.Perfect = perfectRate;
            }
            break;
        }
        return addScores;
    }
    
    public static AddGauges GetAddGauge(List<IChip> chips, CourseType courseType, GaugeType gaugeType, int level)
    {
        int noteCount = 0;
        foreach(IChip chip in chips)
        {
            switch(chip.ChipType)
            {
                case ChipType.Don:
                case ChipType.Ka:
                case ChipType.Don_Big:
                case ChipType.Ka_Big:
                case ChipType.Don_Big_Joint:
                case ChipType.Ka_Big_Joint:
                case ChipType.Purple:
                case ChipType.Clap:
                noteCount++;
                break;
            }
        }

        switch(gaugeType)
        {
            case GaugeType.Level0:
            case GaugeType.Level1:
            case GaugeType.Level2:
            return GetNormalGauge(courseType, level, noteCount);
            default:
            {
                AddGauges addGauges = new()
                {
                    Perfect = 0,
                    Ok = 0,
                    Miss = 0,
                };
                return addGauges;
            }
        }
    }
}