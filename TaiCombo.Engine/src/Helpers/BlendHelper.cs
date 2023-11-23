using Silk.NET.OpenGLES;
using TaiCombo.Engine.Enums;

namespace TaiCombo.Engine.Helpers;

public static class BlendHelper
{
    public static void SetBlend(BlendType blendType)
    {
        switch(blendType)
        {
            case BlendType.Normal:
            GameEngine.Gl.BlendEquation(BlendEquationModeEXT.FuncAdd);
            GameEngine.Gl.BlendFunc(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha);
            break;
            case BlendType.Add:
            GameEngine.Gl.BlendEquation(BlendEquationModeEXT.FuncAdd);
            GameEngine.Gl.BlendFunc(GLEnum.SrcAlpha, GLEnum.One);
            break;
            case BlendType.Screen:
            GameEngine.Gl.BlendEquation(BlendEquationModeEXT.FuncAdd);
            GameEngine.Gl.BlendFunc(GLEnum.One, GLEnum.OneMinusSrcColor);
            break;
            case BlendType.Multi:
            GameEngine.Gl.BlendEquation(BlendEquationModeEXT.FuncAdd);
            GameEngine.Gl.BlendFunc(GLEnum.DstColor, GLEnum.OneMinusSrcAlpha);
            break;
            case BlendType.Sub:
            GameEngine.Gl.BlendEquation(BlendEquationModeEXT.FuncReverseSubtract);
            GameEngine.Gl.BlendFunc(GLEnum.SrcAlpha, GLEnum.One);
            break;
        }
    }
}