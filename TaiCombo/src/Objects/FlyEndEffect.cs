using System.Drawing;
using Microsoft.CodeAnalysis.CSharp.Scripting;
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

class FlyEndEffect
{
    private class FlyEndInfo
    {
        public FlyNoteType FlyNoteType;

        public float Value;
    }

    private List<FlyEndInfo> FlyEndInfos = new();

    private int Player;

    public void Add(FlyNoteType flyNoteType)
    {
        FlyEndInfo flyNoteInfo = new()
        {
            FlyNoteType = flyNoteType,
            Value = 0
        };
        FlyEndInfos.Add(flyNoteInfo);
    }

    public FlyEndEffect(int player)
    {
        Player = player;
    }

    public void Update()
    {
        for(int i = 0; i < FlyEndInfos.Count; i++)
        {
            FlyEndInfo flyEndInfo = FlyEndInfos[i];
            flyEndInfo.Value += (1f * GameEngine.Time_.DeltaTime);

            if (flyEndInfo.Value > 1)
            {
                FlyEndInfos.Remove(flyEndInfo);
                continue;
            }
        }
    }

    public void Draw()
    {
        for(int i = 0; i < FlyEndInfos.Count; i++)
        {
            FlyEndInfo flyEndInfo = FlyEndInfos[i];

            float x = Game.Skin.Value.Play_FlyNotes.End[Player].X;
            float y = Game.Skin.Value.Play_FlyNotes.End[Player].Y;

            float yellowOpacity;
            float whiteOpacity;

            bool showNote = flyEndInfo.Value < 0.15f;
            if (showNote)
            {
                yellowOpacity = flyEndInfo.Value / 0.15f;
                whiteOpacity = flyEndInfo.Value / 0.15f;
            }
            else
            {
                yellowOpacity = 0;
                whiteOpacity = 1 - ((flyEndInfo.Value - 0.15f)  / 0.15f);
            }

            Color4 yellowColor = new Color4(1, 1, 1, yellowOpacity);
            Color4 whiteColor = new Color4(1, 1, 1, whiteOpacity);

            switch(flyEndInfo.FlyNoteType)
            {
                case FlyNoteType.Don:
                if (showNote) 
                {
                    Game.Skin.Assets.Play_FlyNotes_Don.Draw(x, y);
                    Game.Skin.Assets.Play_FlyNotes_Flash_Yellow.Draw(x, y, color:yellowColor);
                }
                Game.Skin.Assets.Play_FlyNotes_Flash.Draw(x, y, color:whiteColor);
                break;
                case FlyNoteType.Ka:
                if (showNote) 
                {
                    Game.Skin.Assets.Play_FlyNotes_Ka.Draw(x, y);
                    Game.Skin.Assets.Play_FlyNotes_Flash_Yellow.Draw(x, y, color:yellowColor);
                }
                Game.Skin.Assets.Play_FlyNotes_Flash.Draw(x, y, color:whiteColor);
                break;
                case FlyNoteType.Don_Big:
                if (showNote) 
                {
                    Game.Skin.Assets.Play_FlyNotes_Don_Big.Draw(x, y);
                    Game.Skin.Assets.Play_FlyNotes_Flash_Yellow_Big.Draw(x, y, color:yellowColor);
                }
                Game.Skin.Assets.Play_FlyNotes_Flash_Big.Draw(x, y, color:whiteColor);
                break;
                case FlyNoteType.Ka_Big:
                if (showNote) 
                {
                    Game.Skin.Assets.Play_FlyNotes_Ka_Big.Draw(x, y);
                    Game.Skin.Assets.Play_FlyNotes_Flash_Yellow_Big.Draw(x, y, color:yellowColor);
                }
                Game.Skin.Assets.Play_FlyNotes_Flash_Big.Draw(x, y, color:whiteColor);
                break;
            }

            if (flyEndInfo.Value < 0.025f)
            {
                Game.Skin.Assets.Play_FlyNotes_Firework_Alt.Draw(Game.Skin.Value.Play_FlyEndEffect_Firework.Pos[Player].X, Game.Skin.Value.Play_FlyEndEffect_Firework.Pos[Player].Y, drawOriginType:DrawOriginType.Center);
            }
            else
            {
                float value = (flyEndInfo.Value - 0.025f);
                float rotation = -(value * 1.4f) * MathF.PI;
                float scale = 0.75f + value;

                Color3 beginColor = Game.Skin.Value.Play_FlyEndEffect_Firework.BeginColor[Player];
                Color3 endColor = Game.Skin.Value.Play_FlyEndEffect_Firework.EndColor[Player];

                float opacity = 1;
                if (value > 0.2f)
                {
                    float value2 = (value - 0.2f) * 6;
                    opacity = 1 - (value2 * 2);
                    rotation -= value2 / 4.0f * MathF.PI;
                    scale += value2;
                }

                float gradValue = Math.Min(value / 0.2f, 1);

                Color4 fireworkColor = new (beginColor.R + ((endColor.R - beginColor.R) * gradValue), 
                beginColor.G + ((endColor.G - beginColor.G) * gradValue), 
                beginColor.B + ((endColor.B - beginColor.B) * gradValue), opacity);

                Game.Skin.Assets.Play_FlyNotes_Firework.Draw(Game.Skin.Value.Play_FlyEndEffect_Firework.Pos[Player].X, Game.Skin.Value.Play_FlyEndEffect_Firework.Pos[Player].Y, color:fireworkColor, rotation:rotation, scaleX:scale, scaleY:scale, drawOriginType:DrawOriginType.Center);
            }
        }
    }
}