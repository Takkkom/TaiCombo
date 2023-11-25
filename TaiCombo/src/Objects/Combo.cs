using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Helper;

namespace TaiCombo.Objects;

class Combo
{
    private int Player;
    private int TaikoSide;

    private int Number;

    private float DrawCounter;

    private float OpenCounter;

    private float CloseCounter;

    private enum States
    {
        None,
        Open,
        Close,
        Draw
    }

    private States CurrentState;

    public void Open(int number)
    {
        CurrentState = States.Open;
        Number = number;

        OpenCounter = 0;
        CloseCounter = 0;
        DrawCounter = 0;
    }

    private void Close()
    {
        CurrentState = States.Close;
    }

    public Combo(int player, int taikoSide)
    {
        Player = player;
        TaikoSide = taikoSide;
    }

    public void Update()
    {
        /*
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.D))
        {
            Open(100);
        }
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.F))
        {
            Open(1000);
        }
        */

        
        switch(CurrentState)
        {
            case States.Draw:
            {
                if (DrawCounter < 1)
                {
                    DrawCounter += 0.7f * GameEngine.Time_.DeltaTime;
                }
                else 
                {
                    Close();
                }
            }
            break;
            case States.Open:
            {
                if (OpenCounter < 1)
                {
                    OpenCounter += 6.0f * GameEngine.Time_.DeltaTime;
                }
                else 
                {
                    CurrentState = States.Draw;
                }
            }
            break;
            case States.Close:
            {
                if (CloseCounter < 1)
                {
                    CloseCounter += 7.0f * GameEngine.Time_.DeltaTime;
                }
                else 
                {
                    CurrentState = States.None;
                }
            }
            break;
        }
    }

    public void Draw()
    {
        int x = Game.Skin.Value.Play_Combo_Base.Pos[Player].X;
        int y = Game.Skin.Value.Play_Combo_Base.Pos[Player].Y;
        float scale = Number >= 1000 ? 0.85f : 1;
        var combo_Number = Number >= 1000 ? Game.Skin.Value.Play_Combo_Number_1000 : Game.Skin.Value.Play_Combo_Number;
        var comboText = Number >= 1000 ? Game.Skin.Value.Play_Combo_Text_1000[Player] : Game.Skin.Value.Play_Combo_Text[Player];
        switch(CurrentState)
        {
            case States.Draw:
            {
                RectangleF rectangle = new(Game.Skin.Value.Play_Combo_Base.Width * 2, 0, Game.Skin.Value.Play_Combo_Base.Width, Game.Skin.Value.Play_Combo_Base.Height);
                Game.Skin.Assets.Play_Combo_Base[TaikoSide].Draw(x, y, rectangle:rectangle);

                NumHelper.DrawNumber(Number, combo_Number.Pos[Player].X, combo_Number.Pos[Player].Y, 
                combo_Number.Width, combo_Number.Height, combo_Number.Padding * scale,
                scale, 1, Game.Skin.Assets.Play_Combo_Number[TaikoSide]);

                Game.Skin.Assets.Play_Combo_Text.Draw(
                    comboText.X, comboText.Y, scale, 1.0f, drawOriginType:Engine.Enums.DrawOriginType.Center);
            }
            break;
            case States.Open:
            {
                float opacity = Math.Min(OpenCounter * 1.6f, 1);
                int frame = OpenCounter > 0.75f ? 1 : 0;
                RectangleF rectangle = new(Game.Skin.Value.Play_Combo_Base.Width * frame, 0, Game.Skin.Value.Play_Combo_Base.Width, Game.Skin.Value.Play_Combo_Base.Height);
                Game.Skin.Assets.Play_Combo_Base[TaikoSide].Draw(x, y, rectangle:rectangle, color:new Color4(1, 1, 1, opacity));

                float numOpacity = Math.Max((OpenCounter - 0.75f) * 4, 0);
                float move = (1 - numOpacity) * 7;
                NumHelper.DrawNumber(Number, combo_Number.Pos[Player].X + move, combo_Number.Pos[Player].Y, 
                combo_Number.Width, combo_Number.Height, combo_Number.Padding * scale,
                scale, 1, Game.Skin.Assets.Play_Combo_Number[TaikoSide], new Color4(1, 1, 1, numOpacity));

                Game.Skin.Assets.Play_Combo_Text.Draw(
                    comboText.X + move, comboText.Y, scale, 1.0f, color:new Color4(1, 1, 1, numOpacity), drawOriginType:Engine.Enums.DrawOriginType.Center);

            }
            break;
            case States.Close:
            {
                float opacity = CloseCounter > 0.8f ? 1 - ((CloseCounter - 0.8f) * 5) : 1;
                int frame = CloseCounter > 0.25f ? (CloseCounter > 0.8f ? 0 : 1) : 2;
                RectangleF rectangle = new(Game.Skin.Value.Play_Combo_Base.Width * frame, 0, Game.Skin.Value.Play_Combo_Base.Width, Game.Skin.Value.Play_Combo_Base.Height);
                Game.Skin.Assets.Play_Combo_Base[TaikoSide].Draw(x, y, rectangle:rectangle, color:new Color4(1, 1, 1, opacity));

                float numOpacity = 1 - (CloseCounter * 1.6f);
                NumHelper.DrawNumber(Number, combo_Number.Pos[Player].X, combo_Number.Pos[Player].Y, 
                combo_Number.Width, combo_Number.Height, combo_Number.Padding * scale,
                scale, 1, Game.Skin.Assets.Play_Combo_Number[TaikoSide], new Color4(1, 1, 1, numOpacity));

                Game.Skin.Assets.Play_Combo_Text.Draw(
                    comboText.X, comboText.Y, scale, 1.0f, color:new Color4(1, 1, 1, numOpacity), drawOriginType:Engine.Enums.DrawOriginType.Center);
            }
            break;
        }
    }
}