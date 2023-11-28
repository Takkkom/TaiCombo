using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Struct;
using TaiCombo.Helper;
using TaiCombo.Plugin.Enums;
using TaiCombo.Structs;

namespace TaiCombo.Objects;

class NamePlate : IDisposable
{
    public PlayerInfo PlayerInfo { get; private set; }

    private Sprite Name;

    public NamePlate(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;

        Name = Game.Skin.Assets.Font_Main.GenSpriteText(playerInfo.Name, new (1, 1, 1, 1), 26, new EdgeInfo[] { new EdgeInfo() { Color = new (0, 0, 0, 1), Ratio = 0.25f } });
    }

    public void Draw(int x, int y, int player, int side)
    {
        Game.Skin.Assets.NamePlate_Base.Draw(x, y);

        float name_scale = 1.0f;
        Name.Draw(x + 193, y + 45, name_scale, 1, null, false, false, 0, null, DrawOriginType.Center);


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

    public void Dispose()
    {
        Name.Dispose();
    }
}