using Silk.NET.Maths;

using TaiCombo.Plugin.Enums;

namespace TaiCombo.Plugin.Chart;

public interface IChip
{
    public long Time { get; set; }

    public long RollLength { get; set; }

    public ChipType ChipType { get; set; }

    public SENoteType SENoteType { get; set; }

    public BranchType BranchType { get; set; }

    public int BalloonCount { get; set; }
    
    public bool IsNote { get; }
    
    public float BPM { get; }
    
    public bool Hit { get; set; }
    
    public bool Miss { get; set; }
    
    public bool Over { get; set; }
    
    public bool Active { get; set; }

    public int NowRollCount { get; set; }


    public void InitHBValue();
    
    public void Update(long time);

    public Vector2D<float> GetNotePosition();

    public IChip Copy();
}
