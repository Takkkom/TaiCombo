using Silk.NET.Maths;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Plugin.Chart;
using TaiCombo.Objects;
using TaiCombo.Enums;
using TaiCombo.Helper;
using TaiCombo.Structs;
using TaiCombo.Plugin.Enums;
using TaiCombo.Skin;

namespace TaiCombo.Scenes;

class PlayScene : Scene
{
    private int PlayerCount;
    private bool PlayingBGM;
    private bool Playing;
    private long InitTime;
    private long NowTime;

    private Sound Song;

    private string ChartPath;

    private bool[] GoGoTime = new bool[Game.MAXPLAYER];

    private CourseType[] Courses = new CourseType[Game.MAXPLAYER];
    private Options[] Options = new Options[Game.MAXPLAYER];

    private JudgeZones[] JudgeZones = new JudgeZones[Game.MAXPLAYER];

    private AddScores[] AddScores = new AddScores[Game.MAXPLAYER];

    private AddGauges[] AddGauges = new AddGauges[Game.MAXPLAYER];
    private GaugeType[] GaugeType = new GaugeType[Game.MAXPLAYER];
    private float PlaySpeed;
    private GameModeType GameMode;

    private float[] NotePixelOffset = new float[Game.MAXPLAYER];

    private void Reset()
    {
        for(int player = 0; player < PlayerCount; player++)
        {
            for(int i = 0; i < Chips[player].Count; i++)
            {
                IChip chip = Chips[player][i];

                chip.Hit = false;
                chip.Miss = false;
                chip.Active = true;
                chip.Over = false;
                chip.NowRollCount = 0;
            }
            GoGoTime[player] = false;
            
            NoteHelper.BranchChips(Chips[player], 0, BranchType.Normal);
        }
    }

    private void Start()
    {
        InitTime = (long)(GameEngine.Time_.NowMicroSecondTime * PlaySpeed) - NowTime;
        Playing = true;
    }

    private void Stop()
    {
        Song.Stop();
        Playing = false;
    }

    private IChartInfo Chart;

    public List<IChip>[] Chips = new List<IChip>[Game.MAXPLAYER];

    private Lane[] Lane = new Lane[Game.MAXPLAYER];

    private Gauge[] Gauge = new Gauge[Game.MAXPLAYER];

    private HitExplosion[] HitExplosion = new HitExplosion[Game.MAXPLAYER];

    private JudgeAnimes[] JudgeAnimes = new JudgeAnimes[Game.MAXPLAYER];

    private TaikoUI[] TaikoUI = new TaikoUI[Game.MAXPLAYER];

    private Rainbow[] Rainbow = new Rainbow[Game.MAXPLAYER];

    private FlyNotes[] FlyNotes = new FlyNotes[Game.MAXPLAYER];

    private Combo[] Combo = new Combo[Game.MAXPLAYER];

    private Roll[] Roll = new Roll[Game.MAXPLAYER];

    private Balloon[] Balloon = new Balloon[Game.MAXPLAYER];

    private ScoreRank[] ScoreRank = new ScoreRank[Game.MAXPLAYER];

    private PlayStates[] States = new PlayStates[Game.MAXPLAYER];

    private IChip[] CurrentChip = new IChip[Game.MAXPLAYER];

    private IChip?[] CurrentRollChip = new IChip?[Game.MAXPLAYER];

    private HitSound[] HitSound = new HitSound[Game.MAXPLAYER];

    private float[] NoteAnimeCounter = new float[Game.MAXPLAYER];

    private float[] RollCloseCounter = new float[Game.MAXPLAYER];

    private int[] NoteFrame = new int[Game.MAXPLAYER];

    private void Event(PlayEventType playEventType, int player)
    {
        switch(playEventType)
        {
            case PlayEventType.GoGoStart:
            {
                Lane[player].GoGoIn();
            }
            break;
            case PlayEventType.GoGoEnd:
            {
                Lane[player].GoGoOut();
            }
            break;
        }
    }

    private void LoadChart(string chartPath)
    {
        IChartInfo? chart = Game.Plugins.LoadChart(chartPath);
        if (chart == null)
        {
            GameEngine.SceneManager_.ChangeScene(new SongSelectScene());
            return;
        }

        Chart = chart;
        Song = new($"{chartPath}{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}{Chart.Audio}");

        for(int player = 0; player < PlayerCount; player++)
        {
            Course course = Chart.Courses[Courses[player]];
            Chips[player] = new();

            for(int i = 0; i < course.Chips.Count; i++)
            {
                IChip chip = course.Chips[i].Copy();

                switch(Options[player].Random)
                {
                    case RandomType.Minor:
                    if (Random.Shared.Next(0, 3) == 0)
                    {
                        chip.ChipType = NoteHelper.FlipChipType(chip.ChipType);
                        chip.SENoteType = NoteHelper.FlipSENoteType(chip.SENoteType);
                    }
                    break;
                    case RandomType.Random:
                    if (Random.Shared.Next(0, 2) == 0)
                    {
                        chip.ChipType = NoteHelper.FlipChipType(chip.ChipType);
                        chip.SENoteType = NoteHelper.FlipSENoteType(chip.SENoteType);
                    }
                    break;
                }
                
                if (Options[player].Flip)
                {
                    chip.ChipType = NoteHelper.FlipChipType(chip.ChipType);
                    chip.SENoteType = NoteHelper.FlipSENoteType(chip.SENoteType);
                }

                Chips[player].Add(chip);
            }
        }
        Reset();
    }

    private void UpdateScoreRank(int player)
    {
        switch(States[player].ScoreRank)
        {
            case ScoreRankType.None:
            if (States[player].Score >= 500000)
            {
                ScoreRank[player].Add(ScoreRankType.E);
                States[player].ScoreRank = ScoreRankType.E;
            }
            break;
            case ScoreRankType.E:
            if (States[player].Score >= 600000)
            {
                ScoreRank[player].Add(ScoreRankType.D);
                States[player].ScoreRank = ScoreRankType.D;
            }
            break;
            case ScoreRankType.D:
            if (States[player].Score >= 700000)
            {
                ScoreRank[player].Add(ScoreRankType.C);
                States[player].ScoreRank = ScoreRankType.B;
            }
            break;
            case ScoreRankType.C:
            if (States[player].Score >= 800000)
            {
                ScoreRank[player].Add(ScoreRankType.B);
                States[player].ScoreRank = ScoreRankType.B;
            }
            break;
            case ScoreRankType.B:
            if (States[player].Score >= 900000)
            {
                ScoreRank[player].Add(ScoreRankType.A);
                States[player].ScoreRank = ScoreRankType.A;
            }
            break;
            case ScoreRankType.A:
            if (States[player].Score >= 950000)
            {
                ScoreRank[player].Add(ScoreRankType.S);
                States[player].ScoreRank = ScoreRankType.S;
            }
            break;
            case ScoreRankType.S:
            if (States[player].Score >= 1000000)
            {
                ScoreRank[player].Add(ScoreRankType.Omega);
                States[player].ScoreRank = ScoreRankType.Omega;
            }
            break;
        }
    }

    private void Judge(int player, IChip chip, JudgeType judge, bool hit, HitType hitType)
    {
        switch(judge)
        {
            case JudgeType.Perfect:
            {
                States[player].Perfect++;
                States[player].Score += GoGoTime[player] ? ScoreHelper.GetValue(AddScores[player].Perfect, AddScores[player].GoGoRate) : AddScores[player].Perfect;
                States[player].Gauge += AddGauges[player].Perfect;
                States[player].Gauge = Math.Min(Math.Max(States[player].Gauge, 0), 100);
                States[player].Combo++;
                
                Gauge[player].SetValue(States[player].Gauge);
                TaikoUI[player].SetCombo(States[player].Combo);
                TaikoUI[player].AddScore(AddScores[player].Perfect);
                TaikoUI[player].SetScore(States[player].Score);

                HitExplosion[player].AddEffect(chip.ChipType == ChipType.Don_Big || chip.ChipType == ChipType.Ka_Big ? 2 : 0);
            }
            break;
            case JudgeType.Ok:
            {
                States[player].Ok++;
                States[player].Score += GoGoTime[player] ? ScoreHelper.GetValue(AddScores[player].Ok, AddScores[player].GoGoRate) : AddScores[player].Ok;
                States[player].Gauge += AddGauges[player].Ok;
                States[player].Gauge = Math.Min(Math.Max(States[player].Gauge, 0), 100);
                States[player].Combo++;
                
                Gauge[player].SetValue(States[player].Gauge);
                TaikoUI[player].SetCombo(States[player].Combo);
                TaikoUI[player].AddScore(AddScores[player].Ok);
                TaikoUI[player].SetScore(States[player].Score);
                
                HitExplosion[player].AddEffect(chip.ChipType == ChipType.Don_Big || chip.ChipType == ChipType.Ka_Big ? 3 : 1);
            }
            break;
            case JudgeType.Miss:
            {
                States[player].Miss++;
                States[player].Score += GoGoTime[player] ? ScoreHelper.GetValue(AddScores[player].Miss, AddScores[player].GoGoRate) : AddScores[player].Miss;
                States[player].Gauge += AddGauges[player].Miss;
                States[player].Gauge = Math.Min(Math.Max(States[player].Gauge, 0), 100);
                States[player].Combo = 0;
                
                Gauge[player].SetValue(States[player].Gauge);
                TaikoUI[player].SetCombo(States[player].Combo);
                TaikoUI[player].AddScore(AddScores[player].Miss);
                TaikoUI[player].SetScore(States[player].Score);
            }
            break;
            case JudgeType.Roll:
            {
                States[player].Roll++;
                States[player].Score += AddScores[player].Roll;
                TaikoUI[player].AddScore(AddScores[player].Roll);
                TaikoUI[player].SetScore(States[player].Score);

                Roll[player].Open(false);
                Roll[player].SetNumber(CurrentRollChip[player].NowRollCount);
                
                if (hitType == HitType.Don)
                {
                    FlyNotes[player].Add(FlyNoteType.Don);
                }
                else if (hitType == HitType.Ka)
                {
                    FlyNotes[player].Add(FlyNoteType.Ka);
                }
                else if (hitType == HitType.Clap)
                {
                    FlyNotes[player].Add(FlyNoteType.Don);
                }
            }
            break;
            case JudgeType.RollBig:
            {
                States[player].Roll++;
                States[player].Score += AddScores[player].Roll_Big;
                TaikoUI[player].AddScore(AddScores[player].Roll_Big);
                TaikoUI[player].SetScore(States[player].Score);

                Roll[player].Open(false);
                Roll[player].SetNumber(CurrentRollChip[player].NowRollCount);
                
                if (hitType == HitType.Don)
                {
                    FlyNotes[player].Add(FlyNoteType.Don_Big);
                }
                else if (hitType == HitType.Ka)
                {
                    FlyNotes[player].Add(FlyNoteType.Ka_Big);
                }
                else if (hitType == HitType.Clap)
                {
                    FlyNotes[player].Add(FlyNoteType.Don_Big);
                }
            }
            break;
            case JudgeType.Balloon:
            {
                States[player].Roll++;
                States[player].Score += AddScores[player].Balloon_Roll;
                TaikoUI[player].AddScore(AddScores[player].Balloon_Roll);
                TaikoUI[player].SetScore(States[player].Score);

                Balloon[player].Open();
                Balloon[player].SetNumber(CurrentRollChip[player].BalloonCount - CurrentRollChip[player].NowRollCount);

                CurrentRollChip[player].Hit = true;
            }
            break;
            case JudgeType.BalloonBreak:
            {
                States[player].Roll++;
                States[player].Score += AddScores[player].Balloon_Broke;
                TaikoUI[player].AddScore(AddScores[player].Balloon_Broke);
                TaikoUI[player].SetScore(States[player].Score);

                Rainbow[player].Open();
                FlyNotes[player].Add(FlyNoteType.Don);

                Balloon[player].Broke();
            }
            break;
            case JudgeType.Kusudama:
            {
                States[player].Roll++;
                States[player].Score += AddScores[player].Kusudama_Roll;
                TaikoUI[player].AddScore(AddScores[player].Kusudama_Roll);
                TaikoUI[player].SetScore(States[player].Score);
            }
            break;
            case JudgeType.KusudamaBreak:
            {
                States[player].Roll++;
                States[player].Score += AddScores[player].Kusudama_Broke;
                TaikoUI[player].AddScore(AddScores[player].Kusudama_Broke);
                TaikoUI[player].SetScore(States[player].Score);
            }
            break;
        }

        UpdateScoreRank(player);

        if (hit)
        {
            switch(chip.ChipType)
            {
                case ChipType.Don:
                FlyNotes[player].Add(FlyNoteType.Don);
                break;
                case ChipType.Ka:
                FlyNotes[player].Add(FlyNoteType.Ka);
                break;
                case ChipType.Don_Big:
                case ChipType.Roll_Balloon_Start:
                FlyNotes[player].Add(FlyNoteType.Don_Big);
                break;
                case ChipType.Ka_Big:
                FlyNotes[player].Add(FlyNoteType.Ka_Big);
                break;
            }
            
            if (States[player].Combo == 10 && Courses[player] == CourseType.Easy)
            {
                Combo[player].Open(States[player].Combo);
            }
            else if (States[player].Combo == 50)
            {
                Combo[player].Open(States[player].Combo);
            }
            else if (States[player].Combo % 100 == 0 && States[player].Combo >= 100) 
            {
                Combo[player].Open(States[player].Combo);
            }

            JudgeAnimes[player].AddJudge(judge);
            chip.Hit = true;
        }
    }

    private void CheckHit(int player, HitType hitType)
    {
        if (CurrentRollChip[player] == null)
        {
            for(int i = 0; i < Chips[player].Count; i++)
            {
                IChip chip = Chips[player][i];

                if (NoteHelper.GetHit(chip.ChipType, hitType) && NoteHelper.IsHittableNote(chip.ChipType) && !chip.Hit && chip.Active)
                {
                    long nowTime = chip.Time - NowTime;
                    JudgeType judge =  NoteHelper.GetJudge(Math.Abs(nowTime), JudgeZones[player]); 

                    if (judge != JudgeType.None)
                    {
                        Judge(player, chip, judge, true, hitType);
                        Lane[player].AddFlash(-1);
                        break;
                    }
                }
            }
        }
        else if (NoteHelper.GetHit(CurrentRollChip[player].ChipType, hitType))  
        {
            CurrentRollChip[player].NowRollCount++;
            switch(CurrentRollChip[player].ChipType)
            {
                case Plugin.Enums.ChipType.Roll_Start:
                Judge(player, CurrentRollChip[player], JudgeType.Roll, false, hitType);
                break;
                case Plugin.Enums.ChipType.Roll_Big_Start:
                Judge(player, CurrentRollChip[player], JudgeType.RollBig, false, hitType);
                break;
                case Plugin.Enums.ChipType.Roll_Balloon_Start:
                if (CurrentRollChip[player].NowRollCount < CurrentRollChip[player].BalloonCount)
                {
                    Judge(player, CurrentRollChip[player], JudgeType.Balloon, false, hitType);
                }
                else 
                {
                    Judge(player, CurrentRollChip[player], JudgeType.BalloonBreak, true, hitType);
                    CurrentRollChip[player] = null;
                }
                break;
                case Plugin.Enums.ChipType.Roll_Kusudama_Start:
                if (CurrentRollChip[player].NowRollCount < CurrentRollChip[player].BalloonCount)
                {
                    Judge(player, CurrentRollChip[player], JudgeType.Kusudama, false, hitType);
                }
                else 
                {
                    Judge(player, CurrentRollChip[player], JudgeType.KusudamaBreak, true, hitType);
                    CurrentRollChip[player] = null;
                }
                break;
                case Plugin.Enums.ChipType.Roll_Don:
                Judge(player, CurrentRollChip[player], JudgeType.Roll, false, hitType);
                break;
                case Plugin.Enums.ChipType.Roll_Ka:
                Judge(player, CurrentRollChip[player], JudgeType.Roll, false, hitType);
                break;
                case Plugin.Enums.ChipType.Roll_Clap:
                Judge(player, CurrentRollChip[player], JudgeType.Roll, false, hitType);
                break;
            }
        }
        
    }

    private void HitTaiko(int player, TaikoType taikoType)
    {
        TaikoUI[player].FlashTaiko(taikoType);
        Sound? sound = null;
        switch(taikoType)
        {
            case TaikoType.DonLeft:
            case TaikoType.DonRight:
            {
                if (PlayerCount == 1)
                {
                    sound = HitSound[player].Don;
                }
                else
                {
                    sound = player == 0 ? HitSound[player].Don_Left : HitSound[player].Don_Right;
                }
                Lane[player].AddFlash(0);
                CheckHit(player, HitType.Don);
            }
            break;
            case TaikoType.KaLeft:
            case TaikoType.KaRight:
            {
                if (PlayerCount == 1)
                {
                    sound = HitSound[player].Ka;
                }
                else
                {
                    sound = player == 0 ? HitSound[player].Ka_Left : HitSound[player].Ka_Right;
                }
                Lane[player].AddFlash(1);
                CheckHit(player, HitType.Ka);
            }
            break;
            case TaikoType.Clap:
            {
                if (PlayerCount == 1)
                {
                    sound = HitSound[player].Clap;
                }
                else
                {
                    sound = player == 0 ? HitSound[player].Clap_Left : HitSound[player].Clap_Right;
                }
                Lane[player].AddFlash(2);
                CheckHit(player, HitType.Clap);
            }
            break;
        }
        sound?.Play();
    }

    private void OverChip(int player, IChip chip, int index, long nowTime)
    {
        if (nowTime < 0)
        {
            CurrentChip[player] = chip;
        }

        switch(chip.ChipType)
        {
            case ChipType.Don:
            case ChipType.Don_Big:
            case ChipType.Don_Big_Joint:
            if (Options[player].AutoPlay && nowTime < 0 && !chip.Hit && chip.Active && !chip.Miss && !chip.Over && Playing)
            {
                HitTaiko(player, TaikoType.DonRight);

                chip.Over = true;
            }
            break;
            case ChipType.Ka:
            case ChipType.Ka_Big:
            case ChipType.Ka_Big_Joint:
            if (Options[player].AutoPlay && nowTime < 0 && !chip.Hit && chip.Active && !chip.Miss && !chip.Over && Playing)
            {
                HitTaiko(player, TaikoType.KaLeft);

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Start:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Big_Start:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Balloon_Start:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Kusudama_Start:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Fuse:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Don:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Ka:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Clap:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;

                chip.Over = true;
            }
            break;
            case ChipType.Roll_End:
            if (nowTime < 0 && !chip.Over)
            {
                if (CurrentRollChip[player] != null && CurrentRollChip[player].Hit)
                {
                    switch(CurrentRollChip[player].ChipType)
                    {
                        case ChipType.Roll_Balloon_Start:
                        Balloon[player].Miss();
                        break;
                    }
                }
                CurrentRollChip[player] = null;

                chip.Over = true;
            }
            break;
            case ChipType.GoGoStart:
            if (nowTime < 0 && !chip.Over)
            {
                Event(PlayEventType.GoGoStart, player);
                GoGoTime[player] = true;

                chip.Over = true;
            }
            break;
            case ChipType.GoGoEnd:
            if (nowTime < 0 && !chip.Over)
            {
                Event(PlayEventType.GoGoEnd, player);
                GoGoTime[player] = false;

                chip.Over = true;
            }
            break;
            case ChipType.BranchStart:
            if (nowTime < 0 && !chip.Over)
            {
                BranchType branchType = BranchType.Master;
                NoteHelper.BranchChips(Chips[player], index + 1, branchType);
                Lane[player].Branch(branchType);
                chip.Over = true;
            }
            break;
            case ChipType.BranchEnd:
            if (nowTime < 0 && !chip.Over)
            {
                
                chip.Over = true;
            }
            break;
            default:
            if (nowTime < 0 && !chip.Over)
            {
                
                chip.Over = true;
            }
            break;
        }
    }

    public PlayScene(Dictionary<string, object> args)
    {
        PlayerCount = (int)args["PlayerCount"];
        ChartPath = (string)args["ChartPath"];
        Courses = (CourseType[])args["Courses"];
        Options = (Options[])args["Options"];
        GaugeType = (GaugeType[])args["GaugeType"];
        PlaySpeed = (float)args["PlaySpeed"];
        GameMode = (GameModeType)args["GameMode"];

        for(int player = 0; player < PlayerCount; player++)
        {
            HitSound[player] = Game.Skin.Assets.HitSounds[Options[player].HitSound];
            NotePixelOffset[player] = (Options[player].Offset / 5.0f) * (Game.Skin.Value.Play_Notes.Padding / 36);
        }

        LoadChart(ChartPath);

        for(int player = 0; player < PlayerCount; player++)
        {
            if ((int)GaugeType[player] == -1)
            {
                switch(Courses[player])
                {
                    case CourseType.Easy:
                    GaugeType[player] = Enums.GaugeType.Level0;
                    break;
                    case CourseType.Normal:
                    case CourseType.Hard:
                    GaugeType[player] = Enums.GaugeType.Level1;
                    break;
                    case CourseType.Oni:
                    case CourseType.Edit:
                    GaugeType[player] = Enums.GaugeType.Level2;
                    break;
                }
            }

            int taikoSide = player;
            TaikoUI[player] = new(player, taikoSide, (int)Courses[player], Options[player]);
            Lane[player] = new(player);
            Gauge[player] = new(GaugeType[player], player, taikoSide);
            HitExplosion[player] = new(player);
            JudgeAnimes[player] = new(player);
            Rainbow[player] = new(player);
            FlyNotes[player] = new(player);
            Combo[player] = new(player);
            Roll[player] = new(player);
            Balloon[player] = new(player);
            ScoreRank[player] = new(player);
            JudgeZones[player] = NoteHelper.JudgeZones[Courses[player]];
            AddScores[player] = ScoreHelper.GetAddScores(Chips[player], ScoreType.Gen4);
            AddGauges[player] = GaugeHelper.GetAddGauge(Chips[player], Courses[player], Chart.Courses[Courses[player]].Level);
        }

        NowTime = -1500000;
    }

    public override void Activate()
    {
        Start();

        
        base.Activate();
    }

    public override void DeActivate()
    {
        Song.Dispose();

        base.DeActivate();
    }

    public override void Update()
    {
        if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.Q))
        {
            LoadChart(ChartPath);
        }
        else if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.Space))
        {
            if (Playing)
            {
                Stop();
            }
            else
            {
                Reset();
                Start();
            }
        }
        else if (GameEngine.Input.GetKeyPressing(Silk.NET.Input.Key.Left))
        {
            NowTime = NowTime - (long)(GameEngine.Time_.DeltaTime * 1000000);
        }
        else if (GameEngine.Input.GetKeyPressing(Silk.NET.Input.Key.Right))
        {
            NowTime = NowTime + (long)(GameEngine.Time_.DeltaTime * 1000000);
        }
        
        if (Playing)
        {
            NowTime = (long)(GameEngine.Time_.NowMicroSecondTime * PlaySpeed) - InitTime;

            float songTime = (NowTime / 1000000.0f) - Chart.Offset;
            if (!PlayingBGM && songTime > 0)
            {
                Song.Time = songTime;
                Song.Play(false);
                Song.Speed = PlaySpeed;
                PlayingBGM = true;
            }
        }
        else 
        {
            PlayingBGM = false;
        }

        for(int player = 0; player < PlayerCount; player++)
        {
            if (Options[player].AutoPlay)
            {
                if (Playing && CurrentRollChip[player] != null)
                {
                    HitTaiko(player, TaikoType.DonRight);
                }
            }
            else
            {
                if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.D))
                {
                    HitTaiko(player, TaikoType.KaLeft);
                }
                else if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.F))
                {
                    HitTaiko(player, TaikoType.DonLeft);
                }
                else if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.J))
                {
                    HitTaiko(player, TaikoType.DonRight);
                }
                else if (GameEngine.Input.GetKeyPressed(Silk.NET.Input.Key.K))
                {
                    HitTaiko(player, TaikoType.KaRight);
                }
            }
            
            CurrentChip[player] = Chips[player][0];
            
            Lane[player].Update();
            Gauge[player].Update();
            HitExplosion[player].Update();
            JudgeAnimes[player].Update();
            Rainbow[player].Update();
            FlyNotes[player].Update();
            Combo[player].Update();
            Roll[player].Update();
            Balloon[player].Update();
            ScoreRank[player].Update();

            Chips[player][0].InitHBValue();

            NoteAnimeCounter[player] += (CurrentChip[player].BPM / 60.0f) * PlaySpeed * GameEngine.Time_.DeltaTime;
            if (NoteAnimeCounter[player] >= 1)
            {
                NoteAnimeCounter[player] = 0;
            }

            if (States[player].Combo < 10)
            {
                NoteFrame[player] = -1;
            }
            else if (States[player].Combo < 50)
            {
                NoteFrame[player] = (int)(NoteAnimeCounter[player] * 2);
            }
            else if (States[player].Combo < 100)
            {
                NoteFrame[player] = (int)(NoteAnimeCounter[player] * 4) % 2;
            }
            else
            {
                NoteFrame[player] = (int)(NoteAnimeCounter[player] * 4) % 2 == 0 ? 0 : 2;
            }

            if (CurrentRollChip[player] == null && Roll[player].Opend)
            {
                RollCloseCounter[player] += 1 * Game.Time_.DeltaTime;
                if (RollCloseCounter[player] > 1.3f)
                {
                    Roll[player].Close();
                }
            }
            else 
            {
                RollCloseCounter[player] = 0;
            }

            for(int i = 0; i < Chips[player].Count; i++)
            {
                var chip = Chips[player][i];
                long nowTime = chip.Time - NowTime;

                if (chip.ChipType == ChipType.Roll_Balloon_Start)
                {
                    var balloonTime = nowTime < 0 ? Math.Max(-nowTime - chip.RollLength, 0) + chip.Time : NowTime;
                    chip.Update(balloonTime);
                }
                else 
                {
                    chip.Update(NowTime);
                }

                if (Playing) OverChip(player, chip, i, nowTime);

                JudgeType judge =  NoteHelper.GetJudge(Math.Abs(nowTime), JudgeZones[player]); 
                    

                if (true) //
                {
                    if (Playing && NoteHelper.IsHittableNote(chip.ChipType) && !chip.Miss && !chip.Hit && nowTime < 0 && judge == JudgeType.None)
                    {
                        Judge(player, chip, JudgeType.Miss, false, HitType.Don);
                        chip.Miss = true;
                    }
                }
                else 
                {
                    chip.Miss = false;
                    chip.Hit = false;
                    chip.Over = false;
                }
            }

            TaikoUI[player].Update();
        }

        base.Update();
    }

    public override void Draw()
    {
        for(int player = 0; player < PlayerCount; player++)
        {
            Lane[player].Draw();
            Gauge[player].Draw(Game.Skin.Value.Play_Gauge.Pos[player].X, Game.Skin.Value.Play_Gauge.Pos[player].Y, 1.0f);
            
            Vector2D<float> target = new Vector2D<float>(Game.Skin.Value.Play_Notes.Pos[player].X, Game.Skin.Value.Play_Notes.Pos[player].Y);
            Vector2D<float> setarget = new Vector2D<float>(Game.Skin.Value.Play_SENotes.Pos[player].X, Game.Skin.Value.Play_SENotes.Pos[player].Y);

            System.Drawing.RectangleF rectangle = new System.Drawing.RectangleF(0, 0, Game.Skin.Value.Play_Notes.Width, Game.Skin.Value.Play_Notes.Height);
            Game.Skin.Assets.Play_Notes_Target.Draw(target.X, target.Y, rectangle:rectangle);
            
            Lane[player].DrawAfterTarget();
            HitExplosion[player].DrawBeforTaikoBG();
            
            target.X += NotePixelOffset[player];
            setarget.X += NotePixelOffset[player];

            for(int i = Chips[player].Count - 1; i >= 0; i--)
            {
                var chip = Chips[player][i];
                if ((chip.IsNote || chip.ChipType == ChipType.Line || chip.ChipType == ChipType.Line_Branched) && !chip.Hit && chip.Active)
                {
                    Vector2D<float> pos = chip.GetNotePosition() * Game.Skin.Value.Play_Notes.Padding * Options[player].ScrollSpeed;
                    Vector2D<float> notepos = target + pos;
                    Vector2D<float> senotepos = setarget + pos;
                    
                    if (chip.ChipType == ChipType.Line)
                    {
                        Game.Skin.Assets.Play_Notes_Line.Draw(notepos.X + (Game.Skin.Value.Play_Notes.Width / 2.0f), notepos.Y);
                    }
                    else if (chip.ChipType == ChipType.Line_Branched)
                    {
                        Game.Skin.Assets.Play_Notes_Line_Branched.Draw(notepos.X + (Game.Skin.Value.Play_Notes.Width / 2.0f), notepos.Y);
                    }
                    else 
                    {
                        switch(Options[player].Invisible)
                        {
                            case InvisibleType.None:
                            NoteHelper.DrawNote(notepos, chip.ChipType, Game.Skin.Value.Play_Notes.Width, Game.Skin.Value.Play_Notes.Height, NoteFrame[player], NoteAnimeCounter[player], player);
                            NoteHelper.DrawSENote(senotepos, chip.SENoteType, Game.Skin.Value.Play_SENotes.Width, Game.Skin.Value.Play_SENotes.Height, player);
                            break;
                            case InvisibleType.SEOnly:
                            NoteHelper.DrawSENote(senotepos, chip.SENoteType, Game.Skin.Value.Play_SENotes.Width, Game.Skin.Value.Play_SENotes.Height, player);
                            break;
                            case InvisibleType.Full:
                            break;
                        }
                    }
                }
            }

            TaikoUI[player].Draw();
            HitExplosion[player].DrawAfterTaikoBG();
            JudgeAnimes[player].Draw();
            Rainbow[player].Draw();
            FlyNotes[player].Draw();
            Combo[player].Draw();
            Roll[player].Draw();
            Balloon[player].Draw();
            ScoreRank[player].Draw();
        }

        base.Draw();
    }
}