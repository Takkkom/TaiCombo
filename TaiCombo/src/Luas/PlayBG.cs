
using NLua;
using TaiCombo.Engine;

namespace TaiCombo.Luas;

class PlayBG : LuaScript
{
    private LuaFunction LuaFuncClearIn;
    private LuaFunction LuaFuncClearOut;

    public void ClearIn(int player)
    {
        LuaFuncClearIn.Call(player);
    }

    public void ClearOut(int player)
    {
        LuaFuncClearOut.Call(player);
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

    public PlayBG(string fileName) : base(fileName)
    {
        LuaFuncClearIn = Script.GetFunction("clearIn");
        LuaFuncClearOut = Script.GetFunction("clearOut");
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