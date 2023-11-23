using System.Text.Json;
using System.Text.Json.Serialization;
using Silk.NET.Windowing;
using TaiCombo.Engine.Enums;


namespace TaiCombo.Common;

class MainConfigValue
{
    public WindowState WindowState { get; set; } = WindowState.Normal;
    public FrameMode FrameMode { get; set; } = FrameMode.VSync;
    public double Framerate { get; set; } = 60;
    public int WindowX { get; set; } = 100;
    public int WindowY { get; set; } = 100;
    public int WindowWidth { get; set; } = 1280;
    public int WindowHeight { get; set; } = 720;
    public string Skin { get; set; } = "SimpleStyle";
    public string Langauge { get; set; } = "ja";
}

class MainConfig
{
    public MainConfigValue Value;

    public const string CONFIGPATH = "MainConfig.json";

    public MainConfig()
    {

    }

    public void Read()
    {
        if (!File.Exists(CONFIGPATH))
        {
            Value = new();
            return;
        }

        using Stream stream = File.OpenRead(CONFIGPATH);
        Value = JsonSerializer.Deserialize<MainConfigValue>(stream);
    }

    public void Write()
    {
        using StreamWriter stream = new StreamWriter(CONFIGPATH);
        stream.Write(JsonSerializer.Serialize(Value));
    }
}