using TaiCombo.Enums;

namespace TaiCombo.Chara;

interface IPlayerChara : IDisposable
{
    public string DirPath { get; protected set; }
    public string Name { get; protected set; }

    public void LoadAssets();

    public void ChangeAnime(CharaAnimeType charaAnimeType, int player, bool loop, Action? action = null);

    public void Update(float bpm, CharaSceneType charaSceneType, int player);
    
    public void Draw(float x, float y, float scale, bool flipX, bool flipY, CharaSceneType charaSceneType, float opacity, int player);
}