using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;

namespace TaiCombo.Structs;

struct PlayStates
{
    public int Perfect;
    public int Ok;
    public int Miss;
    public int Score;
    public int Roll;
    public int Combo;
    public int MaxCombo;
    public float Gauge;
    public ScoreRankType ScoreRank;
}