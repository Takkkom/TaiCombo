using System.Text;
using NLua;
using TaiCombo.Engine;

namespace TaiCombo.Luas;

public class BaseScript : LuaScript
{
    private LuaFunction LuaFuncUpdate;
    private LuaFunction LuaFuncDraw;
    

    public BaseScript(string fileName, FontRenderer mainFont, FontRenderer subFont) : base(fileName, mainFont, subFont)
    {
        LuaFuncUpdate = Script.GetFunction("update");
        LuaFuncDraw = Script.GetFunction("draw");
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

    public new void Dispose()
    {
        LuaFuncUpdate.Dispose();
        LuaFuncDraw.Dispose();
        base.Dispose();
    }
}