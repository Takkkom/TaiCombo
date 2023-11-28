using System.Text;
using NLua;

namespace TaiCombo.Engine;

public class LuaScript : IDisposable
{
    private LuaFuncs Funcs;
    protected Lua Script;

    private LuaFunction LuaFuncLoadAssets;
    private LuaFunction LuaFuncInit;
    

    public LuaScript(string fileName, FontRenderer mainFont, FontRenderer subFont)
    {
        Script = new();
        Script.State.Encoding = Encoding.UTF8;

        Funcs = new LuaFuncs(fileName, mainFont, subFont);
        Script["func"] = Funcs;
        Script.DoFile(fileName);

        LuaFuncLoadAssets = Script.GetFunction("loadAssets");
        LuaFuncInit = Script.GetFunction("init");

        LuaFuncLoadAssets.Call();
    }

    public virtual void Init()
    {
        LuaFuncInit.Call();
    }

    public void Dispose()
    {
        Funcs.Dispose();
        LuaFuncLoadAssets.Dispose();
        LuaFuncInit.Dispose();
    }
}