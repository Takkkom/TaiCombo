using System.Runtime.InteropServices;

namespace TaiCombo.Engine;

class LibManager
{
    public LibManager()
    {
        string path = $"Libs{Path.DirectorySeparatorChar}";
        if (OperatingSystem.IsWindows())
        {
            path += "win-";
        }
        else if (OperatingSystem.IsLinux())
        {
            path += "linux-";
        }
        else if (OperatingSystem.IsMacOS())
        {
            path += "mac-";
        }
        
		switch (RuntimeInformation.ProcessArchitecture)
		{
            case Architecture.X64:
            path += "x64";
            break;
            case Architecture.X86:
            path += "x86";
            break;
            case Architecture.Arm:
            path += "arm";
            break;
            case Architecture.Arm64:
            path += "arm64";
            break;
        }

        DirectoryInfo info = new DirectoryInfo(AppContext.BaseDirectory + path);
        foreach(var fileInfo in info.GetFiles())
        {
            fileInfo.CopyTo(AppContext.BaseDirectory + fileInfo.Name, true);
        }
    }
}