using Silk.NET.Maths;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Engine.Enums;
using TaiCombo.Scenes;
using TaiCombo.Skin;

namespace TaiCombo.Common;

class Game : GameEngine
{
    public const string TITLE = "TaiCombo";
    
    public const int MAXPLAYER = 2;

    public static MainConfig Config { get; private set; } = new();

    public static SkinManager Skin { get; private set; } = new();

    public static PluginManager Plugins { get; private set; } = new();
    

    private Overlay Overlay_;
    
    public Game() : base(TITLE)
    {
    }

    protected override void Configurate()
    {
        Config.Read();
        Skin.Read();

        FrameMode_ = Config.Value.FrameMode;
        WindowState_ = Config.Value.WindowState;
        WindowPosition = new Vector2D<int>(Config.Value.WindowX, Config.Value.WindowY);
        WindowSize = new Vector2D<int>(Config.Value.WindowWidth, Config.Value.WindowHeight);
        Framerate = Config.Value.Framerate;
        Resolution = new Vector2D<int>(Skin.Value.Resolution.Width, Skin.Value.Resolution.Height);

        base.Configurate();
    }

    protected override void Init()
    {
        Plugins.Init();
        
        Overlay_ = new();
        Overlay_.Activate();

        SceneManager_.ChangeScene(new StartupScene());

        base.Init();
    }

    protected override void Terminate()
    {
        Plugins.Terminate();

        Config.Value.FrameMode = FrameMode_;
        Config.Value.WindowState = WindowState_;
        Config.Value.WindowX = WindowPosition.X;
        Config.Value.WindowY = WindowPosition.Y;
        Config.Value.WindowWidth = WindowSize.X;
        Config.Value.WindowHeight = WindowSize.Y;
        Config.Value.Framerate = Framerate;

        Overlay_.DeActivate();

        Config.Write();
        
        base.Terminate();
    }

    protected override void Update()
    {




        Overlay_.Update();
        
        base.Update();
    }

    protected override void Draw()
    {


        Overlay_.Draw();

        base.Draw();
    }
}