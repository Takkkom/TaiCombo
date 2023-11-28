namespace TaiCombo.Enums;

static class EnumToString
{
    public static string CharaSceneTypeToString(CharaSceneType charaSceneType)
    {
        switch(charaSceneType)
        {
            case CharaSceneType.Title:
            return "Title";
            case CharaSceneType.SongSelect:
            return "SongSelect";
            case CharaSceneType.Play:
            return "Play";
            case CharaSceneType.Play_Balloon:
            return "Play_Balloon";
            case CharaSceneType.Result:
            return "Result";
            default:
            return "None";
        }
    }

    public static string CharaAnimeTypeToString(CharaAnimeType charaAnimeType)
    {
        switch(charaAnimeType)
        {
            case CharaAnimeType.Normal:
            return "Normal";
            case CharaAnimeType.Clear:
            return "Clear";
            case CharaAnimeType.Miss:
            return "Miss";
            case CharaAnimeType.Miss_Down:
            return "Miss_Down";
            case CharaAnimeType.GoGo:
            return "GoGo";
            case CharaAnimeType.ClearIn:
            return "ClearIn";
            case CharaAnimeType.ClearOut:
            return "ClearOut";
            case CharaAnimeType.SoulIn:
            return "SoulIn";
            case CharaAnimeType.Jump:
            return "Jump";
            case CharaAnimeType.Jump_Max:
            return "Jump_Max";
            case CharaAnimeType.Return:
            return "Return";
            case CharaAnimeType.GoGoStart:
            return "GoGoStart";
            case CharaAnimeType.Balloon_Breaking:
            return "Balloon_Breaking";
            case CharaAnimeType.Balloon_Broke:
            return "Balloon_Broke";
            case CharaAnimeType.Balloon_Miss:
            return "Balloon_Miss";
            default:
            return "None";
        }
    }
}