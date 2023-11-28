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
    
    public StartupScene()
    {
    }

    public override void Activate()
    {
        Game.Skin.LoadAssets();
        NamePlates[0] = new(new PlayerInfo() { Name = "えんぷてぃ", TitleInfo = new TitleInfo() { Title = "かり", TitleType = TitleType.Gold }, DanInfo = new DanInfo() { Title = "変人", ClearType = ClearType.AllPerfect } });
        NamePlates[1] = new(new PlayerInfo() { Name = "えんぷてぃ", TitleInfo = new TitleInfo() { Title = "かり", TitleType = TitleType.Gold }, DanInfo = new DanInfo() { Title = "変人", ClearType = ClearType.AllPerfect } });


        base.Activate();
    }

    public override void DeActivate()
    {
        base.DeActivate();
    }

    public override void Update()
    {
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.Enter))
        {
            //
            Game.Skin.Assets.Fade_ToPlay.SetValues(new Dictionary<string, object>() { 
                { "title", "テストです" },
                { "subtitle", "test" }
            });
            Game.Fade.StartFade(Game.Skin.Assets.Fade_ToPlay, 1.0f, 1.0f, () =>
            {
                GameEngine.SceneManager_.ChangeScene(new PlayScene(new () { 
                    { "PlayerCount", 1 },
                    { "ChartPath", "Songs/dummy.tja" },
                    //{ "Courses", new Plugin.Enums.CourseType[] { Plugin.Enums.CourseType.Edit, Plugin.Enums.CourseType.Edit } },
                    //{ "Courses", new Plugin.Enums.CourseType[] { Plugin.Enums.CourseType.Oni, Plugin.Enums.CourseType.Edit } },
                    { "Courses", new Plugin.Enums.CourseType[] { Plugin.Enums.CourseType.Oni, Plugin.Enums.CourseType.Oni } },
                    { "Options", new Options[] { 
                        new Options() { ScrollSpeed = 1.0f, Invisible = InvisibleType.None, Flip = false, Random = RandomType.None, Skippable = true, Offset = 0, NoVoice = NoVoiceType.None, HitSound = 0, ScoreType = ScoreType.Gen4, AutoPlay = false },
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
        base.Draw();
    }
}