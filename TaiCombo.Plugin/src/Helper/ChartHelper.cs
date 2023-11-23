using System.Drawing;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Plugin.Chart;
using TaiCombo.Plugin.Enums;

namespace TaiCombo.Plugin.Helper;

public static class ChartHelper
{
    public static void PostProcess(List<IChip> chips)
    {
        IChip? rollStartChip = null;
        for(int i = 0; i < chips.Count; i++)
        {
            IChip chip = chips[i];

            switch(chip.ChipType)
            {
                case ChipType.Don:
                chip.SENoteType = SENoteType.Do;
                break;
                case ChipType.Ka:
                chip.SENoteType = SENoteType.Ka;
                break;
                case ChipType.Don_Big:
                chip.SENoteType = SENoteType.Don_Big;
                break;
                case ChipType.Ka_Big:
                chip.SENoteType = SENoteType.Kat_Big;
                break;
                case ChipType.Roll_Start:
                chip.SENoteType = SENoteType.Roll;
                rollStartChip = chip;
                break;
                case ChipType.Roll_Big_Start:
                chip.SENoteType = SENoteType.Roll;
                rollStartChip = chip;
                break;
                case ChipType.Roll_Balloon_Start:
                chip.SENoteType = SENoteType.Balloon;
                rollStartChip = chip;
                break;
                case ChipType.Roll_End:
                chip.SENoteType = SENoteType.Roll_End;
                if (rollStartChip != null) 
                {
                    rollStartChip.RollLength = chip.Time - rollStartChip.Time;
                }
                rollStartChip = null;
                break;
                case ChipType.Roll_Kusudama_Start:
                chip.SENoteType = SENoteType.Kusudama;
                break;
                case ChipType.Roll_Fuse:
                chip.SENoteType = SENoteType.None;
                rollStartChip = chip;
                break;
                case ChipType.Roll_Don:
                chip.SENoteType = SENoteType.None;
                rollStartChip = chip;
                break;
                case ChipType.Roll_Ka:
                chip.SENoteType = SENoteType.None;
                rollStartChip = chip;
                break;
                case ChipType.Roll_Clap:
                chip.SENoteType = SENoteType.None;
                rollStartChip = chip;
                break;
            }
        }
    }
}