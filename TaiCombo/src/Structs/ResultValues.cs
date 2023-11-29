using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;

namespace TaiCombo.Structs;

struct ResultValues
{
    public float Gauge { get; set; }
    public int Perfect { get; set; }
    public int Ok { get; set; }
    public int Miss { get; set; }
    public int Roll { get; set; }
    public int MaxCombo { get; set; }
    public int Score { get; set; }
    public ScoreRankType ScoreRank { get; set; }
    public ClearType ClearType { get; set; }
}