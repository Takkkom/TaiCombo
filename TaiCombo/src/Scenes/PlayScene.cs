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
using TaiCombo.Plugin.Struct;
using TaiCombo.Chara;

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
    private bool RightSide;

    private float[] NotePixelOffset = new float[Game.MAXPLAYER];

    private void Reset()
    {
        for(int player = 0; player < PlayerCount; player++)
        {
            BalloonMode[player] = false;
            BPM[player] = Chips[player][0].BPM * PlaySpeed;
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
            BranchHold[player] = false;
            
            NoteHelper.BranchChips(Chips[player], 0, BranchType.Normal);
            Event(PlayEventType.ChangeBPM, player);
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

    private Background Background;

    private GoGoSplash GoGoSplash;

    private IPlayerChara[] Charas = new IPlayerChara[Game.MAXPLAYER];

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

    private ClearType[] ClearTypes = new ClearType[Game.MAXPLAYER];
    
    private EndAnime EndAnime;

    private PlayTitle Title;

    private PlayStates[] States = new PlayStates[Game.MAXPLAYER];

    private BranchStates[] BranchStates = new BranchStates[Game.MAXPLAYER];

    private IChip[] CurrentChip = new IChip[Game.MAXPLAYER];

    private IChip?[] CurrentRollChip = new IChip?[Game.MAXPLAYER];

    private HitSound[] HitSound = new HitSound[Game.MAXPLAYER];

    private NamePlate[] NamePlates;

    private float[] NoteAnimeCounter = new float[Game.MAXPLAYER];

    private float[] RollCloseCounter = new float[Game.MAXPLAYER];

    private int[] NoteFrame = new int[Game.MAXPLAYER];

    private bool[] Branchable = new bool[Game.MAXPLAYER];

    private bool[] BranchHold = new bool[Game.MAXPLAYER];

    private bool[] Cleared = new bool[Game.MAXPLAYER];

    private bool[] Maxed = new bool[Game.MAXPLAYER];

    private bool[] Missed = new bool[Game.MAXPLAYER];

    private int[] CurrentMissCount = new int[Game.MAXPLAYER];

    private float[] BPM = new float[Game.MAXPLAYER];

    private List<Action>[] RollProcesss = new List<Action>[Game.MAXPLAYER] { new List<Action>(), new List<Action>() };

    private bool[] BalloonMode = new bool[Game.MAXPLAYER];

    private void StartAutoRoll(int player)
    {
        Task.Run(() => 
        {
            if (CurrentRollChip[player] == null) return;
            bool balloon = CurrentRollChip[player].ChipType == ChipType.Roll_Balloon_Start || CurrentRollChip[player].ChipType == ChipType.Roll_Kusudama_Start;

            int interval;
            if (balloon)
            {
                interval = (int)Math.Ceiling(CurrentRollChip[player].RollLength / Math.Max(CurrentRollChip[player].BalloonCount, 1) / PlaySpeed / 1000);
            }
            else 
            {
                interval = 1000 / 16;
            }

            while(CurrentRollChip[player] != null)
            {
                if (Playing)
                {
                    RollProcesss[player].Add(() => 
                    {
                        HitTaiko(player, TaikoType.DonRight);
                    });
                }

                if (CurrentRollChip[player] == null) break;
                Thread.Sleep(interval);
            }
        });
    }

    private void SetNormalCharaAnime(int player)
    {
        if (GoGoTime[player])
        {
            Charas[player].ChangeAnime(CharaAnimeType.GoGo, player, true);
        }
        else 
        {
            if (CurrentMissCount[player] >= 6)
            {
                Charas[player].ChangeAnime(CharaAnimeType.Miss_Down, player, true);
            }
            else if (Missed[player])
            {
                Charas[player].ChangeAnime(CharaAnimeType.Miss, player, true);
            }
            else if (Cleared[player])
            {
                Charas[player].ChangeAnime(CharaAnimeType.Clear, player, true);
            }
            else
            {
                Charas[player].ChangeAnime(CharaAnimeType.Normal, player, true);
            }
        }
    }

    private void Event(PlayEventType playEventType, int player)
    {
        switch(playEventType)
        {
            case PlayEventType.ChangeBPM:
            {
                Background.SetBPM(BPM);
            }
            break;
            case PlayEventType.GoGoStart:
            {
                Lane[player].GoGoIn();
                if (PlayerCount == 1) 
                {
                    GoGoSplash.Start();
                }
                
                if (!BalloonMode[player]) Charas[player].ChangeAnime(CharaAnimeType.GoGoStart, player, false, () => SetNormalCharaAnime(player));
            }
            break;
            case PlayEventType.GoGoEnd:
            {
                Lane[player].GoGoOut();
                if (!BalloonMode[player]) SetNormalCharaAnime(player);
            }
            break;
            case PlayEventType.ClearIn:
            {
                Background.ClearIn(player);
                if (!BalloonMode[player]) Charas[player].ChangeAnime(CharaAnimeType.ClearIn, player, false, () => SetNormalCharaAnime(player));
            }
            break;
            case PlayEventType.ClearOut:
            {
                Background.ClearOut(player);
                if (!BalloonMode[player]) Charas[player].ChangeAnime(CharaAnimeType.ClearOut, player, false, () => SetNormalCharaAnime(player));
            }
            break;
            case PlayEventType.SoulIn:
            {
                Background.MaxIn(player);
                if (!BalloonMode[player]) Charas[player].ChangeAnime(CharaAnimeType.SoulIn, player, false, () => SetNormalCharaAnime(player));
            }
            break;
            case PlayEventType.SoulOut:
            {
                Background.MaxOut(player);
            }
            break;
            case PlayEventType.Return:
            {
                if (!GoGoTime[player] && !BalloonMode[player]) Charas[player].ChangeAnime(CharaAnimeType.Return, player, false, () => SetNormalCharaAnime(player));
            }
            break;
            case PlayEventType.Roll:
            {
                Background.AddRollEffect(player);
            }
            break;
            case PlayEventType.Balloon_Breaking:
            {
                Balloon[player].Open();
                Balloon[player].SetNumber(CurrentRollChip[player].BalloonCount - CurrentRollChip[player].NowRollCount);
                Charas[player].ChangeAnime(CharaAnimeType.Balloon_Breaking, player, false);
                if (!BalloonMode[player]) 
                {
                    BalloonMode[player] = true;
                }
            }
            break;
            case PlayEventType.Balloon_Broke:
            {
                Rainbow[player].Open();
                FlyNotes[player].Add(FlyNoteType.Don);
                Balloon[player].Broke();
                Charas[player].ChangeAnime(CharaAnimeType.Balloon_Broke, player, false, () => 
                {
                    BalloonMode[player] = false;
                    SetNormalCharaAnime(player);
                });
                
                Game.Skin.Assets.Play_Balloon_Broke_Sound.Play();
            }
            break;
            case PlayEventType.Balloon_Miss:
            {
                Balloon[player].Miss();
                Charas[player].ChangeAnime(CharaAnimeType.Balloon_Miss, player, false, () => 
                {
                    BalloonMode[player] = false;
                    SetNormalCharaAnime(player);
                });
            }
            break;
            case PlayEventType.End:
            {
                if (Cleared[player])
                {
                    if (States[player].Ok == 0 && States[player].Miss == 0)
                    {
                        Charas[player].ChangeAnime(CharaAnimeType.Jump_Max, player, false, () => SetNormalCharaAnime(player));
                        EndAnime.PlayAnime(ClearType.AllPerfect, player);
                        ClearTypes[player] = ClearType.AllPerfect;
                    }
                    else if (States[player].Ok == 0)
                    {
                        Charas[player].ChangeAnime(CharaAnimeType.Jump_Max, player, false, () => SetNormalCharaAnime(player));
                        EndAnime.PlayAnime(ClearType.FullCombo, player);
                        ClearTypes[player] = ClearType.FullCombo;
                    }
                    else
                    {
                        Charas[player].ChangeAnime(CharaAnimeType.ClearIn, player, false, () => SetNormalCharaAnime(player));
                        EndAnime.PlayAnime(ClearType.Clear, player);
                        ClearTypes[player] = ClearType.Clear;
                    }
                }
                else 
                {
                    Charas[player].ChangeAnime(CharaAnimeType.ClearOut, player, false, () => SetNormalCharaAnime(player));
                    EndAnime.PlayAnime(ClearType.None, player);
                    ClearTypes[player] = ClearType.None;
                }

                if (player == 0)
                {
                    Task.Run(() => 
                    {
                        Thread.Sleep(8000);
                        GameEngine.ASyncActions.Add(() => 
                        {
                            Game.Fade.StartFade(Game.Skin.Assets.Fade_Black, 1.5f, 0.5f, () =>
                            {
                                GameEngine.SceneManager_.ChangeScene(new ResultScene(new () 
                                { 
                                    { "PlayerCount", PlayerCount },
                                    { "RightSide", RightSide },
                                    { "GaugeType", GaugeType },
                                    { "NamePlates", NamePlates },
                                    { "Title", Chart.Title },
                                    { "SubTitle", Chart.SubTitle },
                                    { "Options", Options },
                                    {
                                        "Values", new ResultValues[2] { 
                                            new ResultValues() 
                                            { 
                                                Gauge = States[0].Gauge,
                                                Perfect = States[0].Perfect,
                                                Ok = States[0].Ok,
                                                Miss = States[0].Miss,
                                                Roll = States[0].Roll,
                                                MaxCombo = States[0].MaxCombo,
                                                Score = States[0].Score,
                                                ScoreRank = States[0].ScoreRank,
                                                ClearType = ClearTypes[0]
                                            },
                                            new ResultValues() 
                                            { 
                                                Gauge = States[1].Gauge,
                                                Perfect = States[1].Perfect,
                                                Ok = States[1].Ok,
                                                Miss = States[1].Miss,
                                                Roll = States[1].Roll,
                                                MaxCombo = States[1].MaxCombo,
                                                Score = States[1].Score,
                                                ScoreRank = States[1].ScoreRank,
                                                ClearType = ClearTypes[1]
                                            }  
                                        }
                                    }
                                }));
                            });
                        });
                    });
                }
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

                if (!Branchable[player] && chip.ChipType == ChipType.BranchStart)
                {
                    Branchable[player] = true;
                }

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
                CurrentMissCount[player] = 0;

                BranchStates[player].Perfect++;
                BranchStates[player].Combo++;
                
                Gauge[player].SetValue(States[player].Gauge);
                TaikoUI[player].SetCombo(States[player].Combo);
                TaikoUI[player].AddScore(AddScores[player].Perfect);
                TaikoUI[player].SetScore(States[player].Score);

                HitExplosion[player].AddEffect(chip.ChipType == ChipType.Don_Big || chip.ChipType == ChipType.Ka_Big ? 2 : 0);
                
                if (Missed[player])
                {
                    Missed[player] = false;
                    Event(PlayEventType.Return, player);
                }
            }
            break;
            case JudgeType.Ok:
            {
                States[player].Ok++;
                States[player].Score += GoGoTime[player] ? ScoreHelper.GetValue(AddScores[player].Ok, AddScores[player].GoGoRate) : AddScores[player].Ok;
                States[player].Gauge += AddGauges[player].Ok;
                States[player].Gauge = Math.Min(Math.Max(States[player].Gauge, 0), 100);
                States[player].Combo++;
                CurrentMissCount[player] = 0;
                
                BranchStates[player].Ok++;
                BranchStates[player].Combo++;
                
                Gauge[player].SetValue(States[player].Gauge);
                TaikoUI[player].SetCombo(States[player].Combo);
                TaikoUI[player].AddScore(AddScores[player].Ok);
                TaikoUI[player].SetScore(States[player].Score);
                
                HitExplosion[player].AddEffect(chip.ChipType == ChipType.Don_Big || chip.ChipType == ChipType.Ka_Big ? 3 : 1);
                
                if (Missed[player])
                {
                    Missed[player] = false;
                    Event(PlayEventType.Return, player);
                }
            }
            break;
            case JudgeType.Miss:
            {
                States[player].Miss++;
                States[player].Score += GoGoTime[player] ? ScoreHelper.GetValue(AddScores[player].Miss, AddScores[player].GoGoRate) : AddScores[player].Miss;
                States[player].Gauge += AddGauges[player].Miss;
                States[player].Gauge = Math.Min(Math.Max(States[player].Gauge, 0), 100);
                States[player].Combo = 0;
                CurrentMissCount[player]++;
                
                BranchStates[player].Miss = 0;
                BranchStates[player].Combo = 0;
                
                Gauge[player].SetValue(States[player].Gauge);
                TaikoUI[player].SetCombo(States[player].Combo);
                TaikoUI[player].AddScore(AddScores[player].Miss);
                TaikoUI[player].SetScore(States[player].Score);

                if (CurrentMissCount[player] == 6)
                {
                    if (!GoGoTime[player]) SetNormalCharaAnime(player);
                }

                if (!Missed[player])
                {
                    Missed[player] = true;
                    if (!GoGoTime[player]) SetNormalCharaAnime(player);
                }
            }
            break;
            case JudgeType.Roll:
            {
                States[player].Roll++;
                States[player].Score += AddScores[player].Roll;
                BranchStates[player].Roll++;
                BranchStates[player].Score += AddScores[player].Roll;
                TaikoUI[player].AddScore(AddScores[player].Roll);
                TaikoUI[player].SetScore(States[player].Score);

                Roll[player].Open(false);
                Roll[player].SetNumber(CurrentRollChip[player].NowRollCount);

                Event(PlayEventType.Roll, player);
                
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
                BranchStates[player].Roll++;
                BranchStates[player].Roll += AddScores[player].Roll_Big;
                TaikoUI[player].AddScore(AddScores[player].Roll_Big);
                TaikoUI[player].SetScore(States[player].Score);

                Roll[player].Open(false);
                Roll[player].SetNumber(CurrentRollChip[player].NowRollCount);

                Event(PlayEventType.Roll, player);
                
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
                BranchStates[player].Roll++;
                BranchStates[player].Score += AddScores[player].Balloon_Roll;
                TaikoUI[player].AddScore(AddScores[player].Balloon_Roll);
                TaikoUI[player].SetScore(States[player].Score);

                Event( PlayEventType.Balloon_Breaking, player);

                CurrentRollChip[player].Hit = true;
            }
            break;
            case JudgeType.BalloonBreak:
            {
                CurrentRollChip[player] = null;
                States[player].Roll++;
                States[player].Score += AddScores[player].Balloon_Broke;
                BranchStates[player].Roll++;
                BranchStates[player].Score += AddScores[player].Balloon_Broke;
                TaikoUI[player].AddScore(AddScores[player].Balloon_Broke);
                TaikoUI[player].SetScore(States[player].Score);

                Event( PlayEventType.Balloon_Broke, player);
            }
            break;
            case JudgeType.Kusudama:
            {
                States[player].Roll++;
                States[player].Score += AddScores[player].Kusudama_Roll;
                BranchStates[player].Roll++;
                BranchStates[player].Score += AddScores[player].Kusudama_Roll;
                TaikoUI[player].AddScore(AddScores[player].Kusudama_Roll);
                TaikoUI[player].SetScore(States[player].Score);
            }
            break;
            case JudgeType.KusudamaBreak:
            {
                States[player].Roll++;
                States[player].Score += AddScores[player].Kusudama_Broke;
                BranchStates[player].Roll++;
                BranchStates[player].Score += AddScores[player].Kusudama_Broke;
                TaikoUI[player].AddScore(AddScores[player].Kusudama_Broke);
                TaikoUI[player].SetScore(States[player].Score);
            }
            break;
        }

        UpdateScoreRank(player);

        if (Cleared[player])
        {
            if (States[player].Gauge < GaugeHelper.ClearLine[GaugeType[player]])
            {
                Event(PlayEventType.ClearOut, player);
                Cleared[player] = false;
            }
        }
        else
        {
            if (States[player].Gauge >= GaugeHelper.ClearLine[GaugeType[player]])
            {
                Event(PlayEventType.ClearIn, player);
                Cleared[player] = true;
            }
        }

        if (Maxed[player])
        {
            if (States[player].Gauge < 100)
            {
                Event(PlayEventType.SoulOut, player);
                Maxed[player] = false;
            }
        }
        else
        {
            if (States[player].Gauge >= 100)
            {
                Event(PlayEventType.SoulIn, player);
                Maxed[player] = true;
            }
        }

        if (hit)
        {
            if (judge != JudgeType.Miss)
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
            }

            if (States[player].Combo % 10 == 0 && States[player].Combo >= 10 && !GoGoTime[player] && !BalloonMode[player])
            {
                if (Maxed[player])
                {
                    Charas[player].ChangeAnime(CharaAnimeType.Jump_Max, player, false, () => SetNormalCharaAnime(player));
                }
                else 
                {
                    Charas[player].ChangeAnime(CharaAnimeType.Jump, player, false, () => SetNormalCharaAnime(player));
                }
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
                    JudgeType judge = Options[player].AutoPlay ? JudgeType.Perfect : NoteHelper.GetJudge(Math.Abs(nowTime), JudgeZones[player]); 

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
                StartAutoRoll(player);

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Big_Start:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;
                StartAutoRoll(player);

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Balloon_Start:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;
                StartAutoRoll(player);

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Kusudama_Start:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;
                StartAutoRoll(player);

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Fuse:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;
                StartAutoRoll(player);

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Don:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;
                StartAutoRoll(player);

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Ka:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;
                StartAutoRoll(player);

                chip.Over = true;
            }
            break;
            case ChipType.Roll_Clap:
            if (nowTime < 0 && !chip.Over)
            {
                CurrentRollChip[player] = chip;
                StartAutoRoll(player);

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
                        Event( PlayEventType.Balloon_Miss, player);
                        break;
                    }
                }
                CurrentRollChip[player] = null;

                chip.Over = true;
            }
            break;
            case ChipType.BPMChange:
            if (nowTime < 0 && !chip.Over)
            {
                BPM[player] = chip.BPM * PlaySpeed;
                Event(PlayEventType.ChangeBPM, player);
                chip.Over = true;
            }
            break;
            case ChipType.GoGoStart:
            if (nowTime < 0 && !chip.Over)
            {
                GoGoTime[player] = true;
                Event(PlayEventType.GoGoStart, player);

                chip.Over = true;
            }
            break;
            case ChipType.GoGoEnd:
            if (nowTime < 0 && !chip.Over)
            {
                GoGoTime[player] = false;
                Event(PlayEventType.GoGoEnd, player);

                chip.Over = true;
            }
            break;
            case ChipType.BranchStart:
            if (nowTime < 0 && !chip.Over)
            {
                BranchType branchType = chip.GetNextBranch(BranchStates[player]);
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
            case ChipType.BranchSection:
            if (nowTime < 0 && !chip.Over && chip.Active)
            {
                BranchStates[player] = new();
                chip.Over = true;
            }
            break;
            case ChipType.BranchHold:
            if (nowTime < 0 && !chip.Over && chip.Active)
            {
                BranchHold[player] = true;
                chip.Over = true;
            }
            break;
            case ChipType.End:
            if (nowTime < 0 && !chip.Over && chip.Active)
            {
                Event(PlayEventType.End, player);
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
        RightSide = (bool)args["RightSide"];
        NamePlates = (NamePlate[])args["NamePlates"];

        for(int player = 0; player < PlayerCount; player++)
        {
            Charas[player] = Game.Skin.Assets.Characters["0"];
            Charas[player].LoadAssets();
            HitSound[player] = Game.Skin.Assets.HitSounds[Options[player].HitSound];
            NotePixelOffset[player] = (Options[player].Offset / 5.0f) * (Game.Skin.Value.Play_Notes.Padding / 36);
        }

        LoadChart(ChartPath);

        Background = new(PlayerCount, RightSide);
        GoGoSplash = new();
        EndAnime = new(GameMode, PlayerCount);
        Title = new(Chart.Title, "ジャンルはまだ未対応", "Default", 1, -1);

        for(int player = 0; player < PlayerCount; player++)
        {
            switch(GameMode)
            {
                case GameModeType.Play:
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
                }
                break;
                case GameModeType.Training:
                {
                    GaugeType[player] = Enums.GaugeType.None;
                }
                break;
                case GameModeType.Dan:
                {
                    GaugeType[player] = Enums.GaugeType.Dan;
                }
                break;
                case GameModeType.Tower:
                {
                    GaugeType[player] = Enums.GaugeType.None;
                }
                break;
            }

            int taikoSide = RightSide ? 1 : player;
            TaikoUI[player] = new(player, taikoSide, (int)Courses[player], NamePlates[player], Options[player]);
            Lane[player] = new(player, Branchable[player]);
            Gauge[player] = new(GaugeType[player], player, taikoSide);
            HitExplosion[player] = new(player);
            JudgeAnimes[player] = new(player);
            Rainbow[player] = new(player);
            FlyNotes[player] = new(player);
            Combo[player] = new(player, taikoSide);
            Roll[player] = new(player);
            Balloon[player] = new(player);
            ScoreRank[player] = new(player);
            JudgeZones[player] = NoteHelper.JudgeZones[Courses[player]];
            AddScores[player] = ScoreHelper.GetAddScores(Chips[player], ScoreType.Gen4);
            AddGauges[player] = GaugeHelper.GetAddGauge(Chips[player], Courses[player], GaugeType[player], Chart.Courses[Courses[player]].Level);
            
            Event(PlayEventType.ChangeBPM, player);
        }

        NowTime = -1500000;
    }

    private void DrawNotes()
    {
        for(int player = 0; player < PlayerCount; player++)
        {
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
        }
    }

    public override void Activate()
    {
        Reset();
        Start();

        for(int player = 0; player < PlayerCount; player++)
        {
            SetNormalCharaAnime(player);
        }

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
                //Reset();
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
        else if (GameEngine.Input.GetKeyPressing(Silk.NET.Input.Key.Number0))
        {
            Event(PlayEventType.End, 0);
        }
        /*
        else if (GameEngine.Input.GetKeyPressing(Silk.NET.Input.Key.Number1))
        {
            EndAnime.PlayAnime(ClearType.None, 0);
            EndAnime.PlayAnime(ClearType.None, 1);
        }
        else if (GameEngine.Input.GetKeyPressing(Silk.NET.Input.Key.Number2))
        {
            EndAnime.PlayAnime(ClearType.Clear, 0);
            EndAnime.PlayAnime(ClearType.Clear, 1);
        }
        else if (GameEngine.Input.GetKeyPressing(Silk.NET.Input.Key.Number3))
        {
            EndAnime.PlayAnime(ClearType.FullCombo, 0);
            EndAnime.PlayAnime(ClearType.FullCombo, 1);
        }
        else if (GameEngine.Input.GetKeyPressing(Silk.NET.Input.Key.Number4))
        {
            EndAnime.PlayAnime(ClearType.AllPerfect, 0);
            EndAnime.PlayAnime(ClearType.AllPerfect, 1);
        }
        */
        
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

        Background.SetGauge(new float[] { States[0].Gauge, States[1].Gauge } );
        Background.Update();
        GoGoSplash.Update();

        EndAnime.Update();

        for(int player = 0; player < PlayerCount; player++)
        {

            if (Options[player].AutoPlay)
            {
                /*
                if (Playing && CurrentRollChip[player] != null)
                {
                    HitTaiko(player, TaikoType.DonRight);
                }
                */
                if (RollProcesss[player].Count >= 1) 
                {
                    RollProcesss[player][0]?.Invoke(); 
                    RollProcesss[player].RemoveAt(0);
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
            
            Charas[player].Update(BPM[player], CharaSceneType.Play, player);
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

            NoteAnimeCounter[player] += (BPM[player] / 60.0f) * GameEngine.Time_.DeltaTime;
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
                    if (Playing && NoteHelper.IsHittableNote(chip.ChipType) && !chip.Miss && !chip.Hit && chip.Active && nowTime < 0 && judge == JudgeType.None)
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

        Title.Update();

        base.Update();
    }

    public override void Draw()
    {
        Background.Draw();
        GoGoSplash.Draw();
        
        for(int player = 0; player < PlayerCount; player++)
        {
            Lane[player].Draw();
            Gauge[player].Draw(Game.Skin.Value.Play_Gauge.Pos[player].X, Game.Skin.Value.Play_Gauge.Pos[player].Y, 1.0f);
        }

        DrawNotes();

        for(int player = 0; player < PlayerCount; player++)
        {
            if (!BalloonMode[player]) Charas[player].Draw(Game.Skin.Value.Play_Chara[player].X, Game.Skin.Value.Play_Chara[player].Y, 1.0f, false, false, CharaSceneType.Play, 1, player);
            
            TaikoUI[player].Draw();
            
            if (BalloonMode[player]) Charas[player].Draw(Game.Skin.Value.Play_Balloon_Chara[player].X, Game.Skin.Value.Play_Balloon_Chara[player].Y, 1, false, false, CharaSceneType.Play_Balloon, 1, player);
            
            HitExplosion[player].DrawAfterTaikoBG();
            JudgeAnimes[player].Draw();
            Rainbow[player].Draw();
            FlyNotes[player].Draw();
            Combo[player].Draw();
            Roll[player].Draw();
            Balloon[player].Draw();
            ScoreRank[player].Draw();
        }

        EndAnime.Draw();

        Title.Draw();

        base.Draw();
    }
}