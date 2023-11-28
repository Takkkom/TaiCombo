using System.Drawing;
using SkiaSharp;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Struct;
using TaiCombo.Helper;
using TaiCombo.Plugin.Enums;

namespace TaiCombo.Objects;

class PlayTitle : IDisposable
{
    private Sprite Plate;
    private Sprite CountPlate;

    private Sprite Title;

    private Sprite SongCount_Desc_Count;
    private Sprite SongCount_Desc_Max;
    private Sprite SongCount_Value_Count;
    private Sprite SongCount_Value_Max;
    private Sprite GenreName;

    private float PlateCounter;
    private bool ShowCount;

    public PlayTitle(string title, string genreText, string color, int currentSong, int maxSong)
    {
        if (Game.Skin.Assets.Play_Title_GenrePlate.ContainsKey(color))
        {
            Plate = Game.Skin.Assets.Play_Title_GenrePlate[color];
        }
        else 
        {
            Plate = Game.Skin.Assets.Play_Title_GenrePlate["Default"];
        }
        CountPlate = Game.Skin.Assets.Play_Title_GenrePlate["Count"];

        Title = Game.Skin.Assets.Font_Main.GenSpriteText(title, new (1, 1, 1, 1), Game.Skin.Value.Play_Title.Size, new EdgeInfo[] { new EdgeInfo() { Color = new Color4(0, 0, 0, 1), Ratio = 0.25f } });

        SongCount_Desc_Count = Game.Skin.Assets.Font_Sub.GenSpriteText("曲目", new (1, 1, 1, 1), Game.Skin.Value.Play_SongCount_Desc_Count.Size, new EdgeInfo[] { new EdgeInfo() { Color = new Color4(0, 0, 0, 1), Ratio = 0.25f } });
        SongCount_Desc_Max = Game.Skin.Assets.Font_Sub.GenSpriteText("曲", new (1, 1, 1, 1), Game.Skin.Value.Play_SongCount_Desc_Max.Size, new EdgeInfo[] { new EdgeInfo() { Color = new Color4(0, 0, 0, 1), Ratio = 0.25f } });

        SongCount_Value_Count = Game.Skin.Assets.Font_Sub.GenSpriteText(currentSong.ToString(), new (1, 1, 1, 1), Game.Skin.Value.Play_SongCount_Value_Count.Size, new EdgeInfo[] { new EdgeInfo() { Color = new Color4(0, 0, 0, 1), Ratio = 0.25f } });
        SongCount_Value_Max = Game.Skin.Assets.Font_Sub.GenSpriteText(maxSong.ToString(), new (1, 1, 1, 1), Game.Skin.Value.Play_SongCount_Value_Max.Size, new EdgeInfo[] { new EdgeInfo() { Color = new Color4(0, 0, 0, 1), Ratio = 0.25f } });

        GenreName = Game.Skin.Assets.Font_Sub.GenSpriteText(genreText, new (1, 1, 1, 1), Game.Skin.Value.Play_GenrePlate_Name.Size, new EdgeInfo[] { new EdgeInfo() { Color = new Color4(0, 0, 0, 1), Ratio = 0.25f } });
    }

    public void Dispose()
    {
        Title.Dispose();
        
        SongCount_Desc_Count.Dispose();
        SongCount_Desc_Max.Dispose();

        SongCount_Value_Count.Dispose();
        SongCount_Value_Max.Dispose();

        GenreName.Dispose();
    }

    public void Update()
    {
        PlateCounter += 0.5f * GameEngine.Time_.DeltaTime;
        if (PlateCounter >= 1)
        {
            ShowCount = !ShowCount;
            PlateCounter = 0;
        }
    }

    public void Draw()
    {
        int plate_x = Game.Skin.Value.Play_GenrePlate.X;
        int plate_y = Game.Skin.Value.Play_GenrePlate.Y;
        float baseOpacity = Math.Min(PlateCounter * 4, 1);
        float count_opacity = ShowCount ? baseOpacity : 1 - baseOpacity;
        float genre_opacity = 1 - count_opacity;
        Color4 count_color = new Color4(1, 1, 1, count_opacity);

        CountPlate.Draw(plate_x, plate_y, color:count_color);
        SongCount_Desc_Count.Draw(plate_x + Game.Skin.Value.Play_SongCount_Desc_Count.Pos.X, plate_y + Game.Skin.Value.Play_SongCount_Desc_Count.Pos.Y, color:count_color, drawOriginType:DrawOriginType.Right_Down);
        SongCount_Desc_Max.Draw(plate_x + Game.Skin.Value.Play_SongCount_Desc_Max.Pos.X, plate_y + Game.Skin.Value.Play_SongCount_Desc_Max.Pos.Y, color:count_color, drawOriginType:DrawOriginType.Right_Down);

        SongCount_Value_Count.Draw(plate_x + Game.Skin.Value.Play_SongCount_Value_Count.Pos.X, plate_y + Game.Skin.Value.Play_SongCount_Value_Count.Pos.Y, color:count_color, drawOriginType:DrawOriginType.Right_Down);
        SongCount_Value_Max.Draw(plate_x + Game.Skin.Value.Play_SongCount_Value_Max.Pos.X, plate_y + Game.Skin.Value.Play_SongCount_Value_Max.Pos.Y, color:count_color, drawOriginType:DrawOriginType.Right_Down);

        Plate.Draw(plate_x, plate_y, color:new Color4(1, 1, 1, genre_opacity));
        GenreName.Draw(plate_x + Game.Skin.Value.Play_GenrePlate_Name.Pos.X, plate_y + Game.Skin.Value.Play_GenrePlate_Name.Pos.Y, drawOriginType:DrawOriginType.Center, color:new Color4(1, 1, 1, genre_opacity));

        float title_scale = Title.TextureSize.Width / (float)Math.Min(Game.Skin.Value.Play_Title.MaxSize, Title.TextureSize.Width);
        Title.Draw(Game.Skin.Value.Play_Title.Pos.X, Game.Skin.Value.Play_Title.Pos.Y, scaleX:title_scale, drawOriginType:DrawOriginType.Right_Down);
    }
}