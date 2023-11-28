using Silk.NET.Maths;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Helpers;
using TaiCombo.Engine.Struct;
using TaiCombo.Helper;

namespace TaiCombo.Objects;

class Balloon
{
    private int Player;

    public bool Opend;

    private float NumberJump;

    private int Number;

    private float BrokeCounter;

    private float MissCounter;

    private Matrix4X4<float> Balloon_Pos;

    private enum States
    {
        None,
        Breaking,
        Broke,
        Miss
    }

    private States CurrentState;

    public void Open()
    {
        Opend = true;
        CurrentState = States.Breaking;
    }

    public void Broke()
    {
        Opend = false;
        BrokeCounter = 0;
        CurrentState = States.Broke;
    }

    public void Miss()
    {
        Opend = false;
        MissCounter = 0;
        CurrentState = States.Miss;
    }

    public void SetNumber(int num)
    {
        NumberJump = 0;
        Number = num;
    }

    public Balloon(int player)
    {
        Player = player;
        Balloon_Pos = MatrixHelper.Get2DMatrix(Game.Skin.Value.Play_Balloon_Base[player].X, Game.Skin.Value.Play_Balloon_Base[player].Y, 1, 1, Game.Skin.Assets.Play_Balloon_Base.DefaultRect, false, false, 0, DrawOriginType.Left_Up);
    }

    public void Update()
    {
        NumberJump = MathF.Min(NumberJump + (7.0f * GameEngine.Time_.DeltaTime), 1);
        
        switch(CurrentState)
        {
            case States.Breaking:
            {
            }
            break;
            case States.Broke:
            {
                if (BrokeCounter < 1)
                {
                    BrokeCounter += 7.0f * GameEngine.Time_.DeltaTime;
                }
                else 
                {
                    BrokeCounter = 1;
                }
            }
            break;
            case States.Miss:
            {
                if (MissCounter < 1)
                {
                    MissCounter += 2.0f * GameEngine.Time_.DeltaTime;
                }
                else 
                {
                    MissCounter = 1;
                    CurrentState = States.None;
                }
            }
            break;
        }
    }

    public void Draw()
    {
        switch(CurrentState)
        {
            case States.Breaking:
            {
                Game.Skin.Assets.Play_Balloon_Base.Draw(Balloon_Pos);
                    
                float numScale = NumHelper.GetNumJumpScale(NumberJump);

                NumHelper.DrawNumber(Number, Game.Skin.Value.Play_Balloon_Number.Pos[Player].X, Game.Skin.Value.Play_Balloon_Number.Pos[Player].Y - ((Game.Skin.Value.Play_Taiko_Combo.Height / 2) * (numScale - 1)), 
                Game.Skin.Value.Play_Balloon_Number.Width, Game.Skin.Value.Play_Balloon_Number.Height, Game.Skin.Value.Play_Balloon_Number.Padding, 
                1.0f, numScale, Game.Skin.Assets.Play_Balloon_Number);

                int breaking_x = Game.Skin.Value.Play_Balloon_Breaking[Player].X;
                Game.Skin.Assets.Play_Balloon_Breaking[0].Draw(breaking_x, Game.Skin.Value.Play_Balloon_Breaking[Player].Y);
            }
            break;
            case States.Broke:
            {
                Game.Skin.Assets.Play_Balloon_Breaking[5].Draw(Game.Skin.Value.Play_Balloon_Breaking[Player].X, Game.Skin.Value.Play_Balloon_Breaking[Player].Y, color:new Color4(1, 1, 1, 1 - BrokeCounter));
            }
            break;
            case States.Miss:
            {
                if (MissCounter < 0.15f)
                {
                    Game.Skin.Assets.Play_Balloon_Breaking[4 - Math.Min((int)(MissCounter * 40.0f), 4)].Draw(Game.Skin.Value.Play_Balloon_Breaking[Player].X, Game.Skin.Value.Play_Balloon_Breaking[Player].Y);
                }
                else 
                {
                    float baseValue = Math.Min(((MissCounter - 0.15f) / (1 - 0.15f)) * 1.2f, 1);
                    float value;
                    float x;
                    float y;

                    float move1 = Game.Skin.Value.Play_Balloon_Miss.MoveY;
                    float move2 = Game.Skin.Value.Play_Balloon_Miss.MoveY / 2.0f;

                    if (baseValue < 0.0f + 0.333f)
                    {
                        value = baseValue;
                        y = Game.Skin.Value.Play_Balloon_Miss.Pos[Player].Y - ((MathF.Cos(value / 0.333f / 2.0f * MathF.PI) - 1) * move1);
                    }
                    else if (baseValue < 0.333f + 0.222f)
                    {
                        float yValue = (baseValue - 0.333f) / 0.222f;
                        value = 0.333f + (yValue * 0.333f);
                        y = Game.Skin.Value.Play_Balloon_Miss.Pos[Player].Y + move1 - (MathF.Sin(yValue * MathF.PI) * move2);
                    }
                    else
                    {
                        float yValue = (baseValue - 0.555f) / 0.445f;
                        value = 0.666f + (yValue * 0.333f);
                        y = Game.Skin.Value.Play_Balloon_Miss.Pos[Player].Y + move1 - (MathF.Sin(yValue * MathF.PI) * move2);
                    }
                    x = Game.Skin.Value.Play_Balloon_Miss.Pos[Player].X + (value * Game.Skin.Value.Play_Balloon_Miss.MoveX);
                    float rotation = -value * MathF.PI;


                    Game.Skin.Assets.Play_Balloon_Miss.Draw(x, y, rotation:rotation, drawOriginType:DrawOriginType.Center);
                }
            }
            break;
        }
        

    }
}