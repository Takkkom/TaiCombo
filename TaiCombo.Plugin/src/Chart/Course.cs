using TaiCombo.Plugin.Enums;

namespace TaiCombo.Plugin.Chart;

public class Course
{
    public Course(float level, List<IChip> chips)
    {
        Level = level;
        Chips = chips;
    }

    public float Level { get; set; }

    public List<IChip> Chips { get; set; }
}