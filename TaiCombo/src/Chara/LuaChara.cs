using System.Text;
using NLua;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;

namespace TaiCombo.Chara;

class LuaChara : IPlayerChara
{
    public string DirPath { get; set; }
    public string Name { get; set; }

    private bool Disposed = true;
    
    private LuaFuncs Funcs;
    protected Lua Script;

    private LuaFunction LuaFuncLoadAssets;
    private LuaFunction LuaFuncChangeAnime;
    private LuaFunction LuaFuncUpdate;
    private LuaFunction LuaFuncDraw;

    private Action?[] Actions = new Action?[Game.MAXPLAYER];

    private void InvokeEndFunc(int player)
    {
        Actions[player - 1]?.Invoke();
    }


    public LuaChara(string dirPath)
    {
        DirPath = dirPath;
        Name = "None";
        
    }

    public void LoadAssets()
    {
        if(Disposed)
        {
            Script = new();
            Script.State.Encoding = Encoding.UTF8;

            string luaPath = $"{DirPath}{Path.DirectorySeparatorChar}Script.lua";
            Funcs = new LuaFuncs(luaPath, Game.Skin.Assets.Font_Main, Game.Skin.Assets.Font_Sub);
            Script["func"] = Funcs;
            Script.DoFile(luaPath);

            LuaFuncLoadAssets = Script.GetFunction("loadAssets");
            LuaFuncUpdate = Script.GetFunction("update");
            LuaFuncDraw = Script.GetFunction("draw");
            LuaFuncChangeAnime = Script.GetFunction("changeAnime");

            Script["invokeEndFunc"] = InvokeEndFunc;

            LuaFuncLoadAssets.Call();
        }
        Disposed = false;
    }

    public void Dispose()
    {
        if (!Disposed)
        {
            Funcs.Dispose();
            LuaFuncLoadAssets.Dispose();
            Script.Dispose();
        }
        Disposed = true;
    }

    public void ChangeAnime(CharaAnimeType charaAnimeType, int player, bool loop, Action? action = null)
    {
        LuaFuncChangeAnime.Call(EnumToString.CharaAnimeTypeToString(charaAnimeType), player + 1, loop);
        Actions[player] = action;
    }

    public void Update(float bpm, CharaSceneType charaSceneType, int player)
    {
        if (Disposed) return;

        Script["deltaTime"] = GameEngine.Time_.DeltaTime;
        LuaFuncUpdate.Call(bpm, charaSceneType, player + 1);
    }
    
    public void Draw(float x, float y, float scale, bool flipX, bool flipY, CharaSceneType charaSceneType, float opacity, int player)
    {
        if (Disposed) return;

        LuaFuncDraw.Call(x, y, scale, flipX, flipY, Game.Skin.Value.Resolution.Width, Game.Skin.Value.Resolution.Height, EnumToString.CharaSceneTypeToString(charaSceneType), opacity, player + 1);
    }
}