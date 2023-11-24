using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Struct;

namespace TaiCombo.Objects;

class HitExplosion
{
    public class EffectInfo
    {
        public int Type;

        public float Value;
    }

    private int Player;

    private List<EffectInfo> Effects = new();

    public void AddEffect(int type)
    {
        EffectInfo flash = new()
        {
            Type = type,
            Value = 0,
        };
        Effects.Add(flash);
    }

    public HitExplosion(int player)
    {
        Player = player;
    }

    public void Update()
    {
        for(int i = 0; i < Effects.Count; i++)
        {
            EffectInfo effect = Effects[i];
            effect.Value += 7f * GameEngine.Time_.DeltaTime;

            if (effect.Value > 2.5f)
            {
                Effects.Remove(effect);
            }
        }
    }

    public void DrawBeforTaikoBG()
    {
        int x = Game.Skin.Value.Play_HitExplosion.Pos[Player].X;
        int y = Game.Skin.Value.Play_HitExplosion.Pos[Player].Y;
        int width = Game.Skin.Value.Play_HitExplosion.Width;
        int height = Game.Skin.Value.Play_HitExplosion.Height;
        for(int i = 0; i < Effects.Count; i++)
        {
            EffectInfo effect = Effects[i];
            float opacity = Math.Min((2 - (effect.Value - 0.25f)) / 1.5f, 1);
            RectangleF rectangle = new RectangleF(0, height * effect.Type, width, height);

            Game.Skin.Assets.Play_HitExplosion_Notes.Draw(x, y, rectangle:rectangle, color:new Color4(1, 1, 1, opacity), drawOriginType:DrawOriginType.Center, blendType:BlendType.Normal);
        }
    }

    public void DrawAfterTaikoBG()
    {
        int x = Game.Skin.Value.Play_HitExplosion.Pos[Player].X;
        int y = Game.Skin.Value.Play_HitExplosion.Pos[Player].Y;
        int width = Game.Skin.Value.Play_HitExplosion.Width;
        int height = Game.Skin.Value.Play_HitExplosion.Height;
        for(int i = 0; i < Effects.Count; i++)
        {
            EffectInfo effect = Effects[i];
            RectangleF rectangle;
            float opacity;
            float scale;
            if (effect.Type == 0 || effect.Type == 1)
            {
                scale = 1.0f;
                rectangle = new RectangleF(width * Math.Min((int)(effect.Value * 7), 3), height * effect.Type, width, height);
                opacity = Math.Min((1 - effect.Value) * 4, 1);
            }
            else
            {
                scale = 0.5f + (MathF.Sin(Math.Min(effect.Value * 4, 1) * MathF.PI / 2.0f) / 2.0f);
                rectangle = new RectangleF(width * Math.Min((int)(effect.Value * 3), 3), height * effect.Type, width, height);
                opacity = Math.Min((1.5f - effect.Value) * 1.5f, 1);
            }
            Game.Skin.Assets.Play_HitExplosion_Firework.Draw(x, y, scaleX:scale, scaleY:scale, rectangle:rectangle, color:new Color4(1, 1, 1, opacity), drawOriginType:DrawOriginType.Center, blendType:BlendType.Add);

            float big_value = effect.Value;
            float big2_value = Math.Max(effect.Value - 1f, 0) / 1.5f;

            float big_scale = 0.5f + (MathF.Sin(MathF.Sin(Math.Min(big_value, 1) * MathF.PI / 2.0f) * MathF.PI / 2.0f) / 1.5f);
            float big2_scale = 0.5f + (MathF.Sin(MathF.Sin(Math.Min(big2_value, 1) * MathF.PI / 2.0f) * MathF.PI / 2.0f) / 2f);

            float big_opacity = Math.Min((1f - big_value) * 3, 1);
            float big2_opacity = Math.Min((0.75f - big2_value) * 1.5f, 1);

            switch(effect.Type)
            {
                case 2:
                {
                    RectangleF big_rectangle = new RectangleF(0, 0, width, height);
                    Game.Skin.Assets.Play_HitExplosion_Firework_Big.Draw(x, y, scaleX:big_scale, scaleY:big_scale, color:new Color4(1, 1, 1, big_opacity), rectangle:big_rectangle, drawOriginType:DrawOriginType.Center);
                    if (big2_value > 0) Game.Skin.Assets.Play_HitExplosion_Firework_Big.Draw(x, y, scaleX:big2_scale, scaleY:big2_scale, color:new Color4(1, 1, 1, big2_opacity), rectangle:big_rectangle, drawOriginType:DrawOriginType.Center);
                }
                break;
                case 3:
                {
                    RectangleF big_rectangle = new RectangleF(0, height, width, height);
                    Game.Skin.Assets.Play_HitExplosion_Firework_Big.Draw(x, y, scaleX:big_scale, scaleY:big_scale, color:new Color4(1, 1, 1, big_opacity), rectangle:big_rectangle, drawOriginType:DrawOriginType.Center);
                    if (big2_value > 0) Game.Skin.Assets.Play_HitExplosion_Firework_Big.Draw(x, y, scaleX:big2_scale, scaleY:big2_scale, color:new Color4(1, 1, 1, big2_opacity), rectangle:big_rectangle, drawOriginType:DrawOriginType.Center);
                }
                break;
            }
        }
    }
}