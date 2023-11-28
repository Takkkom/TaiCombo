using Silk.NET.Maths;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Engine.Enums;
using TaiCombo.Scenes;
using TaiCombo.Skin;
using TaiCombo.Luas;
using TaiCombo.Enums;

namespace TaiCombo.Common;

class FadeManager
{
    private FadeScript? Script;

    private FadeState FadeState = FadeState.None;

    private float Interval_In;

    private float Interval_Out;

    private float Counter;

    private Action? Action;

    public FadeManager()
    {

    }

    public void StartFade(FadeScript fadeScript, float intervalIn, float intervalOut, Action action)
    {
        FadeState = FadeState.In;
        Script = fadeScript;
        Interval_In = intervalIn;
        Interval_Out = intervalOut;
        Action = action;
        Counter = 0;

        Script?.Init();
        Script?.FadeIn();
    }

    public void Update()
    {
        switch(FadeState)
        {
            case FadeState.None:
            break;
            case FadeState.In:
            {
                Script?.SetCounter(Counter);
                Counter += GameEngine.Time_.DeltaTime / Interval_In;
                if (Counter >= 1)
                {
                    FadeState = FadeState.Idle;
                    Script?.FadeIdle();
                    Counter = 0;
                    Task.Run(() =>
                    {
                        Action?.Invoke();
                        Script?.FadeOut();

                        FadeState = FadeState.Out;
                        Counter = 0;
                        Script?.SetCounter(0);
                    });
                }
            }
            break;
            case FadeState.Idle:
            {
            }
            break;
            case FadeState.Out:
            {
                Script?.SetCounter(Counter);
                Counter += GameEngine.Time_.DeltaTime / Interval_Out;
                if (Counter >= 1)
                {
                    FadeState = FadeState.None;
                    Counter = 0;
                    Script = null;
                    Action = null;
                }
            }
            break;
        }
        Script?.Update();
    }

    public void Draw()
    {
        Script?.Draw();
    }
}