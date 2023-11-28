
using NLua;
using TaiCombo.Engine;

namespace TaiCombo.Luas;

class PlayBG : BaseScript
{
    private LuaFunction LuaFuncAddRollEffect;
    private LuaFunction LuaFuncClearIn;
    private LuaFunction LuaFuncClearOut;
    private LuaFunction LuaFuncMaxIn;
    private LuaFunction LuaFuncMaxOut;

    public void SetGauge(float[] gauge)
    {
        Script["gauge"] = gauge;
    }

    public void SetBPM(float[] bpm)
    {
        Script["bpm"] = bpm;
    }

    public void AddRollEffect(int player)
    {
        LuaFuncAddRollEffect.Call(player);
    }

    public void ClearIn(int player)
    {
        LuaFuncClearIn.Call(player);
    }

    public void ClearOut(int player)
    {
        LuaFuncClearOut.Call(player);
    }

    public void MaxIn(int player)
    {
        LuaFuncMaxIn.Call(player);
    }

    public void MaxOut(int player)
    {
        LuaFuncMaxOut.Call(player);
    }
    
    public void GoGoIn()
    {
        
    }

    public void GoGoOut()
    {
        
    }

    public void SetPlayerCount(int playerCount)
    {
        Script["playerCount"] = playerCount;
    }

    public void SetP1IsBlue(bool p1IsBlue)
    {
        Script["p1IsBlue"] = p1IsBlue;
    }

    public PlayBG(string fileName, FontRenderer mainFont, FontRenderer subFont) : base(fileName, mainFont, subFont)
    {
        LuaFuncAddRollEffect = Script.GetFunction("addRollEffect");
        LuaFuncClearIn = Script.GetFunction("clearIn");
        LuaFuncClearOut = Script.GetFunction("clearOut");
        LuaFuncMaxIn = Script.GetFunction("maxIn");
        LuaFuncMaxOut = Script.GetFunction("maxOut");

        SetGauge(new float[] { 0, 0 });
        SetBPM(new float[] { 150, 150 });
        SetPlayerCount(1);
        SetP1IsBlue(false);
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