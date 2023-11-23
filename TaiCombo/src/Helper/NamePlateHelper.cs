using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Plugin.Chart;
using TaiCombo.Structs;

namespace TaiCombo.Helper;

static class NamePlateHelper
{
    public static void DrawNamePlate(int x, int y, PlayerInfo playerInfo, int side, int player)
    {
        Game.Skin.Assets.NamePlate_Base.Draw(x, y);

        if (playerInfo.TitleInfo == null)
        {

        }
        else 
        {

        }


        switch(side)
        {
            case 0:
            Game.Skin.Assets.NamePlate_Left.Draw(x, y);
            break;
            case 1:
            Game.Skin.Assets.NamePlate_Right.Draw(x, y);
            break;
        }

        switch(player)
        {
            case 0:
            Game.Skin.Assets.NamePlate_1P.Draw(x, y);
            break;
            case 1:
            Game.Skin.Assets.NamePlate_2P.Draw(x, y);
            break;
        }
    }
}