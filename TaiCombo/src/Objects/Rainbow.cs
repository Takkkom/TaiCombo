using TaiCombo.Common;
using TaiCombo.Engine;

namespace TaiCombo.Objects;

class Rainbow
{
    private int Player;

    private float IdleCounter;
    private float InCounter;
    private float OutCounter;

    private enum States
    {
        None,
        In,
        Idle,
        Out
    }

    private States CurrentState;

    public void Open()
    {
        CurrentState = States.In;
        IdleCounter = 0;
        InCounter = 0;
        OutCounter = 0;
    }

    public Rainbow(int player)
    {
        Player = player;
    }

    public void Update()
    {
        switch(CurrentState)
        {
            case States.Idle:
            {
                if (IdleCounter < 1)
                {
                    IdleCounter += 6.0f * GameEngine.Time_.DeltaTime;
                }
                else 
                {
                    CurrentState = States.Out;
                }
            }
            break;
            case States.In:
            {
                if (InCounter < 1)
                {
                    InCounter += 2f * GameEngine.Time_.DeltaTime;
                }
                else 
                {
                    CurrentState = States.Idle;
                }
            }
            break;
            case States.Out:
            {
                if (OutCounter < 1)
                {
                    OutCounter += 4.0f * GameEngine.Time_.DeltaTime;
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
        switch(CurrentState)
        {
            case States.Idle:
            {
                Game.Skin.Assets.Play_Balloon_Rainbow.Draw(Game.Skin.Value.Play_Balloon_Rainbow.Pos[Player].X, Game.Skin.Value.Play_Balloon_Rainbow.Pos[Player].Y, 
                flipY:Player == 1);
            }
            break;
            case States.In:
            {
                Game.Skin.Assets.Play_Balloon_Rainbow.Draw(Game.Skin.Value.Play_Balloon_Rainbow.Pos[Player].X, Game.Skin.Value.Play_Balloon_Rainbow.Pos[Player].Y, 
                flipY:Player == 1, rectangle: new(0, 0, Game.Skin.Assets.Play_Balloon_Rainbow.TextureSize.Width * InCounter, Game.Skin.Assets.Play_Balloon_Rainbow.TextureSize.Height));
            }
            break;
            case States.Out:
            {
                float move = Game.Skin.Assets.Play_Balloon_Rainbow.TextureSize.Width * OutCounter;
                float width = Game.Skin.Assets.Play_Balloon_Rainbow.TextureSize.Width * (1 - OutCounter);
                Game.Skin.Assets.Play_Balloon_Rainbow.Draw(Game.Skin.Value.Play_Balloon_Rainbow.Pos[Player].X + move, Game.Skin.Value.Play_Balloon_Rainbow.Pos[Player].Y, 
                flipY:Player == 1, rectangle: new(move, 0, width, Game.Skin.Assets.Play_Balloon_Rainbow.TextureSize.Height));
            }
            break;
        }
        

    }
}