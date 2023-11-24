using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Helper;

namespace TaiCombo.Objects;

class ScoreRank
{
    private enum States
    {
        In,
        Out,
        Idle
    }

    private class ScoreRankInfo
    {
        public ScoreRankType ScoreRankType;

        public float DrawCounter;
        public float InCounter;
        public float OutCounter;

        public States State;
    }

    private List<ScoreRankInfo> ScoreRankInfos = new();

    private int Player;

    public void Add(ScoreRankType scoreRankType)
    {
        ScoreRankInfo flyNoteInfo = new()
        {
            ScoreRankType = scoreRankType,
            DrawCounter = 0,
            InCounter = 0,
            OutCounter = 0
        };
        ScoreRankInfos.Add(flyNoteInfo);
    }

    public ScoreRank(int player)
    {
        Player = player;
    }

    public void Update()
    {
        /*
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.D))
        {
            Add(ScoreRankType.Omega);
        }
        */

        for(int i = 0; i < ScoreRankInfos.Count; i++)
        {
            ScoreRankInfo scoreRankInfo = ScoreRankInfos[i];
            
            switch(scoreRankInfo.State)
            {
                case States.Idle:
                {
                    if (scoreRankInfo.DrawCounter < 1)
                    {
                        scoreRankInfo.DrawCounter += 0.47f * GameEngine.Time_.DeltaTime;
                    }
                    else 
                    {
                        scoreRankInfo.State = States.Out;
                    }
                }
                break;
                case States.In:
                {
                    if (scoreRankInfo.InCounter < 1)
                    {
                        scoreRankInfo.InCounter += 4.0f * GameEngine.Time_.DeltaTime;
                    }
                    else 
                    {
                        scoreRankInfo.State = States.Idle;
                    }
                }
                break;
                case States.Out:
                {
                    if (scoreRankInfo.OutCounter < 1)
                    {
                        scoreRankInfo.OutCounter += 4.0f * GameEngine.Time_.DeltaTime;
                    }
                    else 
                    {
                        ScoreRankInfos.Remove(scoreRankInfo);
                    }
                }
                break;
            }
        }
    }

    public void Draw()
    {
        for(int i = 0; i < ScoreRankInfos.Count; i++)
        {
            ScoreRankInfo scoreRankInfo = ScoreRankInfos[i];

            float x = Game.Skin.Value.Play_ScoreRank.Pos[Player].X;
            float y = Game.Skin.Value.Play_ScoreRank.Pos[Player].Y;
            float scale = 1;
            float opacity = 1;
            
            switch(scoreRankInfo.State)
            {
                case States.Idle:
                {
                    scale = 1 + (EasingHelper.BasicInOut(Math.Min(scoreRankInfo.DrawCounter * 14.0f, 1)) / 10.0f);
                }
                break;
                case States.In:
                {
                    y += Game.Skin.Value.Play_ScoreRank.Height * (1 - scoreRankInfo.InCounter) / 8.0f;
                    opacity = scoreRankInfo.InCounter;
                }
                break;
                case States.Out:
                {
                    y -= Game.Skin.Value.Play_ScoreRank.Height * scoreRankInfo.OutCounter / 8.0f;
                    opacity = 1 - scoreRankInfo.OutCounter;
                }
                break;
            }
            Game.Skin.Assets.Play_ScoreRank.Draw(x, y, scaleX:scale, scaleY:scale, color:new Color4(1, 1, 1, opacity), rectangle:new RectangleF(0, Game.Skin.Value.Play_ScoreRank.Height * (int)(scoreRankInfo.ScoreRankType - 1), Game.Skin.Value.Play_ScoreRank.Width, Game.Skin.Value.Play_ScoreRank.Height), drawOriginType:DrawOriginType.Center);
        }
    }
}