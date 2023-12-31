using TaiCombo.Common;
using TaiCombo.Enums;


namespace TaiCombo.Skin;

class SkinConfigValue
{
    public string Name { get; set; } = "None";
    public string Version { get; set; } = "None";
    public SizeJson Resolution { get; set; } = new() 
    {
        Width = 1920,
        Height = 1080
    };

    public string Font_Main { get; set; } = "";

    public string Font_Sub { get; set; } = "";

    public MovePosJson Combo_Shine { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 0, Y = -20 }, new PosJson() { X = 15, Y = -5 }, new PosJson() { X = -15, Y = 0 } },
        MoveX = 0,
        MoveY = -20
    };

    public PosJson Gauge_Edge { get; set; } = new()
    { 
        X = -13, 
        Y = -12 
    };

    public PaddingPosJson Gauge_Add { get; set; } = new()
    { 
        Pos = new PosJson() { X = -7, Y = -8 },
        Width = 30,
        Height = 80,
        Padding = 21
    };

    public PaddingMultiPosJson Gauge_ClearText { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 4, Y = 1 }, new PosJson() { X = 4, Y = 33 } },
        Width = 72,
        Height = 32,
        Padding = 21
    };

    public MultiPosSizeJson Gauge_SoulFire { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 1038, Y = -117 }, new PosJson() { X = 1038, Y = -112 } },
        Width = 185,
        Height = 212
    };

    public MultiPosSizeJson Gauge_SoulText { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 1051, Y = -15 }, new PosJson() { X = 1051, Y = -10 } },
        Width = 90,
        Height = 90
    };

    public Dictionary<string, PlayBGJson> Play_Background_Normal = new()
    {
        {
            "Default",
            new PlayBGJson()
            {
                Up = new string[] { "0" },
                Down = new string[] { "0" },
                Down_Clear = new string[] { "0" },
                RollEffect = new string[] { "0" },
                Footer = new string[] { "0", "1", "2" },
                Dancer = new string[] { "0" },
                Mob = new string[] { "0" },
            }
        }
    };
    public PosJson[] Play_Chara { get; set; } = new PosJson[] 
    { 
        new PosJson() { X = 0, Y = 0 }, 
        new PosJson() { X = 0, Y = 805 } 
    };

    public PosJson[] Play_GoGoSplashs { get; set; } = new PosJson[] 
    { 
        new PosJson() { X = 120, Y = 810 }, 
        new PosJson() { X = 460, Y = 810 }, 
        new PosJson() { X = 784, Y = 810 }, 
        new PosJson() { X = 1136, Y = 810 }, 
        new PosJson() { X = 1460, Y = 810 }, 
        new PosJson() { X = 1800, Y = 810 }
    };

    public PosJson[] Play_Lane_Frame { get; set; } = new PosJson[]
    {
        new PosJson()
        {
            X = 498,
            Y = 276
        },
        new PosJson()
        {
            X = 498,
            Y = 540
        }
    };

    public PosJson[] Play_Lane_Main { get; set; } = new PosJson[]
    {
        new PosJson()
        {
            X = 498,
            Y = 288
        },
        new PosJson()
        {
            X = 498,
            Y = 552
        }
    };

    public PosJson[] Play_Lane_Sub { get; set; } = new PosJson[]
    {
        new PosJson()
        {
            X = 498,
            Y = 489
        },
        new PosJson()
        {
            X = 498,
            Y = 753
        }
    };

    public MultiPosSizeJson Play_Lane_GoGoFire { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 618, Y = 385 }, new PosJson() { X = 618, Y = 650 } },
        Width = 540,
        Height = 555
    };

    public MultiPosSizeJson Play_Gauge { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 738, Y = 216 }, new PosJson() { X = 738, Y = 798 } },
        Width = 21,
        Height = 66
    };

    public MultiPosSizeJson Play_HitExplosion { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 618, Y = 386 }, new PosJson() { X = 618, Y = 650 } },
        Width = 350,
        Height = 350
    };

    public MovePosJson Play_Judge { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 620, Y = 271 }, new PosJson() { X = 620, Y = 535 } },
        MoveX = 0,
        MoveY = 18
    };

    public PaddingMultiPosJson Play_Notes { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 521, Y = 288 }, new PosJson() { X = 521, Y = 552 } },
        Width = 195,
        Height = 195,
        Padding = 1440
    };

    public PosJson Play_Notes_Balloon_Offset { get; set; } = new PosJson() { X = 172, Y = 0 };

    public MultiPosSizeJson Play_SENotes { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 521, Y = 490 }, new PosJson() { X = 521, Y = 754 } },
        Width = 195,
        Height = 40
    };

    public PosJson[] Play_Taiko_Background { get; set; } = new PosJson[]
    {
        new PosJson() { X = 0, Y = 276 },
        new PosJson() { X = 0, Y = 540 }
    };

    public PaddingMultiPosJson Play_Taiko_Options { get; set; } = new PaddingMultiPosJson()
    {
        Pos = new PosJson[]{ new PosJson() { X = 171, Y = 354 }, new PosJson() { X = 171, Y = 644 } },
        Width = 130,
        Height = 95,
        Padding = 44
    };

    public PosJson[] Play_Taiko_NamePlate { get; set; } = new PosJson[]
    {
        new PosJson() { X = -10, Y = 440 },
        new PosJson() { X = -10, Y = 550 }
    };

    public MultiPosSizeJson Play_Taiko_CourseSymbol { get; set; } = new()
    {
        Pos = new PosJson[]{ new PosJson() { X = 30, Y = 348 }, new PosJson() { X = 30, Y = 638 } },
        Width = 130,
        Height = 95
    };

    public MultiPosSizeJson Play_Taiko_Taiko { get; set; } = new()
    {
        Pos = new PosJson[]{ new PosJson() { X = 305, Y = 314 }, new PosJson() { X = 305, Y = 578 } },
        Width = 190,
        Height = 190
    };

    public PaddingMultiPosJson Play_Taiko_Combo { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 400, Y = 376 }, new PosJson() { X = 400, Y = 640 } },
        Width = 60,
        Height = 72,
        Padding = 50
    };

    public PaddingMultiPosJson Play_Taiko_Score { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 254, Y = 314 }, new PosJson() { X = 254, Y = 770 } },
        Width = 39,
        Height = 51,
        Padding = 30
    };

    public PaddingMultiPosJson Play_Taiko_AddScore { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 254, Y = 256 }, new PosJson() { X = 254, Y = 828 } },
        Width = 39,
        Height = 51,
        Padding = 30
    };

    public FlyNoteJson Play_FlyNotes { get; set; } = new()
    {
        Begin = new PosJson[] { new PosJson() { X = 521, Y = 288 }, new PosJson() { X = 521, Y = 552 } },
        End = new PosJson[] { new PosJson() { X = 1737, Y = 149 }, new PosJson() { X = 1737, Y = 737 } },
        MoveScriptX = new string[] { 
            @"
            float offset = 0.15f;

            return Begin[1].X + 
            ((End[0].X - Begin[0].X) * ((MathF.Cos(offset * MathF.PI) - MathF.Cos(((Value * (1.0f - (offset * 2.0f))) + offset) * MathF.PI)) / 2.0f) / MathF.Cos(offset * MathF.PI));", 

            @"
            float offset = 0.15f;
            
            return Begin[1].X + 
            ((End[1].X - Begin[1].X) * ((MathF.Cos(offset * MathF.PI) - MathF.Cos(((Value * (1.0f - (offset * 2.0f))) + offset) * MathF.PI)) / 2.0f) / MathF.Cos(offset * MathF.PI));" },
        MoveScriptY = new string[] { 
            @"return Begin[0].Y + ((End[0].Y - Begin[0].Y) * Value) - 
            (MathF.Sin(Value * MathF.PI) * 340.0f);",

            @"return Begin[1].Y + ((End[1].Y - Begin[1].Y) * Value) + 
            (MathF.Sin(Value * MathF.PI) * 340.0f);" },
    };

    public MultiPosSizeJson Play_BigEffect { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = -30, Y = -30 }, new PosJson() { X = -30, Y = -30 } },
        Width = 256,
        Height = 256
    };

    public FlyEndFireWorkJson Play_FlyEndEffect_Firework { get; set; } = new FlyEndFireWorkJson()
    { 
        Pos = new PosJson[] { new PosJson() { X = 1834, Y = 246 },  new PosJson() { X = 1834, Y = 834 } },
        BeginColor = new Engine.Struct.Color3[] { new Engine.Struct.Color3() { R = 1, G = 1, B = 0 },  new Engine.Struct.Color3() { R = 1, G = 1, B = 0 } },
        EndColor = new Engine.Struct.Color3[] { new Engine.Struct.Color3() { R = 1, G = 0.161f, B = 0 },  new Engine.Struct.Color3() { R = 0, G = 1, B = 0.369f } }
    };

    public MultiPosSizeJson Play_Combo_Base { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 441, Y = -21 }, new PosJson() { X = 441, Y = 819 } },
        Width = 540,
        Height = 288
    };

    public PaddingMultiPosJson Play_Combo_Number { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 616, Y = 122 }, new PosJson() { X = 616, Y = 962 } },
        Width = 80,
        Height = 95,
        Padding = 64
    };

    public PaddingMultiPosJson Play_Combo_Number_1000 { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 627, Y = 120 }, new PosJson() { X = 627, Y = 960 } },
        Width = 80,
        Height = 95,
        Padding = 64
    };

    public PosJson[] Play_Combo_Text { get; set; } = new PosJson[] 
    { 
        new PosJson() { X = 778, Y = 148 }, 
        new PosJson() { X = 778, Y = 987 } 
    };

    public PosJson[] Play_Combo_Text_1000 { get; set; } = new PosJson[] 
    { 
        new PosJson() { X = 787, Y = 148 }, 
        new PosJson() { X = 787, Y = 987 } 
    };

    public MultiPosSizeJson Play_Roll_Base { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 374, Y = -15 }, new PosJson() { X = 374, Y = 793 } },
        Width = 501,
        Height = 306
    };

    public PaddingMultiPosJson Play_Roll_Number { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 614, Y = 115 }, new PosJson() { X = 614, Y = 923 } },
        Width = 95,
        Height = 112,
        Padding = 80
    };

    public PosJson[] Play_Roll_Text { get; set; } = new PosJson[] 
    { 
        new PosJson() { X = 567, Y = 162 }, 
        new PosJson() { X = 567, Y = 970 } 
    };

    public PosJson[] Play_Balloon_Base { get; set; } = new PosJson[] 
    { 
        new PosJson() { X = 580, Y = 43 }, 
        new PosJson() { X = 580, Y = 491 } 
    };

    public PaddingMultiPosJson Play_Balloon_Number { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 730, Y = 148 }, new PosJson() { X = 730, Y = 596 } },
        Width = 80,
        Height = 95,
        Padding = 80
    };

    public PosJson[] Play_Balloon_Chara { get; set; } = new PosJson[] 
    { 
        new PosJson() { X = 521, Y = 288 }, 
        new PosJson() { X = 521, Y = 736 } 
    };

    public PosJson[] Play_Balloon_Breaking { get; set; } = new PosJson[] 
    { 
        new PosJson() { X = 636, Y = 171 }, 
        new PosJson() { X = 636, Y = 619 } 
    };

    public MovePosJson Play_Balloon_Miss { get; set; } = new MovePosJson()
    { 
        Pos = new PosJson[]  {
            new PosJson() { X = 710, Y = 384 }, 
            new PosJson() { X = 710, Y = 832 } 
        },
        MoveX = 330,
        MoveY = 84,
    };

    public MultiPosSizeJson Play_Balloon_Rainbow { get; set; } = new()
    {
        Pos = new PosJson[] { new PosJson() { X = 575, Y = -131 }, new PosJson() { X = 575, Y = 664 } },
        Width = 80,
        Height = 95
    };


    public MultiPosSizeJson Play_ScoreRank { get; set; } = new()
    {
        Pos = new PosJson[]{ new PosJson() { X = 157, Y = 138 }, new PosJson() { X = 157, Y = 942 } },
        Width = 210,
        Height = 210
    };

    public PosJson Play_GenrePlate { get; set; } = new () { X = 1590, Y = 104 };
    public TextInfoJson Play_GenrePlate_Name { get; set; } = new () 
    {
        Pos = new PosJson() { X = 144, Y = 16 },
        Size = 20
    };

    public TitleInfoJson Play_Title { get; set; } = new () 
    {
        Pos = new PosJson() { X = 1875, Y = 100 },
        Size = 55,
        MaxSize = 1000
    };

    public TextInfoJson Play_SongCount_Desc_Count { get; set; } = new () 
    {
        Pos = new PosJson() { X = 120, Y = 29 },
        Size = 20
    };

    public TextInfoJson Play_SongCount_Desc_Max { get; set; } = new () 
    {
        Pos = new PosJson() { X = 249, Y = 29 },
        Size = 20
    };

    public TextInfoJson Play_SongCount_Value_Count { get; set; } = new () 
    {
        Pos = new PosJson() { X = 74, Y = 27 },
        Size = 20
    };

    public TextInfoJson Play_SongCount_Value_Max { get; set; } = new () 
    {
        Pos = new PosJson() { X = 224, Y = 27 },
        Size = 20
    };

    public PosJson[] Result_Plate { get; set; } = new PosJson[] { new PosJson() { X = 0, Y = 0 }, new PosJson() { X = 960, Y = 0 } };
    public PosJson[] Result_Gauge { get; set; } = new PosJson[] { new PosJson() { X = 87, Y = 217 }, new PosJson() { X = 1034, Y = 217 } };
    
    public PaddingMultiPosJson[] Result_Number { get; set; } = new PaddingMultiPosJson[]
    {
        new PaddingMultiPosJson()
        {
            Pos = new PosJson[] { new PosJson() { X = 860, Y = 313 }, new PosJson() { X = 860, Y = 375 }, new PosJson() { X = 860, Y = 438 }, new PosJson() { X = 860, Y = 500 }, new PosJson() { X = 860, Y = 562 } },
            Width = 40,
            Height = 50,
            Padding = 32
        },
        new PaddingMultiPosJson()
        {
            Pos = new PosJson[] { new PosJson() { X = 1816, Y = 313 }, new PosJson() { X = 1816, Y = 375 }, new PosJson() { X = 1816, Y = 438 }, new PosJson() { X = 1816, Y = 500 }, new PosJson() { X = 1816, Y = 562 } },
            Width = 40,
            Height = 50,
            Padding = 32
        }
    };

    public PaddingMultiPosJson Result_ScoreNumber { get; set; } = new PaddingMultiPosJson()
    {
        Pos = new PosJson[] { new PosJson() { X = 424, Y = 362 }, new PosJson() { X = 1380, Y = 362 } },
        Width = 80,
        Height = 80,
        Padding = 46
    };

    public PaddingMultiPosJson Result_ScoreRank { get; set; } = new PaddingMultiPosJson()
    {
        Pos = new PosJson[] { new PosJson() { X = 203, Y = 509 }, new PosJson() { X = 1159, Y = 509 } },
        Width = 220,
        Height = 220,
        Padding = 200
    };

    public MultiPosSizeJson Result_Crown { get; set; } = new MultiPosSizeJson()
    {
        Pos = new PosJson[] { new PosJson() { X = 392, Y = 509 }, new PosJson() { X = 1348, Y = 509 } },
        Width = 220,
        Height = 220
    };

    public PosJson[] Result_NamePlate { get; set; } = new PosJson[] { new PosJson() { X = 36, Y = 925 }, new PosJson() { X = 1543, Y = 925 } };


    public HitSoundJson[] HitSounds = new HitSoundJson[]
    {
        new HitSoundJson() { 
            Name = new LangString( new Dictionary<string, string>() 
            { 
                { "default", "Taiko" }, { "ja", "太鼓" }, { "en", "Taiko" }
            }),
            DirName = "Taiko"
        }
    };
}