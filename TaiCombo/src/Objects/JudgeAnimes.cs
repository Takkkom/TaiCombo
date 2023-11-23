using System.Drawing;
using System.Net.Mail;
using Silk.NET.Maths;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Plugin.Chart;

namespace TaiCombo.Objects;

class JudgeAnimes
{
    public class JudgeInfo
    {
        public JudgeType JudgeType;

        public float Value;
    }

    private int Player;

    private List<JudgeInfo> JudgeInfos = new();

    public void AddJudge(JudgeType judgeType)
    {
        JudgeInfo flash = new()
        {
            JudgeType = judgeType,
            Value = 0,
        };
        JudgeInfos.Add(flash);
    }

    public JudgeAnimes(int player)
    {
        Player = player;
    }

    public void Update()
    {
        for(int i = 0; i < JudgeInfos.Count; i++)
        {
            JudgeInfo effect = JudgeInfos[i];
            effect.Value += 4f * GameEngine.Time_.DeltaTime;

            if (effect.Value > 1)
            {
                JudgeInfos.Remove(effect);
            }
        }
    }

    public void Draw()
    {
        for(int i = 0; i < JudgeInfos.Count; i++)
        {
            JudgeInfo effect = JudgeInfos[i];
            float value = -1.0f + Math.Min(effect.Value * 3, 1);
            float opacity = 1.0f - Math.Max((effect.Value * 3) - 2.0f, 0);
            float x = Game.Skin.Value.Play_Judge.Pos[Player].X + (value * Game.Skin.Value.Play_Judge.MoveX);
            float y = Game.Skin.Value.Play_Judge.Pos[Player].Y + (value * Game.Skin.Value.Play_Judge.MoveY);


            
            switch(effect.JudgeType)
            {
                case JudgeType.Perfect:
                Game.Skin.Assets.Play_Judge_Perfect.Draw(x, y, color:new Color4(1, 1, 1, opacity), drawOriginType:DrawOriginType.Center);
                break;
                case JudgeType.Ok:
                Game.Skin.Assets.Play_Judge_Ok.Draw(x, y, color:new Color4(1, 1, 1, opacity), drawOriginType:DrawOriginType.Center);
                break;
                case JudgeType.Miss:
                Game.Skin.Assets.Play_Judge_Miss.Draw(x, y, color:new Color4(1, 1, 1, opacity), drawOriginType:DrawOriginType.Center);
                break;
            }
        }
    }
}