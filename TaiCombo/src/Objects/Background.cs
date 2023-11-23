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
    }

    public void ClearOut(int player)
    {
        Up.ClearIn(player + 1);
    }

    public Background()
    {
        Up = Game.Skin.Assets.Play_BG_Up["0"];
        //Down = new("");
        //Down_Clear = new("");
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