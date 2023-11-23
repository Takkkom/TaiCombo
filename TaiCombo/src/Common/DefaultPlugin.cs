using TaiCombo.Plugin;
using TaiCombo.Plugin.Chart;
using TaiCombo.Chart;

namespace TaiCombo.Common;

class DefaultPlugin : IPlugin
{
    public string Name { get; set; }
    public string Version { get; set; }

    public IChartInfo? LoadChart(string fileName)
    {
        string ext = Path.GetExtension(fileName);
        switch(ext)
        {
            case ".tja":
            return new TJALoader(fileName);
        }
        return null;
    }

    public DefaultPlugin()
    {
        Name = "DefaultPlugin";
        Version = "0.0.0-alpha";
    }

    public void Dispose()
    {
        
    }
}