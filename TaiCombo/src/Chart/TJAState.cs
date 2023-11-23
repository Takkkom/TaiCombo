using TaiCombo.Plugin;
using TaiCombo.Plugin.Chart;
using TaiCombo.Plugin.Enums;

namespace TaiCombo.Chart;


struct TJAState
{
    public long NowTime;
    public long NowHBTime;

    public float BPM;
    public float PrevBPM;
    public float Scroll;
    public (float, float) Measure;
    public float TimeGAP;
    public float Delay;
    public TJAScrollType ScrollType;

    public TJAState Copy()
    {
        TJAState state = new()
        {
            NowTime = NowTime,
            NowHBTime = NowHBTime,
            BPM = BPM,
            PrevBPM = PrevBPM,
            Scroll = Scroll,
            Measure = (Measure.Item1, Measure.Item2),
            TimeGAP = TimeGAP,
            Delay = Delay,
            ScrollType = ScrollType
        };
        return state;
    }
}