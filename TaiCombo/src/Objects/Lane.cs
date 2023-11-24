using System.Drawing;
using Silk.NET.Maths;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Helper;
using TaiCombo.Plugin.Chart;
using TaiCombo.Plugin.Enums;

namespace TaiCombo.Objects;

class Lane
{
    public class LaneFlashInfo
    {
        public int FlashType;

        public float Opacity;
    }

    private int Player;

    private List<LaneFlashInfo> Flashs = new();

    private bool GoGoTime;

    private float GoGoInCounter;

    private float GoGoOutCounter;

    private float GoGoFireCounter;

    private float GoGoFireInCounter;

    private float GoGoFireOutCounter;

    private float BranchCounter = 1;

    private BranchType PrevBranch;
    private BranchType CurrentBranch;

    private bool Branchable;

    public void AddFlash(int hitType)
    {
        LaneFlashInfo flash = new()
        {
            FlashType = hitType,
            Opacity = 1.5f,
        };
        Flashs.Add(flash);
    }

    public void GoGoIn()
    {
        GoGoTime = true;
        GoGoInCounter = 0;
        GoGoFireInCounter = 0;
    }

    public void GoGoOut()
    {
        GoGoTime = false;
        GoGoOutCounter = 0;
        GoGoFireOutCounter = 0;
    }

    public void Branch(BranchType branchType)
    {
        if (PrevBranch == branchType) return;
        PrevBranch = CurrentBranch;
        CurrentBranch = branchType;
        BranchCounter = 0;
    }

    public Lane(int player, bool branchable)
    {
        Player = player;
        Branchable = branchable;
    }

    public void Update()
    {
        /*
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.D))
        {
            Branch(BranchType.Master);
        }
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.F))
        {
            Branch(BranchType.Expert);
        }
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.J))
        {
            Branch(BranchType.Master);
        }
        */

        for(int i = 0; i < Flashs.Count; i++)
        {
            LaneFlashInfo flash = Flashs[i];

            flash.Opacity = Math.Max(flash.Opacity - (10.0f * GameEngine.Time_.DeltaTime), 0);

            if (flash.Opacity <= 0)
            {
                Flashs.Remove(flash);
            }
        }

        if (GoGoTime)
        {
            if (GoGoInCounter < 1)
            {
                GoGoInCounter += (10.0f * GameEngine.Time_.DeltaTime);
            }
            else 
            {
                GoGoInCounter = 1;
            }

            if (GoGoFireInCounter < 1)
            {
                GoGoFireInCounter += 4.0f * GameEngine.Time_.DeltaTime;
            }
            else 
            {
                GoGoFireInCounter = 1;
            }
        }
        else
        {
            if (GoGoOutCounter < 1)
            {
                GoGoOutCounter += 10.0f * GameEngine.Time_.DeltaTime;
            }
            else 
            {
                GoGoOutCounter = 1;
            }

            if (GoGoFireOutCounter < 1)
            {
                GoGoFireOutCounter += 10.0f * GameEngine.Time_.DeltaTime;
            }
            else 
            {
                GoGoFireOutCounter = 1;
            }
        }

        GoGoFireCounter += 4.5f * GameEngine.Time_.DeltaTime;
        if (GoGoFireCounter >= 1)
        {
            GoGoFireCounter = 0;
        }


        if (BranchCounter < 1)
        {
            BranchCounter += 3f * GameEngine.Time_.DeltaTime;
        }
        else 
        {
            BranchCounter = 1;
        }
    }

    public void Draw()
    {
        Game.Skin.Assets.Play_Lane_Frame.Draw(Game.Skin.Value.Play_Lane_Frame[Player].X, Game.Skin.Value.Play_Lane_Frame[Player].Y);
        Game.Skin.Assets.Play_Lane_Base_Main.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y);
        Game.Skin.Assets.Play_Lane_Base_Sub.Draw(Game.Skin.Value.Play_Lane_Sub[Player].X, Game.Skin.Value.Play_Lane_Sub[Player].Y);

        if (Branchable)
        {
            float prevOpacity = 1 - BranchCounter;
            Color4 prevColor = new Color4(1, 1, 1, prevOpacity);

            float textOut = EasingHelper.EaseInBack(BranchCounter * 1.1f);
            float textIn = 1.0f - EasingHelper.EaseOutBack((BranchCounter - 0.5f) * 2.0f);
            Color4 prevTextColor = new Color4(1, 1, 1, 1 - (textOut * 3.0f));
            Color4 curTextColor = new Color4(1, 1, 1, 1 - (textIn * 3.0f));
            float vector = CurrentBranch > PrevBranch ? 1 : -1;
            float prevTextY = textOut * -200.0f * vector;
            float curTextY = textIn * 200.0f * vector;

            switch(CurrentBranch)
            {
                case BranchType.Normal:
                Game.Skin.Assets.Play_Lane_Base_Normal.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y);
                Game.Skin.Assets.Play_Lane_Text_Normal.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y + curTextY, color:curTextColor);
                break;
                case BranchType.Expert:
                Game.Skin.Assets.Play_Lane_Base_Expert.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y);
                Game.Skin.Assets.Play_Lane_Text_Expert.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y + curTextY, color:curTextColor);
                break;
                case BranchType.Master:
                Game.Skin.Assets.Play_Lane_Base_Master.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y);
                Game.Skin.Assets.Play_Lane_Text_Master.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y + curTextY, color:curTextColor);
                break;
            }

            switch(PrevBranch)
            {
                case BranchType.Normal:
                Game.Skin.Assets.Play_Lane_Base_Normal.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y, color:prevColor);
                Game.Skin.Assets.Play_Lane_Text_Normal.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y + prevTextY, color:prevTextColor);
                break;
                case BranchType.Expert:
                Game.Skin.Assets.Play_Lane_Base_Expert.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y, color:prevColor);
                Game.Skin.Assets.Play_Lane_Text_Expert.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y + prevTextY, color:prevTextColor);
                break;
                case BranchType.Master:
                Game.Skin.Assets.Play_Lane_Base_Master.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y, color:prevColor);
                Game.Skin.Assets.Play_Lane_Text_Master.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y + prevTextY, color:prevTextColor);
                break;
            }
        }



        for(int i = 0; i < Flashs.Count; i++)
        {
            LaneFlashInfo flash = Flashs[i];
            Color4 color = new (1, 1, 1, Math.Min(flash.Opacity, 1));
            switch(flash.FlashType)
            {
                case -1:
                Game.Skin.Assets.Play_Lane_Flash_Yellow.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y, color:color, blendType:Engine.Enums.BlendType.Add);
                break;
                case 0:
                Game.Skin.Assets.Play_Lane_Flash_Red.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y, color:color, blendType:Engine.Enums.BlendType.Add);
                break;
                case 1:
                Game.Skin.Assets.Play_Lane_Flash_Blue.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y, color:color, blendType:Engine.Enums.BlendType.Add);
                break;
                case 2:
                Game.Skin.Assets.Play_Lane_Flash_Red.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y, color:color, blendType:Engine.Enums.BlendType.Add);
                break;
            }
        }

        if (GoGoTime)
        {
            float scale = 2 - GoGoFireInCounter;
            Game.Skin.Assets.Play_Lane_GoGoFire.Draw(Game.Skin.Value.Play_Lane_GoGoFire.Pos[Player].X, Game.Skin.Value.Play_Lane_GoGoFire.Pos[Player].Y, 
            scaleX:scale, scaleY:scale, rectangle:new RectangleF(Game.Skin.Value.Play_Lane_GoGoFire.Width * (int)(GoGoFireCounter * 7), 0, Game.Skin.Value.Play_Lane_GoGoFire.Width, Game.Skin.Value.Play_Lane_GoGoFire.Height), drawOriginType:Engine.Enums.DrawOriginType.Center, blendType:Engine.Enums.BlendType.Add);
        }
        else
        {
            Game.Skin.Assets.Play_Lane_GoGoFire.Draw(Game.Skin.Value.Play_Lane_GoGoFire.Pos[Player].X, Game.Skin.Value.Play_Lane_GoGoFire.Pos[Player].Y, 
            rectangle:new RectangleF(Game.Skin.Value.Play_Lane_GoGoFire.Width * (int)(GoGoFireCounter * 7), 0, Game.Skin.Value.Play_Lane_GoGoFire.Width, Game.Skin.Value.Play_Lane_GoGoFire.Height), drawOriginType:Engine.Enums.DrawOriginType.Center, blendType:Engine.Enums.BlendType.Add, 
            color:new (1, 1, 1, 1 - GoGoFireOutCounter));
        }
    }

    public void DrawAfterTarget()
    {
        if (GoGoTime)
        {
            float gogoscale = GoGoInCounter;
            Game.Skin.Assets.Play_Lane_GoGo.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y - (Game.Skin.Assets.Play_Lane_GoGo.TextureSize.Height * (gogoscale - 1) / 2.0f), 
            scaleY:gogoscale, blendType:Engine.Enums.BlendType.Add);
        }
        else
        {
            Game.Skin.Assets.Play_Lane_GoGo.Draw(Game.Skin.Value.Play_Lane_Main[Player].X, Game.Skin.Value.Play_Lane_Main[Player].Y, 
            color:new (1, 1, 1, 1 - GoGoOutCounter), blendType:Engine.Enums.BlendType.Add);
        }
    }
}