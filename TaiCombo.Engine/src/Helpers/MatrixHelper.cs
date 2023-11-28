using System.Drawing;
using Silk.NET.Maths;
using TaiCombo.Engine.Enums;

namespace TaiCombo.Engine.Helpers;

public static class MatrixHelper
{
    public static Matrix4X4<float> ScreenOffset = Matrix4X4.CreateTranslation(-1.0f, 1.0f, 0);

    public static Matrix4X4<float> Get2DMatrix(float x, float y, float scaleX, float scaleY, RectangleF rectangle, bool flipX, bool flipY, float rotation, DrawOriginType drawOriginType)
    {
        float gameAspect = (float)GameEngine.Resolution.X / GameEngine.Resolution.Y;

        //スケーリング-----
        Matrix4X4<float> mvp = Matrix4X4.CreateScale(rectangle.Width / GameEngine.Resolution.X, rectangle.Height / GameEngine.Resolution.Y, 1) * 
            Matrix4X4.CreateScale(flipX ? -scaleX : scaleX, flipY ? -scaleY : scaleY, 1.0f);
        //-----

        //回転-----
        if (rotation != 0)
        {
            mvp *= Matrix4X4.CreateScale(1.0f * gameAspect, 1.0f, 1.0f) * //ここでアスペクト比でスケーリングしないとおかしなことになる
                Matrix4X4.CreateRotationZ(rotation) * 
                Matrix4X4.CreateScale(1.0f / gameAspect, 1.0f, 1.0f);//回転した後戻してあげる
        }
        //-----

        //移動----
        float offsetX = rectangle.Width * scaleX / GameEngine.Resolution.X;
        float offsetY = rectangle.Height * scaleY / GameEngine.Resolution.Y;
        switch(drawOriginType)
        {
            case DrawOriginType.Left_Up:
            mvp *= Matrix4X4.CreateTranslation(offsetX, -offsetY, 0.0f);
            break;
            case DrawOriginType.Left:
            mvp *= Matrix4X4.CreateTranslation(offsetX, 0, 0.0f);
            break;
            case DrawOriginType.Left_Down:
            mvp *= Matrix4X4.CreateTranslation(offsetX, offsetY, 0.0f);
            break;
            case DrawOriginType.Up:
            mvp *= Matrix4X4.CreateTranslation(0.0f, -offsetY, 0.0f);
            break;
            case DrawOriginType.Center:
            mvp *= Matrix4X4.CreateTranslation(0.0f, 0, 0.0f);
            break;
            case DrawOriginType.Down:
            mvp *= Matrix4X4.CreateTranslation(0.0f, offsetY, 0.0f);
            break;
            case DrawOriginType.Right_Up:
            mvp *= Matrix4X4.CreateTranslation(-offsetX, -offsetY, 0.0f);
            break;
            case DrawOriginType.Right:
            mvp *= Matrix4X4.CreateTranslation(-offsetX, 0, 0.0f);
            break;
            case DrawOriginType.Right_Down:
            mvp *= Matrix4X4.CreateTranslation(-offsetX, offsetY, 0.0f);
            break;
        }
        mvp *= ScreenOffset;
        mvp *= Matrix4X4.CreateTranslation(x / GameEngine.Resolution.X * 2, -y / GameEngine.Resolution.Y * 2, 0.0f);

        return mvp;
    }
}