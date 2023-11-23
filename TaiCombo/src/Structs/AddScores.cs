using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;

namespace TaiCombo.Structs;

struct AddScores
{
    public int Perfect;
    public int Ok;
    public int Miss;

    public int Roll;
    public int Roll_Big;

    public int Balloon_Roll;
    public int Balloon_Broke;
    
    public int Kusudama_Roll;
    public int Kusudama_Broke;

    public float GoGoRate;
}