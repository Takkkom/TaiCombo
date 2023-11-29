using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Plugin.Chart;

namespace TaiCombo.Helper;

static class NumHelper
{
    public static int[] SeparateDigits(int num)
    {
		int[] digits = new int[num.ToString().Length];
		for (int i = 0; i < digits.Length; i++)
		{
			digits[i] = num % 10;
			num /= 10;
		}
		return digits;
    }

    public static void DrawCombo(int num, float x, float y, int width, int height, float padding, float scaleX, float scaleY, Sprite sprite, float shineAnime)
    {
        int[] nums = SeparateDigits(num);
        
        for(int i = 0; i < nums.Length; i++)
        {
            RectangleF rectangle = new RectangleF(width * nums[i], 0, width, height);
            float currnetX = x - (padding * i) + (padding * ((nums.Length - 1) / 2.0f));
            float currnetY = y;
            sprite.Draw(currnetX, currnetY - ((Game.Skin.Value.Play_Taiko_Combo.Height / 2) * (scaleY - 1)), scaleX:scaleX, scaleY:scaleY, rectangle:rectangle, drawOriginType:Engine.Enums.DrawOriginType.Center);


            void drawShine(float x, float y, float shineValue)
            {
                shineValue *= 4;
                if (shineValue < 0 || shineValue > 1) return;

                float opacity = MathF.Sin(shineValue * MathF.PI);
                Game.Skin.Assets.Play_Taiko_Shine.Draw(x + (shineValue * Game.Skin.Value.Combo_Shine.MoveX), y + (shineValue * Game.Skin.Value.Combo_Shine.MoveY), color:new Color4(1, 1, 1, opacity), blendType:Engine.Enums.BlendType.Add, drawOriginType:Engine.Enums.DrawOriginType.Center);
            }

            if (shineAnime != -1)
            {
                for(int j = 0; j < Game.Skin.Value.Combo_Shine.Pos.Length; j++)
                {
                    drawShine(currnetX + Game.Skin.Value.Combo_Shine.Pos[j].X, currnetY + Game.Skin.Value.Combo_Shine.Pos[j].Y, shineAnime - (0.1f * j));
                }
            }
        }
    }

    public static void DrawResultNumber(int num, float x, float y, int width, int height, float padding, float scaleX, float scaleY, Sprite sprite, Color4? color = null)
    {
        int[] nums = SeparateDigits(num);
        
        for(int i = 0; i < nums.Length; i++)
        {
            RectangleF rectangle = new RectangleF(width * nums[i], 0, width, height);
            float currnetX = x - (padding * i);
            float currnetY = y;
            sprite.Draw(currnetX, currnetY, scaleX:scaleX, scaleY:scaleY, rectangle:rectangle, drawOriginType:Engine.Enums.DrawOriginType.Center, color:color);
        }
    }

    public static void DrawNumber(int num, float x, float y, int width, int height, float padding, float scaleX, float scaleY, Sprite sprite, Color4? color = null)
    {
        int[] nums = SeparateDigits(num);
        
        for(int i = 0; i < nums.Length; i++)
        {
            RectangleF rectangle = new RectangleF(width * nums[i], 0, width, height);
            float currnetX = x - (padding * i) + (padding * ((nums.Length - 1) / 2.0f));
            float currnetY = y;
            sprite.Draw(currnetX, currnetY, scaleX:scaleX, scaleY:scaleY, rectangle:rectangle, drawOriginType:Engine.Enums.DrawOriginType.Center, color:color);
        }
    }

    public static void DrawScoreNumber(int num, float x, float y, int width, int height, float scaleX, float scaleY, float padding, float outValue, float moveSizeY, Sprite sprite)
    {

        int[] nums = SeparateDigits(num);
        
        for(int i = 0; i < nums.Length; i++)
        {
            RectangleF rectangle = new RectangleF(width * nums[i], 0, width, height);
            float currnetX = x - (padding * i);
            float value = outValue * (1.0f / (1.0f + (i / 5.0f)));
            float currnetY = y - (moveSizeY * MathF.Sin(value * 3 * MathF.PI));
            float opacity = 1.0f - ((value * 4) - 1.0f);
            sprite.Draw(currnetX, currnetY, scaleX:scaleX, scaleY:scaleY, rectangle:rectangle, color:new Color4(1.0f, 1.0f, 1.0f, opacity), drawOriginType:Engine.Enums.DrawOriginType.Center);
        }
    }

    public static float GetNumJumpScale(float value)
    {
        float numScale;
        if (value < 0.125)
        {
            numScale = value * 8;
        }
        else 
        {
            numScale = 1.0f - value;
        }
        numScale /= 4.0f;
        numScale += 1;

        return numScale;
    }
}