using Silk.NET.Windowing;

namespace TaiCombo.Engine;

/// <summary>
/// 時間に関係する値がまとめてあります
/// </summary>
public class Time
{
    private IWindow Window_;

    public int FPS { get; private set; }

    /// <summary>
    /// フレームレートが上がるほど小さくなる値です
    /// 動きをフレームレートに依存させない処理をするために使用します
    /// </summary>
    public float DeltaTime { get; private set; }

    /// <summary>
    /// 現在の秒単位の時間
    /// リズムゲームのスクロールなどのシビアな制御で使用できます
    /// </summary>
    public float NowSecondTime { get; private set; }

    /// <summary>
    /// 現在のミリ秒単位の時間
    /// リズムゲームのスクロールなどのシビアな制御で使用できます
    /// </summary>
    public long NowMilliSecondTime { get; private set; }

    /// <summary>
    /// 現在のマイクロ秒単位の時間
    /// リズムゲームのスクロールなどのシビアな制御で使用できます
    /// </summary>
    public long NowMicroSecondTime { get; private set; }

    public Time(IWindow window)
    {
        Window_ = window;
    }

    public void Update(double deltaTime)
    {
        DeltaTime = (float)deltaTime;
        NowSecondTime = (float)Window_.Time;
        NowMilliSecondTime = (long)(NowSecondTime * 1000.0);
        NowMicroSecondTime = (long)(NowSecondTime * 1000000.0);

        FPS = (int)(1 / deltaTime);
    }
}