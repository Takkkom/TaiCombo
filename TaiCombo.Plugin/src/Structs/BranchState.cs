using System.Drawing;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Plugin.Chart;
using TaiCombo.Plugin.Enums;

namespace TaiCombo.Plugin.Struct;

public struct BranchStates
{
    public int Perfect;
    public int Ok;
    public int Miss;
    public int Roll;
    public int Combo;
    public int Score;
}