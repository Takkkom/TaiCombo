using System.Drawing;
using System.Net.Mail;
using Silk.NET.Maths;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Helper;
using TaiCombo.Plugin.Chart;

namespace TaiCombo.Objects;

class Gauge
{
    private int Player;
    private float Value;

    private Sprite Base;
    private Sprite Clear;
    private Sprite Flash;
    private Sprite Frame;
    private Sprite Edge;
    private Sprite AddEffect;
    private Sprite[] Rainbows;

    private int GaugeState;

    private float AddCounter;

    private float FlashCounter;

    private float RainbowCounter;

    private float SoulFlashCounter;

    private float SoulFireCounter;

    private int ClearLine;

    private int RainbowFrame;

    public Gauge(GaugeType gaugeType, int player, int taikoSide)
    {
        Player = player;
        if (player == 0)
        {
            if (taikoSide == 0)
            {
                Base = Game.Skin.Assets.Gauge_1P_Base[gaugeType];
                Clear = Game.Skin.Assets.Gauge_1P_Clear[gaugeType];
                Flash = Game.Skin.Assets.Gauge_1P_Flash[gaugeType];
            }
            else
            {
                Base = Game.Skin.Assets.Gauge_2P_Base[gaugeType];
                Clear = Game.Skin.Assets.Gauge_2P_Up_Clear[gaugeType];
                Flash = Game.Skin.Assets.Gauge_2P_Up_Flash[gaugeType];
            }
        }
        else
        {
            Base = Game.Skin.Assets.Gauge_2P_Base[gaugeType];
            Clear = Game.Skin.Assets.Gauge_2P_Down_Clear[gaugeType];
            Flash = Game.Skin.Assets.Gauge_2P_Down_Flash[gaugeType];
        }

        Rainbows = Game.Skin.Assets.Gauge_Rainbow[gaugeType];

        Frame = Game.Skin.Assets.Gauge_Frame[gaugeType];
        Edge = Game.Skin.Assets.Gauge_Edge[gaugeType];
        AddEffect = Game.Skin.Assets.Gauge_Add[taikoSide];
        
        ClearLine = (int)(GaugeHelper.ClearLine[gaugeType] / 2);
    }

    public void SetValue(float value)
    {
        Value = value;
        int gaugeState = (int)(value / 2);
        if (gaugeState > GaugeState)
        {
            AddCounter = 0;
        }
        GaugeState = gaugeState;
    }

    public void Update()
    {
        if (AddCounter < 1)
        {
            AddCounter += 2 * GameEngine.Time_.DeltaTime;
        }
        else
        {
            AddCounter = 1;
        }

        FlashCounter += 10 * GameEngine.Time_.DeltaTime;
        if (FlashCounter >= 5)
        {
            FlashCounter = 0;
        }

        RainbowCounter += 1.5f * GameEngine.Time_.DeltaTime;
        if (RainbowCounter >= 1)
        {
            RainbowCounter = 0;
        }

        RainbowFrame = (int)(RainbowCounter * Rainbows.Length);

        SoulFlashCounter += 30.0f * GameEngine.Time_.DeltaTime;
        if (SoulFlashCounter >= 2)
        {
            SoulFlashCounter = 0;
        }

        SoulFireCounter += 2.5f * GameEngine.Time_.DeltaTime;
        if (SoulFireCounter >= 1)
        {
            SoulFireCounter = 0;
        }

    }

    public void Draw(int x, int y, float scale)
    {
        Edge.Draw(x + (Game.Skin.Value.Gauge_Edge.X * scale), y + (Game.Skin.Value.Gauge_Edge.Y * scale), scaleX:scale, scaleY:scale, flipY:Player == 1);
        Base.Draw(x, y, scaleX:scale, scaleY:scale, flipY:Player == 1);
        Clear.Draw(x, y, scaleX:scale, scaleY:scale, rectangle:new RectangleF(0, 0, Game.Skin.Value.Play_Gauge.Width * GaugeState, Game.Skin.Value.Play_Gauge.Height));
        
        if (GaugeState >= ClearLine)
        {
            float flashOpacity = FlashCounter < 1 ? FlashCounter : 2 - FlashCounter;
            Flash.Draw(x, y, scaleX:scale, scaleY:scale, color:new Color4(1, 1, 1, flashOpacity));
        }

        if (Value >= 100)
        {
            int nextFrame = (RainbowFrame + 1) % Rainbows.Length;
            Rainbows[nextFrame].Draw(x, y, flipY:Player == 1);

            float prevOpacity = 1 - ((RainbowCounter * Rainbows.Length) - RainbowFrame);
            Rainbows[RainbowFrame].Draw(x, y, flipY:Player == 1, color:new Color4(1, 1, 1, prevOpacity));
        }

        Frame.Draw(x, y, scaleX:scale, scaleY:scale, flipY:Player == 1, color:new Color4(1, 1, 1, 0.3f));

        float opacity = 1 - AddCounter;

        int addPos = GaugeState - 1;
        if (GaugeState < ClearLine)
        {
            AddEffect.Draw(x + (Game.Skin.Value.Gauge_Add.Pos.X * scale) + (addPos * Game.Skin.Value.Gauge_Add.Padding), y + (Game.Skin.Value.Gauge_Add.Pos.Y * scale), color:new(1, 1, 1, opacity), rectangle:new RectangleF(0, 0, Game.Skin.Value.Gauge_Add.Width, Game.Skin.Value.Gauge_Add.Height), flipY:Player == 1, blendType:BlendType.Add);
        }
        else if (GaugeState == ClearLine)
        {
            AddEffect.Draw(x + (Game.Skin.Value.Gauge_Add.Pos.X * scale) + (addPos * Game.Skin.Value.Gauge_Add.Padding), y + (Game.Skin.Value.Gauge_Add.Pos.Y * scale), color:new(1, 1, 1, opacity), rectangle:new RectangleF(Game.Skin.Value.Gauge_Add.Width, 0, Game.Skin.Value.Gauge_Add.Width, Game.Skin.Value.Gauge_Add.Height), flipY:Player == 1, blendType:BlendType.Add);
        }
        else if (GaugeState > ClearLine)
        {
            AddEffect.Draw(x + (Game.Skin.Value.Gauge_Add.Pos.X * scale) + (addPos * Game.Skin.Value.Gauge_Add.Padding), y + (Game.Skin.Value.Gauge_Add.Pos.Y * scale), color:new(1, 1, 1, opacity), rectangle:new RectangleF(Game.Skin.Value.Gauge_Add.Width * 2, 0, Game.Skin.Value.Gauge_Add.Width, Game.Skin.Value.Gauge_Add.Height), flipY:Player == 1, blendType:BlendType.Add);
        }

        if (GaugeState == 50)
        {
            Game.Skin.Assets.Gauge_SoulFire.Draw(x + Game.Skin.Value.Gauge_SoulFire.Pos[Player].X, y + Game.Skin.Value.Gauge_SoulFire.Pos[Player].Y, 
            rectangle:new RectangleF(Game.Skin.Value.Gauge_SoulFire.Width * (int)(SoulFireCounter * 8), 0, Game.Skin.Value.Gauge_SoulFire.Width, Game.Skin.Value.Gauge_SoulFire.Height));
        }

        if (GaugeState < ClearLine)
        {
            Game.Skin.Assets.Gauge_ClearText.Draw(x + Game.Skin.Value.Gauge_ClearText.Pos[Player].X + (Game.Skin.Value.Gauge_ClearText.Padding * (ClearLine - 1)), y + Game.Skin.Value.Gauge_ClearText.Pos[Player].Y, rectangle:new RectangleF(0, 0, Game.Skin.Value.Gauge_ClearText.Width, Game.Skin.Value.Gauge_ClearText.Height));
            
            Game.Skin.Assets.Gauge_SoulText.Draw(x + Game.Skin.Value.Gauge_SoulText.Pos[Player].X, y + Game.Skin.Value.Gauge_SoulText.Pos[Player].Y, 
            rectangle:new RectangleF(0, 0, Game.Skin.Value.Gauge_SoulText.Width, Game.Skin.Value.Gauge_SoulText.Height));
        }
        else
        {
            Game.Skin.Assets.Gauge_ClearText.Draw(x + Game.Skin.Value.Gauge_ClearText.Pos[Player].X + (Game.Skin.Value.Gauge_ClearText.Padding * (ClearLine - 1)), y + Game.Skin.Value.Gauge_ClearText.Pos[Player].Y, rectangle:new RectangleF(0, Game.Skin.Value.Gauge_ClearText.Height, Game.Skin.Value.Gauge_ClearText.Width, Game.Skin.Value.Gauge_ClearText.Height));
            
            Game.Skin.Assets.Gauge_SoulText.Draw(x + Game.Skin.Value.Gauge_SoulText.Pos[Player].X, y + Game.Skin.Value.Gauge_SoulText.Pos[Player].Y, 
            rectangle:new RectangleF(Game.Skin.Value.Gauge_SoulText.Width, 0, Game.Skin.Value.Gauge_SoulText.Width, Game.Skin.Value.Gauge_SoulText.Height));
        }

        if (GaugeState == 50 && SoulFlashCounter >= 1)
        {
            Game.Skin.Assets.Gauge_SoulText.Draw(x + Game.Skin.Value.Gauge_SoulText.Pos[Player].X, y + Game.Skin.Value.Gauge_SoulText.Pos[Player].Y, 
            rectangle:new RectangleF(Game.Skin.Value.Gauge_SoulText.Width * 2, 0, Game.Skin.Value.Gauge_SoulText.Width, Game.Skin.Value.Gauge_SoulText.Height));
        }
    }
}