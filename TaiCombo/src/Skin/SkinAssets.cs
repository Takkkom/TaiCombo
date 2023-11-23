using System.Text.Json;
using System.Text.Json.Serialization;
using Silk.NET.Windowing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Enums;


namespace TaiCombo.Skin;

/// <summary>
/// 事前に読み込む画像やはこちら
/// </summary>
class SkinAssets : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    private List<IDisposable> DisposableAssets = new();

    public Sprite NamePlate_Base { get; private set; }
    public Sprite NamePlate_Left { get; private set; }
    public Sprite NamePlate_Right { get; private set; }
    public Sprite NamePlate_1P { get; private set; }
    public Sprite NamePlate_2P { get; private set; }

    public Sprite Options_None { get; private set; }
    public Dictionary<float, Sprite> Options_ScrollSpeed { get; private set; } = new();
    public Dictionary<InvisibleType, Sprite> Options_Invisible { get; private set; } = new();
    public Sprite Options_Flip { get; private set; }
    public Dictionary<RandomType, Sprite> Options_Random { get; private set; } = new();
    public Sprite Options_Offset { get; private set; }
    
    public Dictionary<GaugeType, Sprite> Gauge_1P_Base { get; private set; } = new();
    public Dictionary<GaugeType, Sprite> Gauge_2P_Base { get; private set; } = new();
    public Dictionary<GaugeType, Sprite> Gauge_1P_Clear { get; private set; } = new();
    public Dictionary<GaugeType, Sprite> Gauge_2P_Up_Clear { get; private set; } = new();
    public Dictionary<GaugeType, Sprite> Gauge_2P_Down_Clear { get; private set; } = new();
    public Dictionary<GaugeType, Sprite> Gauge_1P_Flash { get; private set; } = new();
    public Dictionary<GaugeType, Sprite> Gauge_2P_Up_Flash { get; private set; } = new();
    public Dictionary<GaugeType, Sprite> Gauge_2P_Down_Flash { get; private set; } = new();
    public Dictionary<GaugeType, Sprite> Gauge_Frame { get; private set; } = new();
    public Dictionary<GaugeType, Sprite> Gauge_Edge { get; private set; } = new();
    public Dictionary<GaugeType, Sprite[]> Gauge_Rainbow { get; private set; } = new();
    public Sprite Gauge_ClearText { get; private set; }
    public Sprite[] Gauge_Add { get; private set; } = new Sprite[Game.MAXPLAYER];
    public Sprite Gauge_SoulText { get; private set; }
    public Sprite Gauge_SoulFire { get; private set; }

    public Sprite Play_Lane_Base_Main { get; private set; }
    public Sprite Play_Lane_Base_Normal { get; private set; }
    public Sprite Play_Lane_Base_Expert { get; private set; }
    public Sprite Play_Lane_Base_Master { get; private set; }
    public Sprite Play_Lane_Base_Sub { get; private set; }
    public Sprite Play_Lane_Text_Normal { get; private set; }
    public Sprite Play_Lane_Text_Expert { get; private set; }
    public Sprite Play_Lane_Text_Master { get; private set; }
    public Sprite Play_Lane_Flash_Red { get; private set; }
    public Sprite Play_Lane_Flash_Blue { get; private set; }
    public Sprite Play_Lane_Flash_Yellow { get; private set; }
    public Sprite Play_Lane_GoGo { get; private set; }
    public Sprite Play_Lane_Frame { get; private set; }
    public Sprite Play_Lane_GoGoFire { get; private set; }

    public Sprite Play_Notes_Target { get; private set; }
    public Sprite Play_Notes_Don { get; private set; }
    public Sprite Play_Notes_Ka { get; private set; }
    public Sprite Play_Notes_Don_Big { get; private set; }
    public Sprite Play_Notes_Ka_Big { get; private set; }
    public Sprite Play_Notes_Roll_Start { get; private set; }
    public Sprite Play_Notes_Roll { get; private set; }
    public Sprite Play_Notes_Roll_End { get; private set; }
    public Sprite Play_Notes_Roll_Big_Start { get; private set; }
    public Sprite Play_Notes_Roll_Big { get; private set; }
    public Sprite Play_Notes_Roll_Big_End { get; private set; }
    public Sprite Play_Notes_Roll_Balloon { get; private set; }
    public Sprite Play_Notes_Roll_Kusudama { get; private set; }
    public Sprite Play_Notes_Line { get; private set; }
    public Sprite Play_Notes_Line_Branched { get; private set; }

    public Sprite Play_SENotes_Do { get; private set; }
    public Sprite Play_SENotes_Don { get; private set; }
    public Sprite Play_SENotes_Don_Big { get; private set; }
    public Sprite Play_SENotes_Ka { get; private set; }
    public Sprite Play_SENotes_Kat { get; private set; }
    public Sprite Play_SENotes_Kat_Big { get; private set; }
    public Sprite Play_SENotes_Roll_Start { get; private set; }
    public Sprite Play_SENotes_Roll { get; private set; }
    public Sprite Play_SENotes_Roll_End { get; private set; }
    public Sprite Play_SENotes_Balloon { get; private set; }

    public Sprite Play_HitExplosion_Firework { get; private set; }
    public Sprite Play_HitExplosion_Firework_Big { get; private set; }
    public Sprite Play_HitExplosion_Notes { get; private set; }

    public Sprite Play_Judge_Perfect { get; private set; }
    public Sprite Play_Judge_Ok { get; private set; }
    public Sprite Play_Judge_Miss { get; private set; }

    public Sprite[] Play_Taiko_Background { get; private set; } = new Sprite[Game.MAXPLAYER];
    public Sprite Play_Taiko_Background_Frame { get; private set; }
    public Sprite Play_Taiko_Taiko { get; private set; }
    public Sprite Play_Taiko_Combo { get; private set; }
    public Sprite Play_Taiko_Combo_Medium { get; private set; }
    public Sprite Play_Taiko_Combo_Big { get; private set; }
    public Sprite Play_Taiko_Combo_Text { get; private set; }
    public Sprite Play_Taiko_Score_Base { get; private set; }
    public Sprite[] Play_Taiko_Score { get; private set; } = new Sprite[Game.MAXPLAYER];
    public Sprite Play_Taiko_CourseSymbol { get; private set; }
    public Sprite Play_Taiko_Shine { get; private set; }

    public Sprite Play_FlyNotes_Don { get; private set; }
    public Sprite Play_FlyNotes_Ka { get; private set; }
    public Sprite Play_FlyNotes_Don_Big { get; private set; }
    public Sprite Play_FlyNotes_Ka_Big { get; private set; }
    public Sprite Play_FlyNotes_Flash { get; private set; }
    public Sprite Play_FlyNotes_Flash_Big { get; private set; }
    public Sprite Play_FlyNotes_Flash_Yellow { get; private set; }
    public Sprite Play_FlyNotes_Flash_Yellow_Big { get; private set; }
    public Sprite Play_FlyNotes_Firework_Alt { get; private set; }
    public Sprite Play_FlyNotes_Firework { get; private set; }

    public Sprite Play_BigEffect { get; private set; }

    public Sprite[] Play_Combo_Base { get; private set; } = new Sprite[Game.MAXPLAYER];
    public Sprite[] Play_Combo_Number { get; private set; } = new Sprite[Game.MAXPLAYER];
    public Sprite Play_Combo_Text { get; private set; }

    public Sprite Play_Roll_Base { get; private set; }
    public Sprite Play_Roll_Number { get; private set; }

    public Sprite Play_Balloon_Base { get; private set; }
    public Sprite Play_Balloon_Number { get; private set; }
    public Sprite[] Play_Balloon_Breaking { get; private set; } = new Sprite[6];
    public Sprite Play_Balloon_Miss { get; private set; }
    public Sprite Play_Balloon_Rainbow { get; private set; }

    public Sprite Play_ScoreRank { get; private set; }

    public Sound Decide { get; private set; }
    public Sound Change { get; private set; }
    public List<HitSound> HitSounds { get; private set; } = new();

    /// <summary>
    /// 2DSpriteの作成と同時にDisposableAssetsに登録します
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private Sprite CreateSprite(string path)
    {
        Sprite sprite = new(path);
        DisposableAssets.Add(sprite);
        return sprite;
    }

    /// <summary>
    /// 2DSpriteの作成と同時にDisposableAssetsに登録します
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private Sprite[] CreateSpriteArray(string path)
    {
        string[] files = Directory.GetFiles(path, "*.png");
        Sprite[] result = new Sprite[files.Length];
        for(int i = 0; i < files.Length; i++)
        {
            Sprite sprite = new(files[i]);
            result[i] = sprite;
            DisposableAssets.Add(sprite);
        }
        return result;
    }

    /// <summary>
    /// Soundの作成と同時にDisposableAssetsに登録します
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private Sound CreateSound(string path)
    {
        Sound sound = new(path);
        DisposableAssets.Add(sound);
        return sound;
    }

    /// <summary>
    /// Soundの作成と同時にDisposableAssetsに登録します
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private HitSound CreateHitSound(HitSoundJson hitSoundJson)
    {
        HitSound sound = new(hitSoundJson);
        DisposableAssets.Add(sound);
        return sound;
    }


    public SkinAssets()
    {
        string nameplatePath = $"{Game.Skin.GraphicsPath}NamePlate{Path.DirectorySeparatorChar}";
        NamePlate_Base = CreateSprite($"{nameplatePath}Base.png");
        NamePlate_Left = CreateSprite($"{nameplatePath}Left.png");
        NamePlate_Right = CreateSprite($"{nameplatePath}Right.png");
        NamePlate_1P = CreateSprite($"{nameplatePath}1P.png");
        NamePlate_2P = CreateSprite($"{nameplatePath}2P.png");

        string optionsPath = $"{Game.Skin.GraphicsPath}Options{Path.DirectorySeparatorChar}";
        
        Options_None = CreateSprite($"{optionsPath}None.png");

        string[] scrollFiles = Directory.GetFiles($"{optionsPath}", "Scroll_*.png");
        for(int i = 0; i < scrollFiles.Length; i++)
        {
            float value = float.Parse(Path.GetFileNameWithoutExtension(scrollFiles[i]).Remove(0, 7));
            Options_ScrollSpeed.Add(value, CreateSprite(scrollFiles[i]));
        }
        Options_Invisible.Add(InvisibleType.None, Options_None);
        Options_Invisible.Add(InvisibleType.SEOnly, CreateSprite($"{optionsPath}Invisible_SEOnly.png"));
        Options_Invisible.Add(InvisibleType.Full, CreateSprite($"{optionsPath}Invisible_Full.png"));
        Options_Flip = CreateSprite($"{optionsPath}Flip.png");
        Options_Random.Add(RandomType.None, Options_None);
        Options_Random.Add(RandomType.Minor, CreateSprite($"{optionsPath}Random_Minor.png"));
        Options_Random.Add(RandomType.Random, CreateSprite($"{optionsPath}Random_Random.png"));
        Options_Offset = CreateSprite($"{optionsPath}Offset.png");

        string gaugePath = $"{Game.Skin.GraphicsPath}Gauge{Path.DirectorySeparatorChar}";
        Gauge_1P_Base.Add(GaugeType.Level0, CreateSprite($"{gaugePath}1P_0_Base.png"));
        Gauge_1P_Base.Add(GaugeType.Level1, CreateSprite($"{gaugePath}1P_1_Base.png"));
        Gauge_1P_Base.Add(GaugeType.Level2, CreateSprite($"{gaugePath}1P_2_Base.png"));
        Gauge_1P_Base.Add(GaugeType.Hard, CreateSprite($"{gaugePath}1P_Hard_Base.png"));
        Gauge_1P_Base.Add(GaugeType.Extreme, CreateSprite($"{gaugePath}1P_Extreme_Base.png"));
        Gauge_1P_Base.Add(GaugeType.ExHard, CreateSprite($"{gaugePath}1P_ExHard_Base.png"));
        Gauge_1P_Base.Add(GaugeType.Dan, CreateSprite($"{gaugePath}1P_Dan_Base.png"));

        Gauge_2P_Base.Add(GaugeType.Level0, CreateSprite($"{gaugePath}2P_0_Base.png"));
        Gauge_2P_Base.Add(GaugeType.Level1, CreateSprite($"{gaugePath}2P_1_Base.png"));
        Gauge_2P_Base.Add(GaugeType.Level2, CreateSprite($"{gaugePath}2P_2_Base.png"));
        Gauge_2P_Base.Add(GaugeType.Hard, CreateSprite($"{gaugePath}2P_Hard_Base.png"));
        Gauge_2P_Base.Add(GaugeType.Extreme, CreateSprite($"{gaugePath}2P_Extreme_Base.png"));
        Gauge_2P_Base.Add(GaugeType.ExHard, CreateSprite($"{gaugePath}2P_ExHard_Base.png"));
        Gauge_2P_Base.Add(GaugeType.Dan, CreateSprite($"{gaugePath}2P_Dan_Base.png"));
        
        Gauge_1P_Clear.Add(GaugeType.Level0, CreateSprite($"{gaugePath}1P_0_Clear.png"));
        Gauge_1P_Clear.Add(GaugeType.Level1, CreateSprite($"{gaugePath}1P_1_Clear.png"));
        Gauge_1P_Clear.Add(GaugeType.Level2, CreateSprite($"{gaugePath}1P_2_Clear.png"));
        Gauge_1P_Clear.Add(GaugeType.Hard, CreateSprite($"{gaugePath}1P_Hard_Clear.png"));
        Gauge_1P_Clear.Add(GaugeType.Extreme, CreateSprite($"{gaugePath}1P_Extreme_Clear.png"));
        Gauge_1P_Clear.Add(GaugeType.ExHard, CreateSprite($"{gaugePath}1P_ExHard_Clear.png"));
        Gauge_1P_Clear.Add(GaugeType.Dan, CreateSprite($"{gaugePath}1P_Dan_Clear.png"));

        Gauge_2P_Up_Clear.Add(GaugeType.Level0, CreateSprite($"{gaugePath}2P_Up_0_Clear.png"));
        Gauge_2P_Up_Clear.Add(GaugeType.Level1, CreateSprite($"{gaugePath}2P_Up_1_Clear.png"));
        Gauge_2P_Up_Clear.Add(GaugeType.Level2, CreateSprite($"{gaugePath}2P_Up_2_Clear.png"));
        Gauge_2P_Up_Clear.Add(GaugeType.Hard, CreateSprite($"{gaugePath}2P_Up_Hard_Clear.png"));
        Gauge_2P_Up_Clear.Add(GaugeType.Extreme, CreateSprite($"{gaugePath}2P_Up_Extreme_Clear.png"));
        Gauge_2P_Up_Clear.Add(GaugeType.ExHard, CreateSprite($"{gaugePath}2P_Up_ExHard_Clear.png"));
        Gauge_2P_Up_Clear.Add(GaugeType.Dan, CreateSprite($"{gaugePath}2P_Up_Dan_Clear.png"));

        Gauge_2P_Down_Clear.Add(GaugeType.Level0, CreateSprite($"{gaugePath}2P_Down_0_Clear.png"));
        Gauge_2P_Down_Clear.Add(GaugeType.Level1, CreateSprite($"{gaugePath}2P_Down_1_Clear.png"));
        Gauge_2P_Down_Clear.Add(GaugeType.Level2, CreateSprite($"{gaugePath}2P_Down_2_Clear.png"));
        Gauge_2P_Down_Clear.Add(GaugeType.Hard, CreateSprite($"{gaugePath}2P_Down_Hard_Clear.png"));
        Gauge_2P_Down_Clear.Add(GaugeType.Extreme, CreateSprite($"{gaugePath}2P_Down_Extreme_Clear.png"));
        Gauge_2P_Down_Clear.Add(GaugeType.ExHard, CreateSprite($"{gaugePath}2P_Down_ExHard_Clear.png"));
        Gauge_2P_Down_Clear.Add(GaugeType.Dan, CreateSprite($"{gaugePath}2P_Down_Dan_Clear.png"));
        
        Gauge_1P_Flash.Add(GaugeType.Level0, CreateSprite($"{gaugePath}1P_0_Flash.png"));
        Gauge_1P_Flash.Add(GaugeType.Level1, CreateSprite($"{gaugePath}1P_1_Flash.png"));
        Gauge_1P_Flash.Add(GaugeType.Level2, CreateSprite($"{gaugePath}1P_2_Flash.png"));

        Gauge_2P_Up_Flash.Add(GaugeType.Level0, CreateSprite($"{gaugePath}2P_Up_0_Flash.png"));
        Gauge_2P_Up_Flash.Add(GaugeType.Level1, CreateSprite($"{gaugePath}2P_Up_1_Flash.png"));
        Gauge_2P_Up_Flash.Add(GaugeType.Level2, CreateSprite($"{gaugePath}2P_Up_2_Flash.png"));
        Gauge_2P_Up_Flash.Add(GaugeType.Hard, CreateSprite($"{gaugePath}2P_Up_Hard_Flash.png"));
        Gauge_2P_Up_Flash.Add(GaugeType.Extreme, CreateSprite($"{gaugePath}2P_Up_Extreme_Flash.png"));
        Gauge_2P_Up_Flash.Add(GaugeType.ExHard, CreateSprite($"{gaugePath}2P_Up_ExHard_Flash.png"));
        Gauge_2P_Up_Flash.Add(GaugeType.Dan, CreateSprite($"{gaugePath}2P_Up_Dan_Flash.png"));

        Gauge_2P_Down_Flash.Add(GaugeType.Level0, CreateSprite($"{gaugePath}2P_Down_0_Flash.png"));
        Gauge_2P_Down_Flash.Add(GaugeType.Level1, CreateSprite($"{gaugePath}2P_Down_1_Flash.png"));
        Gauge_2P_Down_Flash.Add(GaugeType.Level2, CreateSprite($"{gaugePath}2P_Down_2_Flash.png"));
        Gauge_2P_Down_Flash.Add(GaugeType.Hard, CreateSprite($"{gaugePath}2P_Down_Hard_Flash.png"));
        Gauge_2P_Down_Flash.Add(GaugeType.Extreme, CreateSprite($"{gaugePath}2P_Down_Extreme_Flash.png"));
        Gauge_2P_Down_Flash.Add(GaugeType.ExHard, CreateSprite($"{gaugePath}2P_Down_ExHard_Flash.png"));
        Gauge_2P_Down_Flash.Add(GaugeType.Dan, CreateSprite($"{gaugePath}2P_Down_Dan_Flash.png"));

        Gauge_Frame.Add(GaugeType.Level0, CreateSprite($"{gaugePath}Frame_0.png"));
        Gauge_Frame.Add(GaugeType.Level1, CreateSprite($"{gaugePath}Frame_1.png"));
        Gauge_Frame.Add(GaugeType.Level2, CreateSprite($"{gaugePath}Frame_2.png"));
        Gauge_Frame.Add(GaugeType.Hard, CreateSprite($"{gaugePath}Frame_Hard.png"));
        Gauge_Frame.Add(GaugeType.Extreme, CreateSprite($"{gaugePath}Frame_Extreme.png"));
        Gauge_Frame.Add(GaugeType.ExHard, CreateSprite($"{gaugePath}Frame_ExHard.png"));
        Gauge_Frame.Add(GaugeType.Dan, CreateSprite($"{gaugePath}Frame_Dan.png"));

        Gauge_Edge.Add(GaugeType.Level0, CreateSprite($"{gaugePath}Edge_0.png"));
        Gauge_Edge.Add(GaugeType.Level1, CreateSprite($"{gaugePath}Edge_1.png"));
        Gauge_Edge.Add(GaugeType.Level2, CreateSprite($"{gaugePath}Edge_2.png"));
        Gauge_Edge.Add(GaugeType.Hard, CreateSprite($"{gaugePath}Edge_Hard.png"));
        Gauge_Edge.Add(GaugeType.Extreme, CreateSprite($"{gaugePath}Edge_Extreme.png"));
        Gauge_Edge.Add(GaugeType.ExHard, CreateSprite($"{gaugePath}Edge_ExHard.png"));
        Gauge_Edge.Add(GaugeType.Dan, CreateSprite($"{gaugePath}Edge_Dan.png"));

        Gauge_Rainbow.Add(GaugeType.Level0, CreateSpriteArray($"{gaugePath}Rainbow_Level0{Path.DirectorySeparatorChar}"));
        Gauge_Rainbow.Add(GaugeType.Level1, CreateSpriteArray($"{gaugePath}Rainbow_Level1{Path.DirectorySeparatorChar}"));
        Gauge_Rainbow.Add(GaugeType.Level2, CreateSpriteArray($"{gaugePath}Rainbow_Level2{Path.DirectorySeparatorChar}"));
        Gauge_Rainbow.Add(GaugeType.Hard, CreateSpriteArray($"{gaugePath}Rainbow_Hard{Path.DirectorySeparatorChar}"));
        Gauge_Rainbow.Add(GaugeType.Extreme, CreateSpriteArray($"{gaugePath}Rainbow_Extreme{Path.DirectorySeparatorChar}"));
        Gauge_Rainbow.Add(GaugeType.ExHard, CreateSpriteArray($"{gaugePath}Rainbow_ExHard{Path.DirectorySeparatorChar}"));
        Gauge_Rainbow.Add(GaugeType.Dan, CreateSpriteArray($"{gaugePath}Rainbow_Dan{Path.DirectorySeparatorChar}"));

        Gauge_ClearText = CreateSprite($"{gaugePath}ClearText.png");

        Gauge_Add[0] = CreateSprite($"{gaugePath}1P_Add.png");
        Gauge_Add[1] = CreateSprite($"{gaugePath}2P_Add.png");

        Gauge_SoulText = CreateSprite($"{gaugePath}SoulText.png");
        Gauge_SoulFire = CreateSprite($"{gaugePath}SoulFire.png");

        string playPath = $"{Game.Skin.GraphicsPath}Play{Path.DirectorySeparatorChar}";

        Play_Lane_Base_Main = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Base_Main.png");
        Play_Lane_Base_Normal = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Base_Normal.png");
        Play_Lane_Base_Expert = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Base_Expert.png");
        Play_Lane_Base_Master = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Base_Master.png");
        Play_Lane_Base_Sub = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Base_Sub.png");
        Play_Lane_Text_Normal = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Text_Normal.png");
        Play_Lane_Text_Expert = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Text_Expert.png");
        Play_Lane_Text_Master = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Text_Master.png");
        Play_Lane_Flash_Red = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Flash_Red.png");
        Play_Lane_Flash_Blue = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Flash_Blue.png");
        Play_Lane_Flash_Yellow = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Flash_Yellow.png");
        Play_Lane_GoGo = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}GoGo.png");
        Play_Lane_Frame = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}Frame.png");
        Play_Lane_GoGoFire = CreateSprite($"{playPath}Lane{Path.DirectorySeparatorChar}GoGoFire.png");

        Play_Notes_Target = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Target.png");
        Play_Notes_Don = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Don.png");
        Play_Notes_Ka = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Ka.png");
        Play_Notes_Don_Big = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Don_Big.png");
        Play_Notes_Ka_Big = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Ka_Big.png");
        Play_Notes_Roll_Start = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Roll_Start.png");
        Play_Notes_Roll = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Roll.png");
        Play_Notes_Roll_End = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Roll_End.png");
        Play_Notes_Roll_Big_Start = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Roll_Big_Start.png");
        Play_Notes_Roll_Big = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Roll_Big.png");
        Play_Notes_Roll_Big_End = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Roll_Big_End.png");
        Play_Notes_Roll_Balloon = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Roll_Balloon.png");
        Play_Notes_Roll_Kusudama = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Roll_Kusudama.png");
        Play_Notes_Line = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Line.png");
        Play_Notes_Line_Branched = CreateSprite($"{playPath}Notes{Path.DirectorySeparatorChar}Line_Branch.png");

        Play_SENotes_Do = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Do.png");
        Play_SENotes_Don = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Don.png");
        Play_SENotes_Don_Big = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Don_Big.png");
        Play_SENotes_Ka = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Ka.png");
        Play_SENotes_Kat = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Kat.png");
        Play_SENotes_Kat_Big = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Kat_Big.png");
        Play_SENotes_Roll_Start = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Roll_Start.png");
        Play_SENotes_Roll = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Roll.png");
        Play_SENotes_Roll_End = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Roll_End.png");
        Play_SENotes_Balloon = CreateSprite($"{playPath}SENotes{Path.DirectorySeparatorChar}Balloon.png");

        Play_HitExplosion_Firework = CreateSprite($"{playPath}HitExplosion{Path.DirectorySeparatorChar}Firework.png");
        Play_HitExplosion_Firework_Big = CreateSprite($"{playPath}HitExplosion{Path.DirectorySeparatorChar}Firework_Big.png");
        Play_HitExplosion_Notes = CreateSprite($"{playPath}HitExplosion{Path.DirectorySeparatorChar}Notes.png");

        Play_Judge_Perfect = CreateSprite($"{playPath}Judge{Path.DirectorySeparatorChar}Perfect.png");
        Play_Judge_Ok = CreateSprite($"{playPath}Judge{Path.DirectorySeparatorChar}Ok.png");
        Play_Judge_Miss = CreateSprite($"{playPath}Judge{Path.DirectorySeparatorChar}Miss.png");
        
        Play_Taiko_Background[0] = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Background_1P.png");
        Play_Taiko_Background[1] = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Background_2P.png");
        Play_Taiko_Background_Frame = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Background_Frame.png");
        Play_Taiko_Taiko = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Taiko.png");
        Play_Taiko_Combo = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Combo.png");
        Play_Taiko_Combo_Medium = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Combo_Medium.png");
        Play_Taiko_Combo_Big = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Combo_Big.png");
        Play_Taiko_Combo_Text = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Combo_Text.png");
        Play_Taiko_Score_Base = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Score_Base.png");
        Play_Taiko_Score[0] = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Score_1P.png");
        Play_Taiko_Score[1] = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Score_2P.png");
        Play_Taiko_CourseSymbol = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}CourseSymbol.png");
        Play_Taiko_Shine = CreateSprite($"{playPath}Taiko{Path.DirectorySeparatorChar}Shine.png");
        

        Play_FlyNotes_Don = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Don.png");
        Play_FlyNotes_Ka = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Ka.png");
        Play_FlyNotes_Don_Big = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Don_Big.png");
        Play_FlyNotes_Ka_Big = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Ka_Big.png");
        Play_FlyNotes_Flash = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Flash.png");
        Play_FlyNotes_Flash_Big = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Flash_Big.png");
        Play_FlyNotes_Flash_Yellow = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Flash_Yellow.png");
        Play_FlyNotes_Flash_Yellow_Big = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Flash_Yellow_Big.png");
        Play_FlyNotes_Firework_Alt = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Firework_Alt.png");
        Play_FlyNotes_Firework = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}Firework.png");

        Play_BigEffect = CreateSprite($"{playPath}FlyNotes{Path.DirectorySeparatorChar}BigEffect.png");

        Play_Combo_Base[0] = CreateSprite($"{playPath}Combo{Path.DirectorySeparatorChar}Base_1P.png");
        Play_Combo_Base[1] = CreateSprite($"{playPath}Combo{Path.DirectorySeparatorChar}Base_2P.png");
        Play_Combo_Number[0] = CreateSprite($"{playPath}Combo{Path.DirectorySeparatorChar}Number_1P.png");
        Play_Combo_Number[1] = CreateSprite($"{playPath}Combo{Path.DirectorySeparatorChar}Number_2P.png");
        Play_Combo_Text = CreateSprite($"{playPath}Combo{Path.DirectorySeparatorChar}Text.png");

        Play_Roll_Base = CreateSprite($"{playPath}Roll{Path.DirectorySeparatorChar}Base.png");
        Play_Roll_Number = CreateSprite($"{playPath}Roll{Path.DirectorySeparatorChar}Number.png");
        
        Play_Balloon_Base = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Base.png");
        Play_Balloon_Number = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Number.png");
        Play_Balloon_Breaking[0] = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Breaking_0.png");
        Play_Balloon_Breaking[1] = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Breaking_1.png");
        Play_Balloon_Breaking[2] = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Breaking_2.png");
        Play_Balloon_Breaking[3] = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Breaking_3.png");
        Play_Balloon_Breaking[4] = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Breaking_4.png");
        Play_Balloon_Breaking[5] = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Breaking_5.png");
        Play_Balloon_Miss = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Miss.png");
        Play_Balloon_Rainbow = CreateSprite($"{playPath}Balloon{Path.DirectorySeparatorChar}Rainbow.png");

        Play_ScoreRank = CreateSprite($"{playPath}ScoreRank.png");

        Decide = CreateSound($"{Game.Skin.SoundsPath}Decide.ogg");
        Change = CreateSound($"{Game.Skin.SoundsPath}Change.ogg");

        for(int i = 0; i < Game.Skin.Value.HitSounds.Length; i++)
        {
            HitSoundJson hitSoundJson = Game.Skin.Value.HitSounds[i];
            HitSounds.Add(CreateHitSound(hitSoundJson));
        }
    }

    public void Dispose()
    {
        foreach(var item in DisposableAssets)
        {
            item.Dispose();
        }
        DisposableAssets.Clear();
    }
}