using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Luas;

namespace TaiCombo.Objects;

class Background
{
    private PlayBG? Up;
    private PlayBG? Down;
    private PlayBG? Down_Clear;
    private PlayBG? Dancer;
    private Sprite? Footer;
    private PlayBG? Mob;


    public void SetGauge(float[] bpm)
    {
        Up?.SetGauge(bpm);
        Down?.SetGauge(bpm);
        Down_Clear?.SetGauge(bpm);
        Dancer?.SetGauge(bpm);
        Mob?.SetGauge(bpm);
    }

    public void SetBPM(float[] bpm)
    {
        Up?.SetBPM(bpm);
        Down?.SetBPM(bpm);
        Down_Clear?.SetBPM(bpm);
        Dancer?.SetBPM(bpm);
        Mob?.SetBPM(bpm);
    }

    public void ClearIn(int player)
    {
        Up?.ClearIn(player + 1);
        Down?.ClearIn(player + 1);
        Down_Clear?.ClearIn(player + 1);
        Dancer?.ClearIn(player + 1);
        Mob?.ClearIn(player + 1);
    }

    public void ClearOut(int player)
    {
        Up?.ClearOut(player + 1);
        Down?.ClearOut(player + 1);
        Down_Clear?.ClearOut(player + 1);
        Dancer?.ClearOut(player + 1);
        Mob?.ClearOut(player + 1);
    }

    public void MaxIn(int player)
    {
        Up?.MaxIn(player + 1);
        Down?.MaxIn(player + 1);
        Down_Clear?.MaxIn(player + 1);
        Dancer?.MaxIn(player + 1);
        Mob?.MaxIn(player + 1);
    }

    public void MaxOut(int player)
    {
        Up?.MaxOut(player + 1);
        Down?.MaxOut(player + 1);
        Down_Clear?.MaxOut(player + 1);
        Dancer?.MaxOut(player + 1);
        Mob?.MaxOut(player + 1);
    }

    public Background(int playerCount, bool rightSide)
    {
        Up = Game.Skin.Assets.Play_BG_Up["0"];
        Up.SetPlayerCount(playerCount);
        Up.SetP1IsBlue(rightSide);
        Up.Init();

        if (playerCount == 1)
        {
            Down = Game.Skin.Assets.Play_BG_Down["0"];
            Down.Init();

            Down_Clear = Game.Skin.Assets.Play_BG_Down_Clear["0"];
            Down_Clear.Init();

            Dancer = Game.Skin.Assets.Play_BG_Dancer["0"];
            Dancer.Init();

            Footer = Game.Skin.Assets.Play_BG_Footer["0"];

            Mob = Game.Skin.Assets.Play_BG_Mob["0"];
            Mob.Init();
        }
    }

    public void Update()
    {
        Up?.Update();
        Down?.Update();
        Down_Clear?.Update();
        Dancer?.Update();
        Mob?.Update();
    }

    public void Draw()
    {
        Up?.Draw();
        Down?.Draw();
        Down_Clear?.Draw();

        Dancer?.Draw();
        Footer?.Draw(0, GameEngine.Resolution.Y, drawOriginType:DrawOriginType.Left_Down);
        Mob?.Draw();
    }
}