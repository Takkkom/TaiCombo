using System.Drawing;
using TaiCombo.Plugin.Chart;
using TaiCombo.Common;
using TaiCombo.Structs;
using TaiCombo.Enums;
using TaiCombo.Plugin.Enums;

namespace TaiCombo.Helper;

static class ScoreHelper
{
    public static AddScores GetGen4Scores(List<IChip> chips)
    {
        int noteCount = 0;
        int noteBigCount = 0;
        int rollScore = 0;
        foreach(IChip chip in chips)
        {
            switch(chip.ChipType)
            {
                case ChipType.Don:
                case ChipType.Ka:
                case ChipType.Purple:
                case ChipType.Clap:
                noteCount++;
                break;
                case ChipType.Don_Big:
                case ChipType.Ka_Big:
                case ChipType.Don_Big_Joint:
                case ChipType.Ka_Big_Joint:
                noteBigCount++;
                break;
                case ChipType.Roll_Start:
                case ChipType.Roll_Big_Start:
                case ChipType.Roll_Don:
                case ChipType.Roll_Ka:
                case ChipType.Roll_Clap:
                rollScore += (int)MathF.Ceiling(chip.RollLength / 500.0f);
                break;
                case ChipType.Roll_Balloon_Start:
                case ChipType.Roll_Kusudama_Start:
                //rollScore += chip.BalloonCount * 100; //これいらなーい
                break;
            }
        }
        
        AddScores addScores = new();

        int totalCount = noteCount + noteBigCount;
        float maxScore = (1000000.0f - rollScore) / 10.0f;
        int add = (int)(MathF.Ceiling(maxScore / totalCount) * 10);

        addScores.Perfect = add;
        addScores.Ok = (int)MathF.Ceiling(add / 20.0f) * 10;
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
            return GetGen4Scores(chips);
            default:
            throw new Exception("無効なScoreTypeです");
        }
    }

    public static int GetValue(int value, float gogorate)
    {
        return (int)(value * gogorate);
    }
}