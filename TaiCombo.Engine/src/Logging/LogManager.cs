using Silk.NET.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGLES;
using Silk.NET.Windowing.Glfw;
using TaiCombo.Engine.Helpers;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Angle;
using SkiaSharp;
using System.Text;

namespace TaiCombo.Logging;

public class LogManager : IDisposable
{
    private StreamWriter LogFile;

    public LogManager()
    {
        LogFile = new("GameLog.log", false, Encoding.UTF8);
    }

    public void Write(string text)
    {
        LogFile.WriteLine(text);
        LogFile.WriteLine("\n");
    }

    public void Dispose()
    {
        LogFile.Dispose();
    }
}