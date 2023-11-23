using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Structs;
using TaiCombo.Enums;

namespace TaiCombo.Scenes;

class StartupScene : Scene
{
    public StartupScene()
    {
    }

    public override void Activate()
    {
        Task.Run(() => 
        {
            Game.Skin.LoadAssets();
            GameEngine.SceneManager_.ChangeScene(new PlayScene(new () { 
                { "PlayerCount", 2 },
                //{ "ChartPath", "Songs/dummy.tja" },
                //{ "Courses", new Plugin.Enums.CourseType[] { Plugin.Enums.CourseType.Edit, Plugin.Enums.CourseType.Edit } },
                { "Courses", new Plugin.Enums.CourseType[] { Plugin.Enums.CourseType.Oni, Plugin.Enums.CourseType.Oni } },
                { "Options", new Options[] { 
                    new Options() { ScrollSpeed = 1.0f, Invisible = InvisibleType.None, Flip = false, Random = RandomType.None, Skippable = true, Offset = 0, NoVoice = NoVoiceType.None, HitSound = 0, ScoreType = ScoreType.Gen4, AutoPlay = true },
                    new Options() { ScrollSpeed = 1.2f, Invisible = InvisibleType.SEOnly, Flip = true, Random = RandomType.Random, Skippable = true, Offset = 5, NoVoice = NoVoiceType.Combo, HitSound = 0, ScoreType = ScoreType.Gen4, AutoPlay = true }
                 } },
                { "GaugeType", new GaugeType[] { (GaugeType)(-1), (GaugeType)(-1) } },
                { "PlaySpeed", 1.2f },
                { "GameMode", GameModeType.Play }
            }));
            
            /*
            GameEngine.SceneManager_.ChangeScene(new ResultScene(new () 
            { 
                {
                    "Values", 
                    new ResultValues[2] { 
                        new ResultValues() 
                        { 

                        },
                        new ResultValues() 
                        { 

                        }  
                    }
                }
            }));
            */
        });

        base.Activate();
    }

    public override void DeActivate()
    {
        base.DeActivate();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Draw()
    {
        base.Draw();
    }
}