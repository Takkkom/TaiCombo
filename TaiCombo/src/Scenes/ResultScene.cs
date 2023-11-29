using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Helper;
using TaiCombo.Luas;
using TaiCombo.Objects;
using TaiCombo.Structs;

namespace TaiCombo.Scenes;

class ResultScene : Scene
{
    private string Title;
    private string SubTitle;
    private int PlayerCount;
    private bool RightSide;
    private GaugeType[] GaugeType;
    private NamePlate[] NamePlates;
    private Options[] Options;
    private ResultValues[] Values;
    private Gauge[] Gauges = new Gauge[Game.MAXPLAYER];
    private ResultAnimeType AnimeState;
    private float AnimeCounter;
    private float AnimeSpeed;
    private float ScoreRankFlashCounter;

    private void Skip()
    {

    }

    private void SetAnimeState(ResultAnimeType resultAnimeType, float animeSpeed)
    {
        AnimeState = resultAnimeType;
        AnimeCounter = 0;
        AnimeSpeed = animeSpeed;
    }

    private void DrawOmegaRank(float x, float y, ResultAnimeType resultAnimeType, int frame, Sprite sprite, int width, int height)
    {
        RectangleF rect = new RectangleF(0, height * frame, width, height);
        if (AnimeState == resultAnimeType)
        {
            float scale = EasingHelper.ResultScoreRankIn(AnimeCounter, 3.0f, -0.25f, 0.75f);
            float opacity = Math.Min(AnimeCounter / 0.75f, 1);
            float move_value = MathF.Cos(opacity * MathF.PI / 2.0f);

            if (move_value == 1)
            {
                sprite.Draw(x, y, scale, scale, rect, drawOriginType:DrawOriginType.Center, color: new Color4(1, 1, 1, opacity));
            }
            else 
            {
                for(int i = 0; i < 4; i++)
                {
                    float angle = (i + 0.5f) * MathF.PI / 2.0f;
                    float move_x = MathF.Sin(angle) * Game.Skin.Value.Result_ScoreRank.Padding * move_value;
                    float move_y = -MathF.Cos(angle) * Game.Skin.Value.Result_ScoreRank.Padding * move_value;
                    sprite.Draw(x + move_x, y + move_y, scale, scale, rect, drawOriginType:DrawOriginType.Center, color: new Color4(1, 1, 1, opacity));
                }
            }
        }
        else 
        {
            sprite.Draw(x, y, rectangle:rect, drawOriginType:DrawOriginType.Center);
        }
    }

    private void DrawCrown(float x, float y, ResultAnimeType resultAnimeType, int frame, Sprite sprite, int width, int height)
    {
        RectangleF rect = new RectangleF(0, height * frame, width, height);
        if (AnimeState == resultAnimeType)
        {
            float scale = EasingHelper.ResultScoreRankIn(AnimeCounter, 2.0f, -0.25f, 0.75f);
            float opacity = Math.Min(AnimeCounter / 0.75f, 1);

            sprite.Draw(x, y, 
            scale, scale, rect, drawOriginType:DrawOriginType.Center, color: new Color4(1, 1, 1, opacity));
        }
        else 
        {
            sprite.Draw(x, y, rectangle:rect, drawOriginType:DrawOriginType.Center);
        }

    }
    
    private void AnimeProcess()
    {
        Thread.Sleep(550);//PlayBGM

        Game.Skin.Assets.HitSounds[0].Don.Play();
        
        Thread.Sleep(883);//Gauge
        SetAnimeState(ResultAnimeType.AddGauge, 0.35f);
        int gaugeAddTime = 2900 * (int)(Math.Max(Values[0].Gauge, Values[1].Gauge) / 100.0f);
        

        Thread.Sleep(gaugeAddTime + 1150);//Perfect
        SetAnimeState(ResultAnimeType.Perfect, 1.0f / 0.08f);
        
        Thread.Sleep(400);//Ok
        SetAnimeState(ResultAnimeType.Ok, 1.0f / 0.08f);
        
        Thread.Sleep(400);//Miss
        SetAnimeState(ResultAnimeType.Miss, 1.0f / 0.08f);
        
        Thread.Sleep(400);//Roll
        SetAnimeState(ResultAnimeType.Roll, 1.0f / 0.08f);
        
        Thread.Sleep(400);//MaxCombo
        SetAnimeState(ResultAnimeType.MaxCombo, 1.0f / 0.08f);

        
        Thread.Sleep(850);//Score
        SetAnimeState(ResultAnimeType.Score, 1.0f / 0.33f);

        
        Thread.Sleep(1250);//ScoreRank
        SetAnimeState(ResultAnimeType.ScoreRank, 1.0f / 0.28f);

        
        Thread.Sleep(1280);//Crown
        SetAnimeState(ResultAnimeType.Crown, 1.0f / 0.28f);


        Thread.Sleep(1250);//BGAnime
        SetAnimeState(ResultAnimeType.BGAnime, 1.0f / 0.08f);

        for(int player = 0; player < PlayerCount; player++)
        {
            if (Values[player].ClearType >= ClearType.Clear) 
            {
                Game.Skin.Assets.Result_Background.PlayClearAnime(player);
            }
        }
    }

    public ResultScene(Dictionary<string, object> args)
    {
        PlayerCount = (int)args["PlayerCount"];
        Title = (string)args["Title"];
        SubTitle = (string)args["SubTitle"];
        RightSide = (bool)args["RightSide"];
        GaugeType = (GaugeType[])args["GaugeType"];
        NamePlates = (NamePlate[])args["NamePlates"];
        Options = (Options[])args["Options"];
        Values = (ResultValues[])args["Values"];

        for(int player = 0; player < PlayerCount; player++)
        {
            int side = RightSide ? 1 : player;
            Gauges[player] = new Gauge(GaugeType[player], 0, side);
            

        }
        
    }

    public override void Activate()
    {
        Game.Skin.Assets.Result_Background.SetValues(new Dictionary<string, object>()
        {
            { "title", Title },
            { "subtitle", SubTitle }
        });
        Game.Skin.Assets.Result_Background.SetP1IsBlue(RightSide);
        Game.Skin.Assets.Result_Background.SetPlayerCount(PlayerCount);
        Game.Skin.Assets.Result_Background.Init();

        Task.Run(AnimeProcess);
        
        base.Activate();
    }

    public override void DeActivate()
    {

        base.DeActivate();
    }

    public override void Update()
    {

        Game.Skin.Assets.Result_Background.Update();

        if (AnimeCounter < 1)
        {
            AnimeCounter += AnimeSpeed * GameEngine.Time_.DeltaTime;
        }
        else 
        {
            AnimeCounter = 1;
        }

        switch(AnimeState)
        {
            case ResultAnimeType.AddGauge:
            for(int player = 0; player < PlayerCount; player++)
            {
                Gauges[player].SetValue(Math.Min(AnimeCounter * 100, Values[player].Gauge));
            }
            break; 
            case ResultAnimeType.BGAnime:
            {
                if (ScoreRankFlashCounter < 1)
                {
                    ScoreRankFlashCounter += 1 * GameEngine.Time_.DeltaTime;
                }
                else 
                {
                    ScoreRankFlashCounter = 0;
                }
            }
            break; 
        }

        for(int player = 0; player < PlayerCount; player++)
        {
            int side = RightSide ? 1 : player;
            
            Gauges[player].Update();

        }

        base.Update();
    }

    public override void Draw()
    {
        Game.Skin.Assets.Result_Background.Draw();

        for(int player = 0; player < PlayerCount; player++)
        {
            int side = RightSide ? 1 : player;
            
            int plate_x = Game.Skin.Value.Result_Plate[side].X;
            int plate_y = Game.Skin.Value.Result_Plate[side].Y;
            Game.Skin.Assets.Result_Plate[player].Draw(plate_x, plate_y);

            int gauge_x = Game.Skin.Value.Result_Gauge[side].X;
            int gauge_y = Game.Skin.Value.Result_Gauge[side].Y;
            Gauges[player].Draw(gauge_x, gauge_y, 0.7f);

            if (AnimeState >= ResultAnimeType.Perfect)
            {
                float scale = AnimeState == ResultAnimeType.Perfect ? EasingHelper.ResultNumberIn(AnimeCounter, 0.5f) : 1;
                NumHelper.DrawResultNumber(Values[player].Perfect, Game.Skin.Value.Result_Number[side].Pos[0].X, Game.Skin.Value.Result_Number[side].Pos[0].Y, 
                Game.Skin.Value.Result_Number[side].Width, Game.Skin.Value.Result_Number[side].Height, Game.Skin.Value.Result_Number[side].Padding, 
                scale, scale, Game.Skin.Assets.Result_Number);
            }

            if (AnimeState >= ResultAnimeType.Ok)
            {
                float scale = AnimeState == ResultAnimeType.Ok ? EasingHelper.ResultNumberIn(AnimeCounter, 0.5f) : 1;
                NumHelper.DrawResultNumber(Values[player].Ok, Game.Skin.Value.Result_Number[side].Pos[1].X, Game.Skin.Value.Result_Number[side].Pos[1].Y, 
                Game.Skin.Value.Result_Number[side].Width, Game.Skin.Value.Result_Number[side].Height, Game.Skin.Value.Result_Number[side].Padding, 
                scale, scale, Game.Skin.Assets.Result_Number);
            }

            if (AnimeState >= ResultAnimeType.Miss)
            {
                float scale = AnimeState == ResultAnimeType.Miss ? EasingHelper.ResultNumberIn(AnimeCounter, 0.5f) : 1;
                NumHelper.DrawResultNumber(Values[player].Miss, Game.Skin.Value.Result_Number[side].Pos[2].X, Game.Skin.Value.Result_Number[side].Pos[2].Y, 
                Game.Skin.Value.Result_Number[side].Width, Game.Skin.Value.Result_Number[side].Height, Game.Skin.Value.Result_Number[side].Padding, 
                scale, scale, Game.Skin.Assets.Result_Number);
            }

            if (AnimeState >= ResultAnimeType.Roll)
            {
                float scale = AnimeState == ResultAnimeType.Roll ? EasingHelper.ResultNumberIn(AnimeCounter, 0.5f) : 1;
                NumHelper.DrawResultNumber(Values[player].Roll, Game.Skin.Value.Result_Number[side].Pos[3].X, Game.Skin.Value.Result_Number[side].Pos[3].Y, 
                Game.Skin.Value.Result_Number[side].Width, Game.Skin.Value.Result_Number[side].Height, Game.Skin.Value.Result_Number[side].Padding, 
                scale, scale, Game.Skin.Assets.Result_Number);
            }

            if (AnimeState >= ResultAnimeType.MaxCombo)
            {
                float scale = AnimeState == ResultAnimeType.MaxCombo ? EasingHelper.ResultNumberIn(AnimeCounter, 0.5f) : 1;
                NumHelper.DrawResultNumber(Values[player].MaxCombo, Game.Skin.Value.Result_Number[side].Pos[4].X, Game.Skin.Value.Result_Number[side].Pos[4].Y, 
                Game.Skin.Value.Result_Number[side].Width, Game.Skin.Value.Result_Number[side].Height, Game.Skin.Value.Result_Number[side].Padding, 
                scale, scale, Game.Skin.Assets.Result_Number);
            }
            

            if (AnimeState >= ResultAnimeType.Score)
            {
                float scale = AnimeState == ResultAnimeType.Score ? EasingHelper.ResultScoreNumberIn(AnimeCounter, 0.5f, -0.25f, 0.5f) : 1;
                NumHelper.DrawResultNumber(Values[player].Score, Game.Skin.Value.Result_ScoreNumber.Pos[side].X, Game.Skin.Value.Result_ScoreNumber.Pos[side].Y, 
                Game.Skin.Value.Result_ScoreNumber.Width, Game.Skin.Value.Result_ScoreNumber.Height, Game.Skin.Value.Result_ScoreNumber.Padding, 
                scale, scale, Game.Skin.Assets.Result_ScoreNumber);
            }

            if (AnimeState >= ResultAnimeType.ScoreRank)
            {
                if (Values[player].ScoreRank == ScoreRankType.Omega)
                {
                    Sprite scoreRank = Game.Skin.Assets.Result_ScoreRanks[(int)Values[player].ScoreRank - 1];
                    DrawOmegaRank(Game.Skin.Value.Result_ScoreRank.Pos[player].X, Game.Skin.Value.Result_ScoreRank.Pos[player].Y, 
                    ResultAnimeType.ScoreRank, 0, scoreRank, 
                    Game.Skin.Value.Result_ScoreRank.Width, Game.Skin.Value.Result_ScoreRank.Height);
                }
                else if (Values[player].ScoreRank >= ScoreRankType.E)
                {
                    Sprite scoreRank = Game.Skin.Assets.Result_ScoreRanks[(int)Values[player].ScoreRank - 1];
                    DrawCrown(Game.Skin.Value.Result_ScoreRank.Pos[player].X, Game.Skin.Value.Result_ScoreRank.Pos[player].Y, 
                    ResultAnimeType.ScoreRank, 0, scoreRank, 
                    Game.Skin.Value.Result_ScoreRank.Width, Game.Skin.Value.Result_ScoreRank.Height);
                }
            }

            if (AnimeState >= ResultAnimeType.Crown)
            {
                if (Values[player].ClearType >= ClearType.Clear)
                {
                    Sprite crown = Game.Skin.Assets.Result_Crowns[(int)Values[player].ClearType - 1];
                    DrawCrown(Game.Skin.Value.Result_Crown.Pos[player].X, Game.Skin.Value.Result_Crown.Pos[player].Y, ResultAnimeType.Crown, 0, crown, Game.Skin.Value.Result_Crown.Width, Game.Skin.Value.Result_Crown.Height);
                }
            }

            NamePlates[player].Draw(Game.Skin.Value.Result_NamePlate[player].X, Game.Skin.Value.Result_NamePlate[player].Y, player, side);
        }

        base.Draw();
    }
}