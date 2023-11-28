using SkiaSharp;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGLES;
using Silk.NET.Windowing.Glfw;
using TaiCombo.Engine.Helpers;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Angle;
using TaiCombo.Logging;

namespace TaiCombo.Engine;

public class GameEngine : IDisposable
{
    private LogManager LogManager;

    private LibManager LibManager;
    
    private static AngleContext AngleContext;

    /// <summary>
    /// ウインドウのインスタンス
    /// </summary>
    public static IWindow Window_ { get; private set; }

    /// <summary>
    /// OpenGLのインスタンス
    /// </summary>
    public static GL Gl { get; private set; }

    public static WindowState WindowState_
    {
        get 
        {
            return Window_.WindowState;
        }
        set 
        {
            Window_.WindowState = value;
        }
    }

    private static void UpdateFramerate()
    {
        switch(FrameMode_)
        {
            case FrameMode.Unlimited:
            Window_.UpdatesPerSecond = 0;
            Window_.FramesPerSecond = 0;
            Window_.VSync = false;
            AngleContext?.SwapInterval(0);
            break;
            case FrameMode.Limited:
            Window_.UpdatesPerSecond = Framerate;
            Window_.FramesPerSecond = Framerate;
            Window_.VSync = false;
            AngleContext?.SwapInterval(0);
            break;
            case FrameMode.VSync:
            Window_.UpdatesPerSecond = 0;
            Window_.FramesPerSecond = 0;
            Window_.VSync = true;
            AngleContext?.SwapInterval(1);
            break;
        }
    }

    private static FrameMode FrameMode__;
    public static FrameMode FrameMode_
    {
        get 
        {
            return FrameMode__;
        }
        set 
        {
            FrameMode__ = value;
            UpdateFramerate();
        }
    }

    private static Vector2D<int> _WindowPosition;
    /// <summary>
    /// ウインドウの位置
    /// </summary>
    public static Vector2D<int> WindowPosition
    {
        get 
        {
            return _WindowPosition;
        }
        set 
        {
            _WindowPosition = value;
            Window_.Position = value;
        }
    }

    private static Vector2D<int> _WindowSize;
    /// <summary>
    /// ウインドウの大きさ
    /// 解像度ではありません
    /// </summary>
    public static Vector2D<int> WindowSize  
    {
        get 
        {
            return _WindowSize;
        }
        set 
        {
            _WindowSize = value;
            Window_.Size = value;
        }
    }

    private static double _Framerate;
    public static double Framerate 
    {
        get 
        {
            return _Framerate;
        }
        set 
        {
            _Framerate = value;
            UpdateFramerate();
        }
    }

    /// <summary>
    /// ゲームの解像度
    /// </summary>
    public static Vector2D<int> Resolution { get; set; }

    public static InputManager Input { get; private set; }

    public static SceneManager SceneManager_;

    /// <summary>
    /// 時間
    /// </summary>
    public static Time Time_;

    public static List<Action> ASyncActions { get; set; } = new();

    public static int MainThreadID { get; private set; }
        
    private Vector2D<int> ViewPortSize = new Vector2D<int>();
    private Vector2D<int> ViewPortOffset = new Vector2D<int>();


    public unsafe SKBitmap GetScreenShot()
    {
        int ViewportWidth = ViewPortSize.X;
        int ViewportHeight = ViewPortSize.Y;
        fixed(uint* pixels = new uint[(uint)ViewportWidth * (uint)ViewportHeight])
        {
            Gl.ReadBuffer(GLEnum.Front);
            Gl.ReadPixels(ViewPortOffset.X, ViewPortOffset.Y, (uint)ViewportWidth, (uint)ViewportHeight, PixelFormat.Bgra, GLEnum.UnsignedByte, pixels);

            fixed(uint* pixels2 = new uint[(uint)ViewportWidth * (uint)ViewportHeight])
            {
                for(int x = 0; x < ViewportWidth; x++)
                {
                    for(int y = 1; y < ViewportHeight; y++)
                    {
                        int pos = x + ((y - 1) * ViewportWidth);
                        int pos2 = x + ((ViewportHeight - y) * ViewportWidth);
                        var p = pixels[pos2];
                        pixels2[pos] = p;
                    }
                }
                    
                using SKBitmap sKBitmap = new(ViewportWidth, ViewportHeight - 1);
                sKBitmap.SetPixels((IntPtr)pixels2);
                return sKBitmap.Copy();
            }
        }
    }

    public unsafe void GetScreenShotAsync(Action<SKBitmap> action)
    {
        int ViewportWidth = ViewPortSize.X;
        int ViewportHeight = ViewPortSize.Y;
        byte[] pixels = new byte[(uint)ViewportWidth * (uint)ViewportHeight * 4];
        Gl.ReadBuffer(GLEnum.Front);
        fixed(byte* pix = pixels)
        {
            Gl.ReadPixels(ViewPortOffset.X, ViewPortOffset.Y, (uint)ViewportWidth, (uint)ViewportHeight, PixelFormat.Bgra, GLEnum.UnsignedByte, pix);
        }

            Task.Run(() =>{
                fixed(byte* pixels2 = new byte[(uint)ViewportWidth * (uint)ViewportHeight * 4])
                {
                    for(int x = 0; x < ViewportWidth; x++)
                    {
                        for(int y = 1; y < ViewportHeight; y++)
                        {
                            int pos = x + ((y - 1) * ViewportWidth);
                            int pos2 = x + ((ViewportHeight - y) * ViewportWidth);
                            pixels2[(pos * 4) + 0] = pixels[(pos2 * 4) + 0];
                            pixels2[(pos * 4) + 1] = pixels[(pos2 * 4) + 1];
                            pixels2[(pos * 4) + 2] = pixels[(pos2 * 4) + 2];
                            pixels2[(pos * 4) + 3] = 255;
                        }
                    }
                        
                    using SKBitmap sKBitmap = new(ViewportWidth, ViewportHeight - 1);
                    sKBitmap.SetPixels((IntPtr)pixels2);
                    action(sKBitmap);
                }
            });
    }

    public GameEngine(string title)
    {
        LogManager = new();
        LibManager = new();
        MainThreadID = Thread.CurrentThread.ManagedThreadId;
        //GlfwWindowing.Use();

        WindowOptions options = WindowOptions.Default with 
        {
            Title = title,
            WindowBorder = WindowBorder.Resizable,
            API = GraphicsAPI.None,
            IsVisible = false,

            WindowState = WindowState.Normal,
            Position = new Vector2D<int>(100, 100),
            Size = new Vector2D<int>(1280, 720),
            FramesPerSecond = 60,
            UpdatesPerSecond = 60,
            VSync = true
        };

        Window_ = Window.Create(options);
        Window_.Load += OnLoad;
        Window_.Closing += OnClosing;
        Window_.Update += OnUpdate;
        Window_.Render += OnRender;
        Window_.Resize += OnResize;
        Window_.Move += OnMove;

        Configurate();

        Window_.IsVisible = true;
    }

    private void OnLoad()
    {
        #if !DEBUG
        try
        #endif
        {
            LogManager.Write("Initializing Sound");
            Sound.Init();
            LogManager.Write("Sound initialization complete");

            LogManager.Write("Initializing AngleContext");
            AngleContext = new AngleContext(Silk.NET.GLFW.AnglePlatformType.OpenGL, Window_);
            AngleContext.MakeCurrent();
            LogManager.Write("AngleContext initialization complete");

            Gl = new GL(AngleContext);
            Gl.Enable(GLEnum.Blend);
            BlendHelper.SetBlend(BlendType.Normal);
            AngleContext.SwapInterval(FrameMode_ == FrameMode.VSync ? 1 : 0);
            Sprite.Init();

            Input = new(Window_);
            Time_ = new(Window_);

            SceneManager_ = new();

            LogManager.Write("Initializing Game");
            Init();
            LogManager.Write("Game initialization complete");
        }
        #if !DEBUG
        catch(Exception ex)
        {
            LogManager.Write(ex.ToString());
        }
        #endif
    }

    private void OnClosing()
    {
        Terminate();

        SceneManager_.Dispose();
        Input.Dispose();
        Sprite.Terminate();
        AngleContext.Dispose();
        Gl.Dispose();
        
        Sound.Init();
    }

    private void OnUpdate(double deltaTime)
    {
        #if !DEBUG
        try
        #endif
        {
            if (ASyncActions.Count > 0)
            {
                ASyncActions[0]?.Invoke();
                ASyncActions.Remove(ASyncActions[0]);
            }
            
            Input.Update();
            Time_.Update(deltaTime);

            SceneManager_.Update();

            Update();
        }
        #if !DEBUG
        catch(Exception ex)
        {
            LogManager.Write(ex.ToString());
        }
        #endif
    }

    private void OnRender(double deltaTime)
    {
        #if !DEBUG
        try
        #endif
        {
            Gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SceneManager_.Draw();

            Draw();

            AngleContext.SwapBuffers();
        }
        #if !DEBUG
        catch(Exception ex)
        {
            LogManager.Write(ex.ToString());
        }
        #endif
    }

    private void OnResize(Vector2D<int> size)
    {
        if (size.X > 0 && size.Y > 0)
        {
            float resolutionAspect = (float)Resolution.X / Resolution.Y;

            if (Window_.WindowState == WindowState.Normal)
            {
                if (size.X != WindowSize.X)
                {
                    size.Y = (int)(size.X / resolutionAspect);
                }
                else
                {
                    size.X = (int)(size.Y * resolutionAspect);
                }
            }

            
            float windowAspect = (float)size.X / size.Y;
            if (windowAspect > resolutionAspect)
            {
                ViewPortSize.X = (int)(size.Y * resolutionAspect);
                ViewPortSize.Y = size.Y;
            }
            else 
            {
                ViewPortSize.X = size.X;
                ViewPortSize.Y = (int)(size.X / resolutionAspect);
            }
        }
        ViewPortOffset.X = (size.X - ViewPortSize.X) / 2;
        ViewPortOffset.Y = (size.Y - ViewPortSize.Y) / 2;

        Gl.Viewport(ViewPortOffset.X, ViewPortOffset.Y, (uint)ViewPortSize.X, (uint)ViewPortSize.Y);

        WindowSize = size;
    }

    private void OnMove(Vector2D<int> pos)
    {

    }

    /// <summary>
    /// 設定
    /// </summary>
    protected virtual void Configurate()
    {

    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected virtual void Init()
    {

    }

    /// <summary>
    /// 終了
    /// </summary>
    protected virtual void Terminate()
    {

    }

    /// <summary>
    /// 更新
    /// </summary>
    protected virtual void Update()
    {

    }

    /// <summary>
    /// 描画
    /// </summary>
    protected virtual void Draw()
    {

    }

    public void Run()
    {
        Window_.Run();
    }

    public void Dispose()
    {
        Window_.Dispose();
        LogManager.Dispose();
    }
}