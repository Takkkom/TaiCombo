using TaiCombo.Plugin.Enums;

namespace TaiCombo.Plugin.Chart;

public class Course
{
    public Course(int level, List<IChip> chips)
    {
        Level = level;
        Chips = chips;
    }

    public int Level { get; set; }

    public List<IChip> Chips { get; set; }
}