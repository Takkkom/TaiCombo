using TaiCombo.Enums;
using TaiCombo.Structs;
using TaiCombo.Plugin.Enums;
using Silk.NET.Maths;
using TaiCombo.Engine;
using TaiCombo.Common;
using TaiCombo.Plugin.Chart;

namespace TaiCombo.Helper;

static class NoteHelper
{
    public static Dictionary<CourseType, JudgeZones> JudgeZones = new Dictionary<CourseType, JudgeZones>
    {
        { CourseType.Easy, new JudgeZones() { Perfect = 42000, Ok = 108000, Miss = 125000 } },
        { CourseType.Normal, new JudgeZones() { Perfect = 42000, Ok = 108000, Miss = 125000 } },
        { CourseType.Hard, new JudgeZones() { Perfect = 42000, Ok = 75000, Miss = 108000 } },
        { CourseType.Oni, new JudgeZones() { Perfect = 25000, Ok = 75000, Miss = 108000 } },
        { CourseType.Edit, new JudgeZones() { Perfect = 25000, Ok = 75000, Miss = 108000 } }
    };

    private static Vector2D<float>[] RollEndPos = new Vector2D<float>[Game.MAXPLAYER];

    private static Vector2D<float>[] SERollEndPos = new Vector2D<float>[Game.MAXPLAYER];




    public static bool GetHit(ChipType chip, HitType hitType)
    {
        switch(chip)
        {
            case ChipType.Don:
            case ChipType.Don_Big:
            case ChipType.Don_Big_Joint:
            case ChipType.Roll_Don:
            case ChipType.Roll_Balloon_Start:
            case ChipType.Roll_Kusudama_Start:
            case ChipType.Roll_Fuse:
            return hitType == HitType.Don;

            case ChipType.Ka:
            case ChipType.Ka_Big:
            case ChipType.Ka_Big_Joint:
            return hitType == HitType.Ka;

            case ChipType.Roll_Start:
            case ChipType.Roll_Big_Start:
            return true;

            case ChipType.Clap:
            return hitType == HitType.Clap;
            
            default:
            return false;
        }
    }

    public static bool IsHittableNote(ChipType chip)
    {
        switch(chip)
        {
            case ChipType.Don:
            case ChipType.Don_Big:
            case ChipType.Don_Big_Joint:
            case ChipType.Ka:
            case ChipType.Ka_Big:
            case ChipType.Clap:
            return true;
            
            default:
            return false;
        }
    }

    public static JudgeType GetJudge(long time, JudgeZones judgeZones)
    {
        if (time < judgeZones.Perfect)
        {
            return JudgeType.Perfect;
        }
        else if (time < judgeZones.Ok)
        {
            return JudgeType.Ok;
        }
        else if (time < judgeZones.Miss)
        {
            return JudgeType.Miss;
        }
        else 
        {
            return JudgeType.None;
        }
    }

    private static bool OutNote(Vector2D<float> pos, float width, float height)
    {
        if (pos.X < -width || pos.X > GameEngine.Resolution[0] || pos.Y < -height || pos.Y > GameEngine.Resolution[1]) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
    
    public static void DrawNote(Vector2D<float> pos, Plugin.Enums.ChipType chipType, float width, float height, int frame, float noteAnime, int player)
    {
        int getNoteIndex(int defaultNoteIndex)
        {
            return frame == -1 ? defaultNoteIndex : frame;
        }

        float rollWidth = RollEndPos[player].X - pos.X;

        switch(chipType)
        {
            case Plugin.Enums.ChipType.Don:
            if (!OutNote(pos, width, height)) 
            {
                System.Drawing.RectangleF rectangle = new System.Drawing.RectangleF(0, getNoteIndex(0) * height, width, height);
                Game.Skin.Assets.Play_Notes_Don.Draw(pos.X, pos.Y, rectangle:rectangle);
            }
            break;
            case Plugin.Enums.ChipType.Ka:
            if (!OutNote(pos, width, height)) 
            {
                System.Drawing.RectangleF rectangle = new System.Drawing.RectangleF(0, getNoteIndex(0) * height, width, height);
                Game.Skin.Assets.Play_Notes_Ka.Draw(pos.X, pos.Y, rectangle:rectangle);
            }
            break;
            case Plugin.Enums.ChipType.Don_Big:
            if (!OutNote(pos, width, height)) 
            {
                System.Drawing.RectangleF rectangle = new System.Drawing.RectangleF(0, getNoteIndex(1) * height, width, height);
                Game.Skin.Assets.Play_Notes_Don_Big.Draw(pos.X, pos.Y, rectangle:rectangle);
            }
            break;
            case Plugin.Enums.ChipType.Ka_Big:
            if (!OutNote(pos, width, height)) 
            {
                System.Drawing.RectangleF rectangle = new System.Drawing.RectangleF(0, getNoteIndex(1) * height, width, height);
                Game.Skin.Assets.Play_Notes_Ka_Big.Draw(pos.X, pos.Y, rectangle:rectangle);
            }
            break;
            case Plugin.Enums.ChipType.Roll_Start:
            if (!OutNote(pos, rollWidth + width, height)) 
            {
                System.Drawing.RectangleF rectangle = new System.Drawing.RectangleF(0, getNoteIndex(0) * height, width, height);
                
                Game.Skin.Assets.Play_Notes_Roll.Draw(pos.X + (width / 2.0f), pos.Y, scaleX:rollWidth / width, rectangle:rectangle);
                Game.Skin.Assets.Play_Notes_Roll_Start.Draw(pos.X, pos.Y, rectangle:rectangle);
                Game.Skin.Assets.Play_Notes_Roll_End.Draw(RollEndPos[player].X, RollEndPos[player].Y, rectangle:rectangle);
            }
            break;
            case Plugin.Enums.ChipType.Roll_Big_Start:
            if (!OutNote(pos, rollWidth + width, height)) 
            {
                System.Drawing.RectangleF rectangle = new System.Drawing.RectangleF(0, getNoteIndex(1) * height, width, height);

                Game.Skin.Assets.Play_Notes_Roll_Big.Draw(pos.X + (width / 2.0f), pos.Y, scaleX:rollWidth / width, rectangle:rectangle);
                Game.Skin.Assets.Play_Notes_Roll_Big_Start.Draw(pos.X, pos.Y, rectangle:rectangle);
                Game.Skin.Assets.Play_Notes_Roll_Big_End.Draw(RollEndPos[player].X, RollEndPos[player].Y, rectangle:rectangle);
            }
            break;
            case Plugin.Enums.ChipType.Roll_Balloon_Start:
            if (!OutNote(pos, width, height)) 
            {
                float balloonScale = frame == -1 ? 1 : (1.0f - (EasingHelper.BasicInOut(Math.Min(noteAnime * 2, 1)) / 4.0f));
                Game.Skin.Assets.Play_Notes_Roll_Balloon.Draw(pos.X, pos.Y, rectangle:new System.Drawing.RectangleF(0, getNoteIndex(0) * height, width, height));
                Game.Skin.Assets.Play_Notes_Roll_Balloon.Draw(
                    pos.X + Game.Skin.Value.Play_Notes_Balloon_Offset.X, pos.Y + Game.Skin.Value.Play_Notes_Balloon_Offset.Y, 
                    rectangle:new System.Drawing.RectangleF(width, getNoteIndex(0) * height, width, height), 
                scaleX:balloonScale);
            }
            break;
            case Plugin.Enums.ChipType.Roll_End:
            RollEndPos[player] = pos;
            break;
            case Plugin.Enums.ChipType.Roll_Kusudama_Start:
            if (!OutNote(pos, width, height)) 
            {
                System.Drawing.RectangleF rectangle = new System.Drawing.RectangleF(0, getNoteIndex(0) * height, width, height);
                Game.Skin.Assets.Play_Notes_Roll_Kusudama.Draw(pos.X, pos.Y, rectangle:rectangle);
            }
            break;
        }
    }
    

    public static void DrawSENote(Vector2D<float> pos, Plugin.Enums.SENoteType senoteType, float width, float height, int player)
    {
        float rollWidth = SERollEndPos[player].X - pos.X;

        switch(senoteType)
        {
            case Plugin.Enums.SENoteType.Do:
            if (!OutNote(pos, width, height)) 
            {
                Game.Skin.Assets.Play_SENotes_Do.Draw(pos.X, pos.Y);
            }
            break;
            case Plugin.Enums.SENoteType.Don:
            if (!OutNote(pos, width, height)) 
            {
                Game.Skin.Assets.Play_SENotes_Don.Draw(pos.X, pos.Y);
            }
            break;
            case Plugin.Enums.SENoteType.Don_Big:
            if (!OutNote(pos, width, height)) 
            {
                Game.Skin.Assets.Play_SENotes_Don_Big.Draw(pos.X, pos.Y);
            }
            break;
            case Plugin.Enums.SENoteType.Ka:
            if (!OutNote(pos, width, height)) 
            {
                Game.Skin.Assets.Play_SENotes_Ka.Draw(pos.X, pos.Y);
            }
            break;
            case Plugin.Enums.SENoteType.Kat:
            if (!OutNote(pos, width, height)) 
            {
                Game.Skin.Assets.Play_SENotes_Kat.Draw(pos.X, pos.Y);
            }
            break;
            case Plugin.Enums.SENoteType.Kat_Big:
            if (!OutNote(pos, width, height)) 
            {
                Game.Skin.Assets.Play_SENotes_Kat_Big.Draw(pos.X, pos.Y);
            }
            break;
            case Plugin.Enums.SENoteType.Roll:
            if (!OutNote(pos, rollWidth + width, height)) 
            {
                Game.Skin.Assets.Play_SENotes_Roll.Draw(pos.X + (width / 2.0f), pos.Y, scaleX:rollWidth / width);

                Game.Skin.Assets.Play_SENotes_Roll_Start.Draw(pos.X, pos.Y);
                Game.Skin.Assets.Play_SENotes_Roll_End.Draw(SERollEndPos[player].X, SERollEndPos[player].Y);
            }
            break;
            case Plugin.Enums.SENoteType.Balloon:
            if (!OutNote(pos, width, height)) 
            {
                Game.Skin.Assets.Play_SENotes_Balloon.Draw(pos.X, pos.Y);
            }
            break;
            case Plugin.Enums.SENoteType.Roll_End:
            SERollEndPos[player] = pos;
            break;
            case Plugin.Enums.SENoteType.Kusudama:
            if (!OutNote(pos, width, height)) 
            {
                Game.Skin.Assets.Play_Notes_Roll_Kusudama.Draw(pos.X, pos.Y);
            }
            break;
        }
    }

    public static ChipType FlipChipType(ChipType chipType)
    {
        switch(chipType)
        {
            case ChipType.Don:
            return ChipType.Ka;;
            case ChipType.Ka:
            return ChipType.Don;
            case ChipType.Don_Big:
            return ChipType.Ka_Big;;
            case ChipType.Ka_Big:
            return ChipType.Don_Big;;
            case ChipType.Don_Big_Joint:
            return ChipType.Ka_Big_Joint;
            case ChipType.Ka_Big_Joint:
            return ChipType.Don_Big_Joint;
            case ChipType.Roll_Don:
            return ChipType.Roll_Ka;
            case ChipType.Roll_Ka:
            return ChipType.Roll_Don;
            default:
            return chipType;
        }
    }

    public static SENoteType FlipSENoteType(SENoteType seNoteType)
    {
        switch(seNoteType)
        {
            case SENoteType.Do:
            return SENoteType.Ka;
            case SENoteType.Don:
            return SENoteType.Kat;
            case SENoteType.Don_Big:
            return SENoteType.Kat_Big;
            case SENoteType.Ka:
            return SENoteType.Do;
            case SENoteType.Kat:
            return SENoteType.Don;
            case SENoteType.Kat_Big:
            return SENoteType.Don_Big;
            default:
            return seNoteType;
        }
    }

    public static void BranchChips(List<IChip> chips, int branchChipIndex, BranchType branch)
    {
        for(int i = branchChipIndex; i < chips.Count; i++)
        {
            IChip chip = chips[i];
            if (chip.ChipType == ChipType.BranchStart || chip.ChipType == ChipType.BranchEnd) 
            {
                chip.Active = true;
                continue;
            }

            if (chip.BranchType == branch)
            {
                chip.Active = true;
            }
            else 
            {
                chip.Active = false;
            }
        }
    }
}