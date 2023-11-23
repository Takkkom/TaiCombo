using TaiCombo.Plugin;
using TaiCombo.Plugin.Chart;

namespace TaiCombo.Common;

class PluginManager
{
    private List<IPlugin> Plugins = new();

    public PluginManager()
    {
    }

    public void Init()
    {
        Plugins.Add(new DefaultPlugin());
    }

    public void Terminate()
    {
        for(int i = 0; i < Plugins.Count; i++)
        {
            Plugins[i].Dispose();
        }
        
        Plugins.Clear();
    }

    public IChartInfo? LoadChart(string filePath)
    {
        for(int i = 0; i < Plugins.Count; i++)
        {
            IChartInfo? chart = Plugins[i].LoadChart(filePath);
            if (chart == null) continue;
            else return chart;
        }
        return null;
    }
}