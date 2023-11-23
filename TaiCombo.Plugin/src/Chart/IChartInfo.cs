using TaiCombo.Plugin.Enums;

namespace TaiCombo.Plugin.Chart;

public interface IChartInfo
{
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Audio { get; set; }
    public string PreImage { get; set; }
    public float Offset { get; set; }
    public List<string> Creator { get; set; }
    public List<string> Charter { get; set; }
    public Dictionary<CourseType, Course> Courses { get; set; }
}