using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Helper;
using TaiCombo.Structs;

namespace TaiCombo.Objects;

class TaikoUI
{
    public class AddScoreInfo
    {
        public int Score;
        public float State;
        public float X_Begin;
        public float X;
        public float Y_Begin;
        public float Y;
    }

    private int Player;
    private int TaikoSide;
    private int CourseSymbol;

    private float ScoreJump;
    private float ComboJump;

    private List<AddScoreInfo> AddScores = new();

    private int Combo;
    private int Score;

    private float[] TaikoFlashOpacity = new float[5];

    private float ComboShine;

    private NamePlate NamePlate;

    private Options Options;

    private List<Sprite> DrawOptions = new();

    public void AddScore(int num)
    {
        if (num <= 0) return;
         
        AddScoreInfo score = new()
        {
            Score = num,
            State = 0,
            X_Begin = Game.Skin.Value.Play_Taiko_AddScore.Pos[Player].X,
            Y_Begin = Game.Skin.Value.Play_Taiko_AddScore.Pos[Player].Y
        };
        AddScores.Add(score);
    }

    public void SetScore(int num)
    {
        if (Score < num) ScoreJump = 0;
        Score = num;
    }

    public void SetCombo(int num)
    {
        if (Combo < num) ComboJump = 0;
        Combo = num;
    }

    public void FlashTaiko(TaikoType taikoType)
    {
        TaikoFlashOpacity[(int)taikoType] = 1.5f;
    }

    public TaikoUI(int player, int taikoSide, int courseSymbol, NamePlate namePlate, Options options)
    {
        Player = player;
        TaikoSide = taikoSide;
        CourseSymbol = courseSymbol;
        NamePlate = namePlate;
        Options = options;

        if (Game.Skin.Assets.Options_ScrollSpeed.ContainsKey(options.ScrollSpeed)) DrawOptions.Add(Game.Skin.Assets.Options_ScrollSpeed[options.ScrollSpeed]);
        if (options.Invisible != InvisibleType.None) DrawOptions.Add(Game.Skin.Assets.Options_Invisible[options.Invisible]);
        if (options.Flip) DrawOptions.Add(Game.Skin.Assets.Options_Flip);
        if (options.Random != RandomType.None) DrawOptions.Add(Game.Skin.Assets.Options_Random[options.Random]);
        if (options.Offset != 0) DrawOptions.Add(Game.Skin.Assets.Options_Offset);
    }

    public void Update()
    {
        /*
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.D)) //デバッグ用なのだ
        {
            SetCombo(10);
            SetScore(10);
            AddScore(10);
        }
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.F))
        {
            SetCombo(50);
            SetScore(50);
            AddScore(50);
        }
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.J))
        {
            SetCombo(100);
            SetScore(100);
            AddScore(100);
        }
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.K))
        {
            SetCombo(1000);
            SetScore(1000);
            AddScore(1000);
        }
        */

        for(int i = 0; i < 5; i++)
        {
            TaikoFlashOpacity[i] = Math.Max(TaikoFlashOpacity[i] - (10.0f * GameEngine.Time_.DeltaTime), 0);
        }
        

        for(int i = 0; i < AddScores.Count; i++)
        {
            AddScoreInfo score = AddScores[i];

            score.State += 2.0f * GameEngine.Time_.DeltaTime;

            float scoreIn = score.State * 3.5f;
            score.X = score.X_Begin + (100 * (1.0f - EasingHelper.EaseOutBack(Math.Min(scoreIn, 1))));
            score.Y = score.Y_Begin;

            if (score.State > 1)
            {
                AddScores.Remove(score);
            }
        }
        
        ComboJump = MathF.Min(ComboJump + (7.0f * GameEngine.Time_.DeltaTime), 1);
        ScoreJump = MathF.Min(ScoreJump + (7.0f * GameEngine.Time_.DeltaTime), 1);

        ComboShine += 1f * GameEngine.Time_.DeltaTime;
        if (ComboShine > 0.5f)
        {
            ComboShine = 0;
        }
    }

    public void Draw()
    {
        Game.Skin.Assets.Play_Taiko_Background[TaikoSide].Draw(Game.Skin.Value.Play_Taiko_Background[Player].X, Game.Skin.Value.Play_Taiko_Background[Player].Y);
        Game.Skin.Assets.Play_Taiko_Background_Frame.Draw(Game.Skin.Value.Play_Taiko_Background[Player].X, Game.Skin.Value.Play_Taiko_Background[Player].Y, flipY:Player == 1);


        //Score
        for(int i = 0; i < DrawOptions.Count; i++)
        {
            int x = Game.Skin.Value.Play_Taiko_Options.Pos[Player].X + (Game.Skin.Value.Play_Taiko_Options.Padding * (i % 3));
            int y = Game.Skin.Value.Play_Taiko_Options.Pos[Player].Y + (Game.Skin.Value.Play_Taiko_Options.Padding * (i / 3));
            DrawOptions[i].Draw(x, y);
        }

        NamePlate.Draw(Game.Skin.Value.Play_Taiko_NamePlate[Player].X, Game.Skin.Value.Play_Taiko_NamePlate[Player].Y, TaikoSide, Player);




        Game.Skin.Assets.Play_Taiko_CourseSymbol.Draw(Game.Skin.Value.Play_Taiko_CourseSymbol.Pos[Player].X, Game.Skin.Value.Play_Taiko_CourseSymbol.Pos[Player].Y, 
        rectangle:new System.Drawing.RectangleF(0, Game.Skin.Value.Play_Taiko_CourseSymbol.Height * CourseSymbol, Game.Skin.Value.Play_Taiko_CourseSymbol.Width, Game.Skin.Value.Play_Taiko_CourseSymbol.Height));




        //Taiko-----
        Game.Skin.Assets.Play_Taiko_Taiko.Draw(Game.Skin.Value.Play_Taiko_Taiko.Pos[Player].X, Game.Skin.Value.Play_Taiko_Taiko.Pos[Player].Y, 
        rectangle:new System.Drawing.RectangleF(0, Game.Skin.Value.Play_Taiko_Taiko.Height * 0, Game.Skin.Value.Play_Taiko_Taiko.Width, Game.Skin.Value.Play_Taiko_Taiko.Height));

        for(int i = 0; i < 5; i++)
        {
            Game.Skin.Assets.Play_Taiko_Taiko.Draw(Game.Skin.Value.Play_Taiko_Taiko.Pos[Player].X, Game.Skin.Value.Play_Taiko_Taiko.Pos[Player].Y, 
            rectangle:new System.Drawing.RectangleF(0, Game.Skin.Value.Play_Taiko_Taiko.Height * (i + 1), Game.Skin.Value.Play_Taiko_Taiko.Width, Game.Skin.Value.Play_Taiko_Taiko.Height),
            color:new Color4(1.0f, 1.0f, 1.0f, Math.Min(TaikoFlashOpacity[i], 1)));
        }
        //---------


        //Combo-----
        if (Combo >= 10)
        {
            float comboScale = NumHelper.GetNumJumpScale(ComboJump);

            if (Combo < 50)
            {
                NumHelper.DrawCombo(Combo, Game.Skin.Value.Play_Taiko_Combo.Pos[Player].X, 
                Game.Skin.Value.Play_Taiko_Combo.Pos[Player].Y, 
                Game.Skin.Value.Play_Taiko_Combo.Width, Game.Skin.Value.Play_Taiko_Combo.Height, Game.Skin.Value.Play_Taiko_Combo.Padding,
                1.0f, comboScale, Game.Skin.Assets.Play_Taiko_Combo, -1);
            }
            else if (Combo < 100)
            {
                NumHelper.DrawCombo(Combo, Game.Skin.Value.Play_Taiko_Combo.Pos[Player].X, 
                Game.Skin.Value.Play_Taiko_Combo.Pos[Player].Y, 
                Game.Skin.Value.Play_Taiko_Combo.Width, Game.Skin.Value.Play_Taiko_Combo.Height, Game.Skin.Value.Play_Taiko_Combo.Padding,
                1.0f, comboScale, Game.Skin.Assets.Play_Taiko_Combo_Medium, ComboShine);
            }
            else
            {
                float scaleX = Combo < 1000 ? 1.0f : 0.85f;
                NumHelper.DrawCombo(Combo, Game.Skin.Value.Play_Taiko_Combo.Pos[Player].X, 
                Game.Skin.Value.Play_Taiko_Combo.Pos[Player].Y, 
                Game.Skin.Value.Play_Taiko_Combo.Width, Game.Skin.Value.Play_Taiko_Combo.Height, Game.Skin.Value.Play_Taiko_Combo.Padding * scaleX,
                scaleX, comboScale, Game.Skin.Assets.Play_Taiko_Combo_Big, ComboShine);
            }
        
            Game.Skin.Assets.Play_Taiko_Combo_Text.Draw(Game.Skin.Value.Play_Taiko_Taiko.Pos[Player].X, Game.Skin.Value.Play_Taiko_Taiko.Pos[Player].Y);
        }
        //-----------

        //Score-----
        float scoreScale = NumHelper.GetNumJumpScale(ScoreJump);

        NumHelper.DrawScoreNumber(Score, Game.Skin.Value.Play_Taiko_Score.Pos[Player].X, 
        Game.Skin.Value.Play_Taiko_Score.Pos[Player].Y - ((Game.Skin.Value.Play_Taiko_Score.Height / 2.5f) * (scoreScale - 1)), 
        Game.Skin.Value.Play_Taiko_Score.Width, Game.Skin.Value.Play_Taiko_Score.Height, 
        1.0f, scoreScale,Game.Skin.Value.Play_Taiko_Score.Padding, 0.0f, 0.0f, Game.Skin.Assets.Play_Taiko_Score_Base);


        for(int i = 0; i < AddScores.Count; i++)
        {
            AddScoreInfo score = AddScores[i];
            NumHelper.DrawScoreNumber(score.Score, score.X, score.Y, 
            Game.Skin.Value.Play_Taiko_AddScore.Width, Game.Skin.Value.Play_Taiko_AddScore.Height, 
            1.0f, 1.0f, Game.Skin.Value.Play_Taiko_AddScore.Padding, MathF.Max(score.State - 0.5f, 0) * 2, Player == 0 ? 12 : -12, Game.Skin.Assets.Play_Taiko_Score[TaikoSide]);
        }
        //-----------
    }
}