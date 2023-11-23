using System.Text.Json;
using Silk.NET.Maths;
using TaiCombo.Common;
using TaiCombo.Engine;

namespace TaiCombo.Skin;

class SkinManager
{
    public SkinConfigValue Value { get; private set; }

    public SkinAssets Assets { get; private set; }

    public string SkinPath 
    {
        get 
        {
            return $"Skins{Path.DirectorySeparatorChar}{Game.Config.Value.Skin}{Path.DirectorySeparatorChar}";
        }
    }

    public string GraphicsPath 
    {
        get 
        {
            return $"{SkinPath}Graphics{Path.DirectorySeparatorChar}";
        }
    }

    public string SoundsPath 
    {
        get 
        {
            return $"{SkinPath}Sounds{Path.DirectorySeparatorChar}";
        }
    }

    public SkinManager()
    {

    }

    public void LoadAssets()
    {
        Assets = new();
    }

    public void Terminate()
    {
        Assets.Dispose();
    }

    public void ChangeSkin(string skinName)
    {
        Game.Config.Value.Skin = skinName;
        
        Read();

        GameEngine.Resolution = new Vector2D<int>(Value.Resolution.Width, Value.Resolution.Height);

        Assets?.Dispose();
        Assets = new();
    }

    public void Read()
    {
        if (!File.Exists($"{SkinPath}SkinConfig.json"))
        {
            Value = new();
            return;
        }

        using Stream stream = File.OpenRead($"{SkinPath}SkinConfig.json");
        Value = JsonSerializer.Deserialize<SkinConfigValue>(stream);
    }

    public void Write()
    {
        using StreamWriter stream = new StreamWriter($"{SkinPath}SkinConfig.json");
        stream.Write(JsonSerializer.Serialize(Value));
    }
}