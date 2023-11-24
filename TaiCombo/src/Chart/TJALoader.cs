using System.Text;
using TaiCombo.Plugin;
using TaiCombo.Plugin.Chart;
using TaiCombo.Plugin.Enums;
using TaiCombo.Plugin.Helper;

namespace TaiCombo.Chart;


class TJALoader : IChartInfo
{
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string Audio { get; set; } = "";
    public string PreImage { get; set; } = "";
    public float Offset { get; set; } = 0.0f;
    public List<string> Creator { get; set; } = new();
    public List<string> Charter { get; set; } = new();
    public Dictionary<CourseType, Course> Courses { get; set; } = new();

    private float InitBPM = 150;
    private int Level;
    private List<int> Balloons = new();
    private List<IChip> Chips = new();
    private List<TJAChip> CurrentChips = new();
    private TJAScrollType InitScrollType;

    private CourseType CurrentCourseType = CourseType.Easy;

    private bool LoadingChart = false;

    private int CurrentNoteCount = 0;

    private TJAState State;
    private TJAState BranchState;

    private BranchType CurrentBranch;

    private int BalloonCount = 0;

    private ChipType CharToChipType(char ch)
    {
        switch(ch)
        {
            case '0':
            return ChipType.None;
            case '1':
            return ChipType.Don;
            case '2':
            return ChipType.Ka;
            case '3':
            return ChipType.Don_Big;
            case '4':
            return ChipType.Ka_Big;
            case '5':
            return ChipType.Roll_Start;
            case '6':
            return ChipType.Roll_Big_Start;
            case '7':
            return ChipType.Roll_Balloon_Start;
            case '8':
            return ChipType.Roll_End;
            case '9':
            return ChipType.Roll_Kusudama_Start;
            case 'A':
            return ChipType.Don_Big_Joint;
            case 'B':
            return ChipType.Ka_Big_Joint;
            case 'C':
            return ChipType.Mine;
            case 'D':
            return ChipType.Roll_Fuse;
            case 'E':
            return ChipType.None;
            case 'F':
            return ChipType.Adlib;
            case 'G':
            return ChipType.Purple;
            case 'H':
            return ChipType.Roll_Don;
            case 'I':
            return ChipType.Roll_Ka;
            default:
            return ChipType.Invalid;
        }
    }

    private CourseType StringToCourseType(string value)
    {
        switch(value.ToLower())
        {
            case "0":
            case "easy":
            return CourseType.Easy;
            case "1":
            case "normal":
            return CourseType.Normal;
            case "2":
            case "hard":
            return CourseType.Hard;
            case "3":
            case "oni":
            return CourseType.Oni;
            case "4":
            case "edit":
            return CourseType.Edit;
            default:
            return CourseType.Easy;
        }
    }

    private void SetHeader(string key, string value)
    {
        switch(key)
        {
            case "TITLE":
            Title = value;
            break;
            case "SUBTITLE":
            SubTitle = value;
            break;
            case "WAVE":
            Audio = value;
            break;
            case "PREIMAGE":
            PreImage = value;
            break;
            case "BPM":
            if (float.TryParse(value, out float bpm))
            {
                InitBPM = bpm;
            }
            break;
            case "OFFSET":
            if (float.TryParse(value, out float offset))
            {
                Offset = offset;
            }
            break;
            case "MAKER":
            Charter.Add(value);
            break;
            case "COURSE":
            {
                CurrentCourseType = StringToCourseType(value);
            }
            break;
            case "LEVEL":
            {
                if (int.TryParse(value, out int level))
                {
                    Level = level;
                }
                else 
                {
                    Level = 1;
                }
            }
            break;
            case "BALLOON":
            {
                string[] balloons = value.Split(',');
                for(int i = 0; i < balloons.Length; i++)
                {
                    if (!int.TryParse(balloons[i], out int balloonCount))
                    {
                        balloonCount = 5;
                    }
                    Balloons.Add(balloonCount);
                }
            }
            break;
        }
    }

    private bool CurrentBPMChanged;

    private long PrevNoteTime = 0;

    private TJAChip? PrevNote;

    private bool BarLineOn;

    public void SetCommand(string text)
    {
        if (text == "#START")
        {
            State = new()
            {
                NowTime = 0,
                BPM = InitBPM,
                PrevBPM = InitBPM,
                Scroll = 1.0f,
                Measure = (4.0f, 4.0f),
                ScrollType = State.ScrollType
            };
            Chips = new();
            Courses.Add(CurrentCourseType, new Course(Level, Chips));
            LoadingChart = true;
            BarLineOn = true;

            CurrentBranch = BranchType.Normal;
        }
        else if (text == "#END")
        {
            ChartHelper.PostProcess(Chips);
            LoadingChart = false;
        }
        else if (text.StartsWith("#BPMCHANGE"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.BPMChange;

            if (float.TryParse(text.Remove(0, 10), out float bpm))
            {
                chip.BPM = bpm;
            }
            else 
            {
                chip.BPM = 150.0f;
            }

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#MEASURE"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.MeasureChange;
            var split = text.Remove(0, 8).Split('/');
            if (!float.TryParse(split[0], out float measure1)) measure1 = 4;
            if (!float.TryParse(split[1], out float measure2)) measure2 = 4;
            
            chip.Measure = (measure1, measure2);

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#SCROLL"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.ScrollChange;
            
            if (float.TryParse(text.Remove(0, 7), out float scroll))
            {
                chip.Scroll = scroll;
            }
            else 
            {
                chip.Scroll = 1.0f;
            }

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#DELAY"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.Delay;
            
            if (float.TryParse(text.Remove(0, 6), out float delay))
            {
                chip.Delay = delay;
            }
            else 
            {
                chip.Delay = 0.0f;
            }

            if (Math.Abs(chip.Delay) > 0.01) CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#GOGOSTART"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.GoGoStart;

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#GOGOEND"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.GoGoEnd;

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#BARLINEON"))
        {
            BarLineOn = true;
        }
        else if (text.StartsWith("#BARLINEOFF"))
        {
            BarLineOn = false;
        }
        else if (text.StartsWith("#BRANCHSTART"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.BranchStart;
            
            string[] values = text.Remove(0, 12).Split(',');
            if (values[0].Contains("p"))
            {
                chip.BranchMode = TJABranchMode.Accuracy;
            }
            else if (values[0].Contains("r"))
            {
                chip.BranchMode = TJABranchMode.Roll;
            }
            else if (values[0].Contains("s"))
            {
                chip.BranchMode = TJABranchMode.Score;
            }
            else if (values[0].Contains("c"))
            {
                chip.BranchMode = TJABranchMode.Combo;
            }

            if (float.TryParse(values[1], out float value1)) chip.BranchStart_Expart = value1;
            if (float.TryParse(values[2], out float value2)) chip.BranchStart_Master = value2;


            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#BRANCHEND"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.BranchEnd;

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#SECTION"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.BranchSection;

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#LEVELHOLD"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.BranchHold;

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#N"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.Branch_N;

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#E"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.Branch_E;

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#M"))
        {
            TJAChip chip = new();
            chip.ChipType = ChipType.Branch_M;

            CurrentChips.Add(chip);
        }
        else if (text.StartsWith("#NMSCROLL"))
        {
            State.ScrollType = TJAScrollType.Normal;
        }
        else if (text.StartsWith("#BMSCROLL"))
        {
            State.ScrollType = TJAScrollType.BM;
        }
        else if (text.StartsWith("#HBSCROLL"))
        {
            State.ScrollType = TJAScrollType.HB;
        }
        else if (LoadingChart && text[0] != '#') 
        {
            for(int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (ch == ',')
                {
                    if (CurrentNoteCount == 0)
                    {
                        TJAChip chip = new()
                        {
                            ChipType = ChipType.None,
                            ScrollType = State.ScrollType
                        };
                        CurrentChips.Add(chip);
                        CurrentNoteCount++;
                        
                        if (BarLineOn)
                        {
                            TJAChip lineChip = new();
                            lineChip.ChipType = ChipType.Line;
                            lineChip.ScrollType = lineChip.ScrollType;
                            CurrentChips.Add(lineChip);
                        }
                    }
                    for(int j = 0; j < CurrentChips.Count; j++)
                    {
                        TJAChip chip = CurrentChips[j];
                        chip.Time = State.NowTime;
                        chip.HBTime = State.NowHBTime;
                        chip.ScrollType = State.ScrollType;


                        switch(chip.ChipType)
                        {
                            case ChipType.None:
                            case ChipType.Don:
                            case ChipType.Ka:
                            case ChipType.Don_Big:
                            case ChipType.Ka_Big:
                            case ChipType.Roll_Start:
                            case ChipType.Roll_Big_Start:
                            case ChipType.Roll_Balloon_Start:
                            case ChipType.Roll_End:
                            case ChipType.Roll_Kusudama_Start:
                            case ChipType.Don_Big_Joint:
                            case ChipType.Ka_Big_Joint:
                            case ChipType.Mine:
                            case ChipType.Roll_Fuse:
                            case ChipType.Adlib:
                            case ChipType.Purple:
                            case ChipType.Clap:
                            case ChipType.Roll_Don:
                            case ChipType.Roll_Ka:
                            {
                                PrevNote = chip;
                                PrevNoteTime = State.NowTime;
                                chip.BPM = State.BPM;
                                chip.PrevBPM = State.PrevBPM;
                                chip.Scroll = State.Scroll;
                                chip.Measure = (State.Measure.Item1, State.Measure.Item2);
                                chip.BranchType = CurrentBranch;
                                chip.Delay = State.Delay;
                                chip.ChangedBPM = CurrentBPMChanged;

                                State.NowTime += (long)(240000000 * (chip.Measure.Item1 / chip.Measure.Item2) / chip.BPM / CurrentNoteCount);

                                long hbMove = (long)(1000000 * (chip.Measure.Item1 / chip.Measure.Item2) / CurrentNoteCount);
                                State.NowHBTime += hbMove;

                                if (CurrentBPMChanged)
                                {
                                    State.TimeGAP -= (chip.Time * chip.BPM) - (chip.Time * chip.PrevBPM);
                                }
                                chip.TimeGAP = State.TimeGAP;

                                CurrentBPMChanged = false;
                            }
                            break;
                            case ChipType.BPMChange:
                            {
                                State.PrevBPM = PrevNote == null ? InitBPM : PrevNote.BPM;
                                State.BPM = chip.BPM;
                                chip.Scroll = State.Scroll;
                                chip.Measure = (State.Measure.Item1, State.Measure.Item2);
                                chip.TimeGAP = State.TimeGAP;
                                chip.Delay = State.Delay;
                                chip.BranchType = CurrentBranch;

                                CurrentBPMChanged = true;
                            }
                            break;
                            case ChipType.MeasureChange:
                            {
                                chip.BPM = State.BPM;
                                chip.PrevBPM = State.PrevBPM;
                                chip.Scroll = State.Scroll;
                                State.Measure = (chip.Measure.Item1, chip.Measure.Item2);
                                chip.TimeGAP = State.TimeGAP;
                                chip.Delay = State.Delay;
                                chip.BranchType = CurrentBranch;
                            }
                            break;
                            case ChipType.ScrollChange:
                            {
                                chip.BPM = State.BPM;
                                chip.PrevBPM = State.PrevBPM;
                                State.Scroll = chip.Scroll;
                                chip.Measure = (State.Measure.Item1, State.Measure.Item2);
                                chip.TimeGAP = State.TimeGAP;
                                chip.BranchType = CurrentBranch;
                            }
                            break;
                            case ChipType.Delay:
                            {
                                State.NowTime += (long)(chip.Delay * 1000000);
                                chip.Time = PrevNoteTime;
                                chip.BPM = State.BPM;
                                chip.PrevBPM = State.PrevBPM;
                                chip.Scroll = State.Scroll;
                                chip.Measure = (State.Measure.Item1, State.Measure.Item2);
                                chip.TimeGAP = State.TimeGAP;
                                chip.BranchType = CurrentBranch;
                                State.Delay += chip.Delay;
                                if (chip.Delay < 0)
                                {
                                    State.NowHBTime += (long)(chip.Delay * 4600.0f * chip.BPM);
                                }
                            }
                            break;
                            case ChipType.GoGoStart:
                            {
                                
                            }
                            break;
                            case ChipType.GoGoEnd:
                            {
                                
                            }
                            break;
                            case ChipType.BranchStart:
                            {
                                BranchState = State.Copy();
                                chip.BPM = State.BPM;
                                chip.PrevBPM = State.PrevBPM;
                                chip.Scroll = State.Scroll;
                                chip.Measure = (State.Measure.Item1, State.Measure.Item2);
                                chip.TimeGAP = State.TimeGAP;
                                chip.Delay = State.Delay;
                                chip.BranchType = CurrentBranch;
                                chip.Time -= (long)(240000000 * (chip.Measure.Item1 / chip.Measure.Item2) / chip.BPM);
                            }
                            break;
                            case ChipType.BranchEnd:
                            {
                                chip.BPM = State.BPM;
                                chip.PrevBPM = State.PrevBPM;
                                chip.Scroll = State.Scroll;
                                chip.Measure = (State.Measure.Item1, State.Measure.Item2);
                                chip.TimeGAP = State.TimeGAP;
                                chip.Delay = State.Delay;
                                chip.BranchType = CurrentBranch;
                            }
                            break;
                            case ChipType.Branch_N:
                            {
                                State = BranchState.Copy();
                                CurrentBranch = BranchType.Normal;
                            }
                            break;
                            case ChipType.Branch_E:
                            {
                                State = BranchState.Copy();
                                CurrentBranch = BranchType.Expert;
                            }
                            break;
                            case ChipType.Branch_M:
                            {
                                State = BranchState.Copy();
                                CurrentBranch = BranchType.Master;
                            }
                            break;
                            case ChipType.Line:
                            case ChipType.Line_Branched:
                            {
                                chip.Time = PrevNote.Time;
                                chip.HBTime = PrevNote.HBTime;
                                chip.BPM = State.BPM;
                                chip.PrevBPM = State.PrevBPM;
                                chip.Scroll = State.Scroll;
                                chip.Measure = (State.Measure.Item1, State.Measure.Item2);
                                chip.TimeGAP = State.TimeGAP;
                                chip.Delay = State.Delay;
                                chip.BranchType = CurrentBranch;
                            }
                            break;
                            default:
                            {
                            }
                            break;
                        }

                        Chips.Add(chip);
                    }
                    CurrentChips.Clear();
                    CurrentNoteCount = 0;
                }
                else 
                {
                    ChipType chipType = CharToChipType(ch);
                    if (chipType == ChipType.Invalid) continue;

                    TJAChip chip = new();
                    chip.ChipType = chipType;
                    chip.ScrollType = State.ScrollType;
                    if (chipType == ChipType.Roll_Balloon_Start)
                    {
                        chip.BalloonCount = Balloons.Count <= BalloonCount ? 5 : Balloons[BalloonCount];
                        BalloonCount++;
                    }
                    CurrentChips.Add(chip);

                    if (CurrentNoteCount == 0 && BarLineOn)
                    {
                        TJAChip lineChip = new();
                        lineChip.ChipType = ChipType.Line;
                        lineChip.ScrollType = lineChip.ScrollType;
                        CurrentChips.Add(lineChip);
                    }

                    CurrentNoteCount++;
                }
            }
        }
    }

    private string RemoveComment(string text)
    {
        string result = "";
        for(int i = 0; i < text.Length; i++)
        {
            if (i != text.Length - 1 && text[i] == '/' && text[i + 1] == '/')
            {
                break;
            }
            else 
            {
                result += text[i];
            }
        }
        return result;
    }

    public TJALoader(string fileName)
    {
        using StreamReader stream = new(fileName, Encoding.GetEncoding("SHIFT_JIS"));
        while(!stream.EndOfStream)
        {
            string? text = stream.ReadLine();
            text = RemoveComment(text);
            if (text == null || text == "") continue;



            string[] text_split = text.Split(':');

            if (text_split.Length == 2)
            {
                string key = text_split[0];
                string value = text_split[1];

                SetHeader(key, value);
            }
            SetCommand(text);
        }
    }
}