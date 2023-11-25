using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Helper;
using TaiCombo.Luas;

namespace TaiCombo.Objects;

class EndAnime
{
    private GameModeType GameMode;

    private ClearType[] ClearTypes = new ClearType[Game.MAXPLAYER];

    public void PlayAnime(ClearType clearType, int player)
    {
        ClearTypes[player] = clearType;
        switch (clearType)
        {
            case ClearType.None:
            Game.Skin.Assets.Play_EndAnime_Failed_Sound?.Play();
            Game.Skin.Assets.Play_EndAnime_Failed?.PlayAnime(player + 1);
            break;
            case ClearType.Clear:
            Game.Skin.Assets.Play_EndAnime_Clear_Sound?.Play();
            Game.Skin.Assets.Play_EndAnime_Clear?.PlayAnime(player + 1);
            break;
            case ClearType.FullCombo:
            Game.Skin.Assets.Play_EndAnime_FullCombo_Sound?.Play();
            Game.Skin.Assets.Play_EndAnime_FullCombo?.PlayAnime(player + 1);
            break;
            case ClearType.AllPerfect:
            Game.Skin.Assets.Play_EndAnime_AllPerfect_Sound?.Play();
            Game.Skin.Assets.Play_EndAnime_AllPerfect?.PlayAnime(player + 1);
            break;
        }
    }

    public EndAnime(GameModeType gameMode, int playerCount)
    {
        GameMode = gameMode;
        
        Game.Skin.Assets.Play_EndAnime_Failed.SetPlayerCount(playerCount);
        Game.Skin.Assets.Play_EndAnime_Failed?.Init();

        Game.Skin.Assets.Play_EndAnime_Clear.SetPlayerCount(playerCount);
        Game.Skin.Assets.Play_EndAnime_Clear?.Init();
        
        Game.Skin.Assets.Play_EndAnime_FullCombo.SetPlayerCount(playerCount);
        Game.Skin.Assets.Play_EndAnime_FullCombo?.Init();

        Game.Skin.Assets.Play_EndAnime_AllPerfect.SetPlayerCount(playerCount);
        Game.Skin.Assets.Play_EndAnime_AllPerfect?.Init();
    }

    public void Update()
    {
        Game.Skin.Assets.Play_EndAnime_Failed?.Update();
        Game.Skin.Assets.Play_EndAnime_Clear?.Update();
        Game.Skin.Assets.Play_EndAnime_FullCombo?.Update();
        Game.Skin.Assets.Play_EndAnime_AllPerfect?.Update();
    }

    public void Draw()
    {
        Game.Skin.Assets.Play_EndAnime_Failed?.Draw();
        Game.Skin.Assets.Play_EndAnime_Clear?.Draw();
        Game.Skin.Assets.Play_EndAnime_FullCombo?.Draw();
        Game.Skin.Assets.Play_EndAnime_AllPerfect?.Draw();
    }
}