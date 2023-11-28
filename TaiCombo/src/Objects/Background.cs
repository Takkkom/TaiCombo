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
    private PlayBG RollEffect;
    private PlayBG? Dancer;
    private Sprite? Footer;
    private PlayBG? Mob;


    public void SetGauge(float[] bpm)
    {
        Up?.SetGauge(bpm);
        Down?.SetGauge(bpm);
        Down_Clear?.SetGauge(bpm);
        RollEffect?.SetGauge(bpm);
        Dancer?.SetGauge(bpm);
        Mob?.SetGauge(bpm);
    }

    public void SetBPM(float[] bpm)
    {
        Up?.SetBPM(bpm);
        Down?.SetBPM(bpm);
        Down_Clear?.SetBPM(bpm);
        RollEffect?.SetBPM(bpm);
        Dancer?.SetBPM(bpm);
        Mob?.SetBPM(bpm);
    }

    public void AddRollEffect(int player)
    {
        Up?.AddRollEffect(player + 1);
        Down?.AddRollEffect(player + 1);
        Down_Clear?.AddRollEffect(player + 1);
        RollEffect?.AddRollEffect(player + 1);
        Dancer?.AddRollEffect(player + 1);
        Mob?.AddRollEffect(player + 1);
    }

    public void ClearIn(int player)
    {
        Up?.ClearIn(player + 1);
        Down?.ClearIn(player + 1);
        Down_Clear?.ClearIn(player + 1);
        RollEffect?.ClearIn(player + 1);
        Dancer?.ClearIn(player + 1);
        Mob?.ClearIn(player + 1);
    }

    public void ClearOut(int player)
    {
        Up?.ClearOut(player + 1);
        Down?.ClearOut(player + 1);
        Down_Clear?.ClearOut(player + 1);
        RollEffect?.ClearOut(player + 1);
        Dancer?.ClearOut(player + 1);
        Mob?.ClearOut(player + 1);
    }

    public void MaxIn(int player)
    {
        Up?.MaxIn(player + 1);
        Down?.MaxIn(player + 1);
        Down_Clear?.MaxIn(player + 1);
        RollEffect?.MaxIn(player + 1);
        Dancer?.MaxIn(player + 1);
        Mob?.MaxIn(player + 1);
    }

    public void MaxOut(int player)
    {
        Up?.MaxOut(player + 1);
        Down?.MaxOut(player + 1);
        Down_Clear?.MaxOut(player + 1);
        RollEffect?.MaxOut(player + 1);
        Dancer?.MaxOut(player + 1);
        Mob?.MaxOut(player + 1);
    }

    public Background(int playerCount, bool rightSide)
    {
        var bg = Game.Skin.Value.Play_Background_Normal["Default"];
        Up = Game.Skin.Assets.Play_BG_Up[bg.Up[Random.Shared.Next(0, bg.Up.Length)]];
        Up.SetPlayerCount(playerCount);
        Up.SetP1IsBlue(rightSide);
        Up.Init();

        RollEffect = Game.Skin.Assets.Play_BG_RollEffect[bg.RollEffect[Random.Shared.Next(0, bg.RollEffect.Length)]];
        RollEffect.SetPlayerCount(playerCount);
        RollEffect.SetP1IsBlue(rightSide);
        RollEffect.Init();

        if (playerCount == 1)
        {
            Down = Game.Skin.Assets.Play_BG_Down[bg.Down[Random.Shared.Next(0, bg.Down.Length)]];
            Down.Init();

            Down_Clear = Game.Skin.Assets.Play_BG_Down_Clear[bg.Down_Clear[Random.Shared.Next(0, bg.Down_Clear.Length)]];
            Down_Clear.Init();

            Dancer = Game.Skin.Assets.Play_BG_Dancer[bg.Dancer[Random.Shared.Next(0, bg.Dancer.Length)]];
            Dancer.Init();

            Footer = Game.Skin.Assets.Play_BG_Footer[bg.Footer[Random.Shared.Next(0, bg.Footer.Length)]];

            Mob = Game.Skin.Assets.Play_BG_Mob[bg.Mob[Random.Shared.Next(0, bg.Mob.Length)]];
            Mob.Init();
        }
    }

    public void Update()
    {
        Up?.Update();
        Down?.Update();
        Down_Clear?.Update();

        RollEffect?.Update();

        Dancer?.Update();
        Mob?.Update();
    }

    public void Draw()
    {
        Up?.Draw();
        Down?.Draw();
        Down_Clear?.Draw();

        RollEffect?.Draw();

        Dancer?.Draw();
        Footer?.Draw(0, GameEngine.Resolution.Y, drawOriginType:DrawOriginType.Left_Down);
        Mob?.Draw();
    }
}