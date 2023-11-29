
using NLua;
using TaiCombo.Engine;

namespace TaiCombo.Luas;

class ResultBG : BaseScript
{
    private LuaFunction LuaFuncPlayClearAnime;

    public void PlayClearAnime(int player)
    {
        LuaFuncPlayClearAnime.Call(player + 1);
    }


    public void SetPlayerCount(int playerCount)
    {
        Script["playerCount"] = playerCount;
    }

    public void SetP1IsBlue(bool p1IsBlue)
    {
        Script["p1IsBlue"] = p1IsBlue;
    }

    public void SetValues(Dictionary<string, object> values)
    {
        foreach(var info in values)
        {
            Script[info.Key] = info.Value;
        }
    }

    public ResultBG(string fileName, FontRenderer mainFont, FontRenderer subFont) : base(fileName, mainFont, subFont)
    {
        LuaFuncPlayClearAnime = Script.GetFunction("playClearAnime");
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