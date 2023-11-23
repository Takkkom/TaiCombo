using TaiCombo.Plugin.Chart;

namespace TaiCombo.Plugin;

public interface IPlugin : IDisposable
{
    public string Name { get; set; }
    public string Version { get; set; }

    public IChartInfo? LoadChart(string filePath);
}
