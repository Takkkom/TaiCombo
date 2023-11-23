using System.Text;
using NLua;

namespace TaiCombo.Engine;

public class LuaScript : IDisposable
{
    protected Lua Script;

    private LuaFunction LuaFuncLoadAssets;
    private LuaFunction LuaFuncInit;
    private LuaFunction LuaFuncUpdate;
    private LuaFunction LuaFuncDraw;
    

    public LuaScript(string fileName)
    {
        Script = new();
        Script.State.Encoding = Encoding.UTF8;

        Script["func"] = new LuaFuncs(fileName);
        Script.DoFile(fileName);

        LuaFuncLoadAssets = Script.GetFunction("loadAssets");
        LuaFuncInit = Script.GetFunction("init");
        LuaFuncUpdate = Script.GetFunction("update");
        LuaFuncDraw = Script.GetFunction("draw");

        LuaFuncLoadAssets.Call();
    }

    public virtual void Init()
    {
        LuaFuncInit.Call();
    }

    public virtual void Update()
    {
        Script["deltaTime"] = GameEngine.Time_.DeltaTime;
        LuaFuncUpdate.Call();
    }

    public virtual void Draw()
    {
        LuaFuncDraw.Call();
    }

    public void Dispose()
    {
        LuaFuncLoadAssets.Dispose();
        LuaFuncInit.Dispose();
        LuaFuncUpdate.Dispose();
        LuaFuncDraw.Dispose();
    }
}