
using NLua;
using TaiCombo.Engine;

namespace TaiCombo.Luas;

class EndAnineScript : PlayBG
{
    private LuaFunction LuaFuncPlayAnime;

    public void PlayAnime(int player)
    {
        LuaFuncPlayAnime.Call(player);
    }

    public EndAnineScript(string fileName, FontRenderer mainFont, FontRenderer subFont) : base(fileName, mainFont, subFont)
    {
        LuaFuncPlayAnime = Script.GetFunction("playAnime");
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