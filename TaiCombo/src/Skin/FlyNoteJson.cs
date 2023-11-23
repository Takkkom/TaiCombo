using System.Text.Json.Serialization;

namespace TaiCombo.Skin;

public class FlyNoteJson
{
    public PosJson[] Begin { get; set; }
    public PosJson[] End { get; set; }

    [JsonIgnore]
    public float Value { get; set; }
    
    public string[] MoveScriptX { get; set; }
    public string[] MoveScriptY { get; set; }
}