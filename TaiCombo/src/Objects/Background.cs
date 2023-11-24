using System.Drawing;
using Silk.NET.Maths;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Helper;
using TaiCombo.Luas;
using TaiCombo.Plugin.Chart;

namespace TaiCombo.Objects;

class Background
{
    private PlayBG Up;
    private PlayBG? Down;
    private PlayBG? Down_Clear;


    public void ClearIn(int player)
    {
        Up.ClearIn(player + 1);
        Down?.ClearIn(player + 1);
        Down_Clear?.ClearIn(player + 1);
    }

    public void ClearOut(int player)
    {
        Up.ClearOut(player + 1);
        Down?.ClearOut(player + 1);
        Down_Clear?.ClearOut(player + 1);
    }

    public Background(int playerCount)
    {
        Up = Game.Skin.Assets.Play_BG_Up["0"];
        Up.SetPlayerCount(playerCount);

        if (playerCount == 1)
        {
            Down = Game.Skin.Assets.Play_BG_Down["0"];
            Down_Clear = Game.Skin.Assets.Play_BG_Down_Clear["0"];
        }
    }

    public void Update()
    {
        Up.Update();
        Down?.Update();
        Down_Clear?.Update();
    }

    public void Draw()
    {
        Up.Draw();
        Down?.Draw();
        Down_Clear?.Draw();
    }
}