using Silk.NET.OpenGLES;

namespace TaiCombo.Engine.Helpers;

public static class ShaderHelper
{
    public static uint CreateShader(string code, ShaderType shaderType)
    {
        uint vertexShader = GameEngine.Gl.CreateShader(shaderType);

        GameEngine.Gl.ShaderSource(vertexShader, code);
        GameEngine.Gl.CompileShader(vertexShader);
        
        GameEngine.Gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int status);

        if (status != (int)GLEnum.True)
            throw new Exception($"{shaderType} failed to compile:{GameEngine.Gl.GetShaderInfoLog(vertexShader)}");

        return vertexShader;
    }

    public static uint CreateShaderProgram(uint vertexShader, uint fragmentShader)
    {
        uint program = GameEngine.Gl.CreateProgram();
        
        GameEngine.Gl.AttachShader(program, vertexShader);
        GameEngine.Gl.AttachShader(program, fragmentShader);

        GameEngine.Gl.LinkProgram(program);

        GameEngine.Gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out int linkStatus);
        if (linkStatus != (int)GLEnum.True)
            throw new Exception($"Program failed to link:{GameEngine.Gl.GetProgramInfoLog(program)}");

        GameEngine.Gl.DetachShader(program, vertexShader);
        GameEngine.Gl.DetachShader(program, fragmentShader);

        return program;
    }

    public static uint CreateShaderProgramFromSource(string vertexCode, string fragmentCode)
    {
        uint vertexShader = CreateShader(vertexCode, ShaderType.VertexShader);
        uint fragmentShader = CreateShader(fragmentCode, ShaderType.FragmentShader);
        
        uint program = CreateShaderProgram(vertexShader, fragmentShader);
        
        GameEngine.Gl.DeleteShader(vertexShader);
        GameEngine.Gl.DeleteShader(fragmentShader);

        return program;
    }
}