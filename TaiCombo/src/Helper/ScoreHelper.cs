using System.Drawing;
using TaiCombo.Plugin.Chart;
using TaiCombo.Common;
using TaiCombo.Structs;
using TaiCombo.Enums;

namespace TaiCombo.Helper;

static class ScoreHelper
{
    public static AddScores GetAddScores(List<IChip> chips, ScoreType scoreType)
    {
        switch(scoreType)
        {
            case ScoreType.Gen3:
            {
                AddScores addScores = new();


                return addScores;
            }
            case ScoreType.Gen3_True:
            {
                AddScores addScores = new();


                return addScores;
            }
            case ScoreType.Gen4:
            {
                AddScores addScores = new();

                addScores.Perfect = 100;
                addScores.Ok = 100;
                addScores.Miss = 0;

                addScores.Roll = 100;
                addScores.Roll_Big = 100;

                addScores.Balloon_Roll = 100;
                addScores.Balloon_Broke = 100;
                addScores.Kusudama_Roll = 100;
                addScores.Kusudama_Broke = 100;

                addScores.GoGoRate = 1.0f;

                return addScores;
            }
            default:
            throw new Exception("無効なScoreTypeです");
        }
    }

    public static int GetValue(int value, float gogorate)
    {
        return (int)(value * gogorate);
    }
}