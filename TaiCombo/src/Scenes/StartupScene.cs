using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Structs;
using TaiCombo.Enums;
using TaiCombo.Objects;

namespace TaiCombo.Scenes;

class StartupScene : Scene
{
    private NamePlate[] NamePlates = new NamePlate[2];

    private Sprite[] Texts;
    private string[] Charts;
    private int Index;

    public StartupScene()
    {
        Charts = Directory.GetFiles($"Songs{Path.DirectorySeparatorChar}", "*.tja", SearchOption.AllDirectories);
    }

    public override void Activate()
    {
        Game.Skin.LoadAssets();
        NamePlates[0] = new(new PlayerInfo() { Name = "えんぷてぃ", TitleInfo = new TitleInfo() { Title = "かり", TitleType = TitleType.Gold }, DanInfo = new DanInfo() { Title = "変人", ClearType = ClearType.AllPerfect } });
        NamePlates[1] = new(new PlayerInfo() { Name = "えんぷてぃ", TitleInfo = new TitleInfo() { Title = "かり", TitleType = TitleType.Gold }, DanInfo = new DanInfo() { Title = "変人", ClearType = ClearType.AllPerfect } });

        Texts = new Sprite[Charts.Length];
        for(int i = 0; i < Texts.Length; i++)
        {
            Texts[i] = Game.Skin.Assets.Font_Main.GenSpriteText(Charts[i], new Color4(1, 1, 1, 1), 40);
        }


        base.Activate();
    }

    public override void DeActivate()
    {
        for(int i = 0; i < Texts.Length; i++)
        {
            Texts[i].Dispose();
        }
        base.DeActivate();
    }

    public override void Update()
    {
        
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.Up))
        {
            Index = Math.Max(Index - 1, 0);
        }
        else if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.Down))
        {
            Index = Math.Min(Index + 1, Charts.Length - 1);
        }
        else if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.Enter))
        {
            /*
            Game.Fade.StartFade(Game.Skin.Assets.Fade_Black, 1.5f, 0.5f, () =>
            {
                GameEngine.SceneManager_.ChangeScene(new ResultScene(new () 
                { 
                    { "PlayerCount", 2 },
                    { "RightSide", false },
                    { "GaugeType", new GaugeType[] { GaugeType.Level0, GaugeType.Level2 } },
                    { "NamePlates", NamePlates },
                    { "Title", "タイトル" },
                    { "SubTitle", "サブタイトル" },
                    { "Options", new Options[] { 
                        new Options() { ScrollSpeed = 1.0f, Invisible = InvisibleType.None, Flip = false, Random = RandomType.None, Skippable = true, Offset = 0, NoVoice = NoVoiceType.None, HitSound = 0, ScoreType = ScoreType.Gen4, AutoPlay = true },
                        new Options() { ScrollSpeed = 1.0f, Invisible = InvisibleType.None, Flip = false, Random = RandomType.None, Skippable = true, Offset = 0, NoVoice = NoVoiceType.None, HitSound = 0, ScoreType = ScoreType.Gen4, AutoPlay = true }
                    } },
                    {
                        "Values", new ResultValues[2] { 
                             new ResultValues() 
                            { 
                                Gauge = 100,
                                Perfect = 100,
                                Ok = 100,
                                Miss = 100,
                                Roll = 100,
                                MaxCombo = 100,
                                Score = 1000000,
                                ScoreRank = ScoreRankType.Omega,
                                ClearType = ClearType.AllPerfect
                            },
                            new ResultValues() 
                            { 
                                Gauge = 100,
                                Perfect = 100,
                                Ok = 100,
                                Miss = 100,
                                Roll = 100,
                                MaxCombo = 100,
                                Score = 1000000,
                                ScoreRank = ScoreRankType.Omega,
                                ClearType = ClearType.AllPerfect
                            }  
                        }
                    }
                }));
            });
            */
            Game.Skin.Assets.Fade_ToPlay.SetValues(new Dictionary<string, object>() { 
                { "title", "テストです" },
                { "subtitle", "test" }
            });
            Game.Fade.StartFade(Game.Skin.Assets.Fade_ToPlay, 1.0f, 1.0f, () =>
            {
                GameEngine.SceneManager_.ChangeScene(new PlayScene(new () { 
                    { "PlayerCount", 2 },
                    { "ChartPath", Charts[Index] },
                    { "Courses", new Plugin.Enums.CourseType[] { Plugin.Enums.CourseType.Oni, Plugin.Enums.CourseType.Oni } },
                    { "Options", new Options[] { 
                        new Options() { ScrollSpeed = 1.0f, Invisible = InvisibleType.None, Flip = false, Random = RandomType.None, Skippable = true, Offset = 0, NoVoice = NoVoiceType.None, HitSound = 0, ScoreType = ScoreType.Gen4, AutoPlay = true },
                        new Options() { ScrollSpeed = 1.0f, Invisible = InvisibleType.None, Flip = false, Random = RandomType.None, Skippable = true, Offset = 0, NoVoice = NoVoiceType.None, HitSound = 0, ScoreType = ScoreType.Gen4, AutoPlay = true }
                    } },
                    { "GaugeType", new GaugeType[] { (GaugeType)(-1), (GaugeType)(-1) } },
                    { "PlaySpeed", 1.0f },
                    { "GameMode", GameModeType.Play },
                    { "RightSide", false },
                    { "NamePlates", NamePlates }
                }));
            });
        }
        base.Update();
    }

    public override void Draw()
    {
        int center = 4;
        for(int i = 0; i < 9; i++)
        {
            int index = i - center;
            int trueIndex = index + Index;
            if (trueIndex < 0 || trueIndex >= Texts.Length) continue;

            float x = 960;
            float y = 540 + (index * 40);
            Color4 color = index == 0 ? new Color4(1, 0, 0, 1) : new Color4(1, 1, 1, 1);

            Texts[trueIndex].Draw(x, y, color:color, drawOriginType:Engine.Enums.DrawOriginType.Center);
        }
        base.Draw();
    }
}