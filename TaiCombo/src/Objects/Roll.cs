using System.Drawing;
using Silk.NET.Maths;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Helper;
using TaiCombo.Plugin.Chart;

namespace TaiCombo.Objects;

class Roll
{
    private int Player;

    private float OpenCounter;
    private float CloseCounter = 1;

    private int BaseFrame;

    public bool Opend;

    private float NumberJump;

    private int Number;

    public void Open(bool force)
    {
        if (!force && Opend) return;
        Opend = true;
        OpenCounter = -0.1f;
    }

    public void Close()
    {
        Opend = false;
        CloseCounter = 0;
    }

    public void SetNumber(int num)
    {
        NumberJump = 0;
        Number = num;
    }

    public Roll(int player)
    {
        Player = player;
    }

    public void Update()
    {

        if (Opend)
        {
            if (OpenCounter < 1)
            {
                OpenCounter += 5.0f * GameEngine.Time_.DeltaTime;
            }
            else
            {
                OpenCounter = 1;
            }
            BaseFrame = Math.Min((int)(OpenCounter * 5), 4);
        }
        else 
        {
            if (CloseCounter < 1)
            {
                CloseCounter += 5.0f * GameEngine.Time_.DeltaTime;
            }
            else
            {
                CloseCounter = 1;
            }
            BaseFrame = 4 - Math.Min((int)(CloseCounter * 5), 4);
        }

        NumberJump = MathF.Min(NumberJump + (7.0f * GameEngine.Time_.DeltaTime), 1);
    }

    public void Draw()
    {
        Color4 color;
        Color4 numColor;
        if (Opend)
        {
            float opacity = 1 + Math.Min(OpenCounter * 10.0f, 0);
            color = new(1, 1, 1, opacity);
            float numOpacity = 1 + Math.Min((OpenCounter - 0.7f) * 3.0f, 0);
            numColor = new(1, 1, 1, numOpacity);
        }
        else 
        {
            float opacity = 1 - Math.Max((CloseCounter - 0.9f) * 10.0f, 0);
            color = new(1, 1, 1, opacity);
            float numOpacity = 1 - Math.Max(CloseCounter * 3.0f, 0);
            numColor = new(1, 1, 1, numOpacity);
        }

        float numScale = NumHelper.GetNumJumpScale(NumberJump);

        Game.Skin.Assets.Play_Roll_Base.Draw(Game.Skin.Value.Play_Roll_Base.Pos[Player].X, Game.Skin.Value.Play_Roll_Base.Pos[Player].Y,
        rectangle:new RectangleF(Game.Skin.Value.Play_Roll_Base.Width * BaseFrame, 0, Game.Skin.Value.Play_Roll_Base.Width, Game.Skin.Value.Play_Roll_Base.Height), color:color);
            
        NumHelper.DrawNumber(Number, Game.Skin.Value.Play_Roll_Number.Pos[Player].X, Game.Skin.Value.Play_Roll_Number.Pos[Player].Y - ((Game.Skin.Value.Play_Taiko_Combo.Height / 2) * (numScale - 1)), 
        Game.Skin.Value.Play_Roll_Number.Width, Game.Skin.Value.Play_Roll_Number.Height, Game.Skin.Value.Play_Roll_Number.Padding, 
        1.0f, numScale, Game.Skin.Assets.Play_Roll_Number, numColor);
    }
}