using Silk.NET.Maths;
using TaiCombo.Plugin;
using TaiCombo.Plugin.Chart;
using TaiCombo.Plugin.Enums;
using TaiCombo.Plugin.Struct;

namespace TaiCombo.Chart;

class TJAChip : IChip
{
    public long Time { get; set; }
    
    public long RollLength { get; set; }

    public ChipType ChipType { get; set; }

    public SENoteType SENoteType { get; set; }

    public BranchType BranchType { get; set; }

    public TJABranchMode BranchMode;

    public int BalloonCount { get; set; }

    public float BranchStart_Expart { get; set; }

    public float BranchStart_Master { get; set; }
    
    public bool Hit { get; set; } = false;
    
    public bool Miss { get; set; } = false;
    
    public bool Active { get; set; } = false;
    
    public bool Over { get; set; } = false;

    public int NowRollCount { get; set; }
    
    public bool IsNote 
    {
        get 
        {
            switch(ChipType)
            {
                case ChipType.Don:
                case ChipType.Ka:
                case ChipType.Don_Big:
                case ChipType.Ka_Big:
                case ChipType.Roll_Start:
                case ChipType.Roll_Big_Start:
                case ChipType.Roll_Balloon_Start:
                case ChipType.Roll_End:
                case ChipType.Roll_Kusudama_Start:
                case ChipType.Roll_Fuse:
                case ChipType.Roll_Don:
                case ChipType.Roll_Ka:
                case ChipType.Roll_Clap:
                return true;
                default:
                return false;
            }
        }
    }

    public long HBTime { get; set; }

    public TJAScrollType ScrollType { get; set; }
    
    public float BPM { get; set; }
    
    public float PrevBPM { get; set; }
    
    public (float, float) Measure { get; set; }
    
    public float Scroll { get; set; }
    
    public float Delay { get; set; }
    
    public bool ChangedBPM { get; set; }
    public float TimeGAP = 0;
    
    private long NowTime;
    private float NowHBTime;
    private static float CurrentBPM = 0;
    private static float CurrentTimeGap = 0;
    private static float CurrentDelay = 0;
    private static float CurrentDelayTotal = 0;

    public void InitHBValue()
    {
        CurrentBPM = BPM;
        CurrentTimeGap = 0;
        CurrentDelay = 0;
        CurrentDelayTotal = 0;
    }

    public BranchType GetNextBranch(BranchStates branchState)
    {
        float value = 0;

        switch(BranchMode)
        {
            case TJABranchMode.Accuracy:
            {
                int noteCount = branchState.Perfect + branchState.Ok + branchState.Miss;
                float perfectRate = branchState.Perfect;
                float okRate = branchState.Ok / 2.0f;
                float goodMiss = 0;
                value = (perfectRate + okRate + goodMiss) / (float)noteCount;
                value *= 100;
            }
            break;
            case TJABranchMode.Roll:
            {
                value = branchState.Roll;
            }
            break;
            case TJABranchMode.Combo:
            {
                value = branchState.Combo;
            }
            break;
            case TJABranchMode.Score:
            {
                value = branchState.Score;
            }
            break;
            default:
            break;
        }
        
        if (value >= BranchStart_Master) return BranchType.Master;
        else if (value >= BranchStart_Expart) return BranchType.Expert;
        else return BranchType.Normal;
    }

    public void Update(long time)
    {
        NowTime = Time - time;
        
        /*
        if (NowTime < 0 && ChipType == ChipType.Delay)
        {
            float prevDelay = CurrentDelay;
            CurrentDelay = Delay * 1000000;
            if (NowTime > -CurrentDelay)
            {
                CurrentDelayTotal = NowTime - prevDelay;
            }
            else 
            {
                CurrentDelayTotal = -CurrentDelay - prevDelay;
            }
        }
        */

        if (ChipType == ChipType.Delay)
        {
            if (Delay > 0)
            {
                float prevDelay = CurrentDelay;
                CurrentDelay = Delay * 1000000;
                if (NowTime < 0)
                {
                    if (NowTime > -CurrentDelay)
                    {
                        CurrentDelayTotal = NowTime - prevDelay;
                    }
                    else 
                    {
                        CurrentDelayTotal = -CurrentDelay - prevDelay;
                    }
                }
            }
            else 
            {
                //CurrentDelayTotal = -CurrentDelay - prevDelay;
            }
        }

        switch(ScrollType)
        {
            case TJAScrollType.BM:
            case TJAScrollType.HB:
            {
                if ((IsNote || ChipType == ChipType.None || ChipType == ChipType.Line || ChipType == ChipType.Line_Branched) && Active)
                {
                    if (NowTime < 0)
                    {
                        if (BPM * (Measure.Item1 / Measure.Item2) > 0)
                        {
                            CurrentBPM = BPM;
                            CurrentTimeGap = TimeGAP;
                        }
                    }
                    NowHBTime = HBTime - ((((time + CurrentDelayTotal) * CurrentBPM) + CurrentTimeGap) / 240.0f);
                }
            }
            break; 
            default:
            {
            }
            break; 
        }
    }

    public Vector2D<float> GetNotePosition()
    {

        Vector2D<float> pos;
        switch(ScrollType)
        {
            case TJAScrollType.BM:
            {
                pos = new(NowHBTime / 1000000.0f, 0.0f);
            }
            break; 
            case TJAScrollType.HB:
            {
                pos = new(NowHBTime * Scroll / 1000000.0f, 0.0f);
            }
            break; 
            default:
            {
                float basePos = NowTime * Scroll * BPM / 240000000.0f;
                pos = new(basePos, 0.0f);
            }
            break; 
        }
        return pos;
    }

    public IChip Copy()
    {
        return (TJAChip)MemberwiseClone();
    }
}