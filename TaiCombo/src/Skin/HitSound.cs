using TaiCombo.Common;
using TaiCombo.Engine;

namespace TaiCombo.Skin;

class HitSound : IDisposable
{
    public Sound Don { get; private set; }
    public Sound Don_Left { get; private set; }
    public Sound Don_Right { get; private set; }

    public Sound Ka { get; private set; }
    public Sound Ka_Left { get; private set; }
    public Sound Ka_Right { get; private set; }


    public Sound Clap { get; private set; }
    public Sound Clap_Left { get; private set; }
    public Sound Clap_Right { get; private set; }

    public HitSound(HitSoundJson hitSoundJson)
    {
        string hitSoundPath = $"{Game.Skin.SoundsPath}HitSounds{Path.DirectorySeparatorChar}{hitSoundJson.DirName}{Path.DirectorySeparatorChar}";
        Don = new($"{hitSoundPath}Don.ogg");
        Don_Left = new($"{hitSoundPath}Don_Left.ogg");
        Don_Right = new($"{hitSoundPath}Don_Right.ogg");
        Ka = new($"{hitSoundPath}Ka.ogg");
        Ka_Left = new($"{hitSoundPath}Ka_Left.ogg");
        Ka_Right = new($"{hitSoundPath}Ka_Right.ogg");
        Clap = new($"{hitSoundPath}Clap.ogg");
        Clap_Left = new($"{hitSoundPath}Clap_Left.ogg");
        Clap_Right = new($"{hitSoundPath}Clap_Right.ogg");
    }

    public void Dispose()
    {
        Don.Dispose();
        Don_Left.Dispose();
        Don_Right.Dispose();
        Ka.Dispose();
        Ka_Left.Dispose();
        Ka_Right.Dispose();
    }
}