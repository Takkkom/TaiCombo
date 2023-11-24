using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;

namespace TaiCombo.Objects;

class BigEffect
{
    private class BigEffectInfo
    {
        public float X;

        public float Y;

        public float Value;
    }

    private List<BigEffectInfo> FlyNoteInfos = new();

    private int Player;

    private const int MAXNOTECOUNT = 20;
    private const int FRAMECOUNT = 13;

    public void Add(float x, float y)
    {
        BigEffectInfo flyNoteInfo = new()
        {
            X = x + Game.Skin.Value.Play_BigEffect.Pos[Player].X,
            Y = y + Game.Skin.Value.Play_BigEffect.Pos[Player].Y,
            Value = 0
        };
        FlyNoteInfos.Add(flyNoteInfo);

        if (MAXNOTECOUNT <= FlyNoteInfos.Count)
        {
            FlyNoteInfos.RemoveAt(0);
        }
    }

    public BigEffect(int player)
    {
        Player = player;
    }

    public void Update()
    {
        for(int i = 0; i < FlyNoteInfos.Count; i++)
        {
            BigEffectInfo bigEffectInfo = FlyNoteInfos[i];
            bigEffectInfo.Value += 4f * GameEngine.Time_.DeltaTime;

            if (bigEffectInfo.Value > 1)
            {
                FlyNoteInfos.Remove(bigEffectInfo);
                continue;
            }
        }
    }

    public void Draw()
    {
        for(int i = 0; i < FlyNoteInfos.Count; i++)
        {
            BigEffectInfo bigEffectInfo = FlyNoteInfos[i];

            int frame = Math.Min((int)(bigEffectInfo.Value * (FRAMECOUNT - 1)), (FRAMECOUNT - 1));

            Game.Skin.Assets.Play_BigEffect.Draw(bigEffectInfo.X, bigEffectInfo.Y, rectangle:new RectangleF(Game.Skin.Value.Play_BigEffect.Width * frame, 0, Game.Skin.Value.Play_BigEffect.Width, Game.Skin.Value.Play_BigEffect.Height), blendType:BlendType.Add);
        }
    }
}