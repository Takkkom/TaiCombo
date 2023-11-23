using Silk.NET.Maths;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Engine.Enums;
using TaiCombo.Scenes;
using TaiCombo.Skin;

namespace TaiCombo.Common;

class LangString
{
    public Dictionary<string, string> Texts { get; set; } = new();

    public LangString(Dictionary<string, string> texts)
    {
        Texts = texts;
    }

    public string GetText(string? lang = null)
    {
        if (lang == null) lang = Game.Config.Value.Langauge;
        
        if (Texts.ContainsKey(lang))
        {
            return Texts[lang];
        }
        else 
        {
            return Texts["default"];
        }
    }
}