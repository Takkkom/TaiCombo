
using NLua;
using TaiCombo.Engine;

namespace TaiCombo.Luas;

class FadeScript : BaseScript
{
    private LuaFunction LuaFuncFadeIn;
    private LuaFunction LuaFuncFadeOut;

    public void FadeIn()
    {
        Script["state"] = "In";
        LuaFuncFadeIn.Call();
    }

    public void FadeIdle()
    {
        Script["state"] = "Idle";
        LuaFuncFadeIn.Call();
    }

    public void FadeOut()
    {
        Script["state"] = "Out";
        LuaFuncFadeOut.Call();
    }

    public void SetCounter(float value)
    {
        Script["counter"] = value;
    }

    public void SetValues(Dictionary<string, object> values)
    {
        foreach(var info in values)
        {
            Script[info.Key] = info.Value;
        }
    }

    public FadeScript(string fileName, FontRenderer mainFont, FontRenderer subFont) : base(fileName, mainFont, subFont)
    {
        LuaFuncFadeIn = Script.GetFunction("fadeIn");
        LuaFuncFadeOut = Script.GetFunction("fadeOut");
        SetCounter(0);
    }

    public override void Init()
    {
        base.Init();
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