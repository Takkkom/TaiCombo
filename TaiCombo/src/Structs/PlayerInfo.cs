using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;

namespace TaiCombo.Structs;

struct PlayerInfo
{
    public string Name { get; set; }
    public TitleInfo? TitleInfo { get; set; }
    public DanInfo? DanInfo { get; set; }
}