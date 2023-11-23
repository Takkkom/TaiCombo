using Silk.NET.Input;
using Silk.NET.OpenGLES.Extensions.ImGui;
using ImGuiNET;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Scenes;

namespace TaiCombo.Common;

class Overlay : Scene
{
    private bool ShowImGUI = false;

    private ImGuiController ImGUI_;

    private byte[] NextSkinBuf;

    private byte[] NameBuf;
    private byte[] VerBuf;
    
    private void ReadBuffer()
    {
        NextSkinBuf = System.Text.Encoding.ASCII.GetBytes(Game.Config.Value.Skin);
        Array.Resize(ref NextSkinBuf, 256);

        NameBuf = System.Text.Encoding.ASCII.GetBytes(Game.Skin.Value.Name);
        Array.Resize(ref NameBuf, 256);

        VerBuf = System.Text.Encoding.ASCII.GetBytes(Game.Skin.Value.Version);
        Array.Resize(ref VerBuf, 256);
    }
    public Overlay()
    {
    }

    public override void Activate()
    {
        ImGUI_ = new(GameEngine.Gl, GameEngine.Window_, GameEngine.Input.Context);

        ReadBuffer();
        
        base.Activate();
    }

    public override void DeActivate()
    {
        ImGUI_.Dispose();

        base.DeActivate();
    }

    public override void Update()
    {
        if (GameEngine.Input.GetKeyPressed(Key.F12))
        {
            if (GameEngine.Input.GetKeyPressing(Key.ControlLeft))
            {
                ShowImGUI = !ShowImGUI;
            }
            else 
            {

            }
        }
        base.Update();
    }

    public override void Draw()
    {
        if (!ShowImGUI) return;

        ImGUI_.Update((float)GameEngine.Time_.DeltaTime);

        ImGui.Begin("System");

        DrawMainConfig();

        ImGui.End();

        ImGUI_.Render();

        base.Draw();
    }

    public void DrawMainConfig()
    {
        if (ImGui.TreeNode("Info"))
        {
            ImGui.Text($"FPS: {GameEngine.Time_.FPS}");
            ImGui.Text($"DeltaTime: {GameEngine.Time_.DeltaTime}");
            ImGui.Text($"NowSecondTime: {GameEngine.Time_.NowSecondTime}");
            ImGui.Text($"VSync: {GameEngine.Window_.VSync}");

            ImGui.TreePop();
        }
        if (ImGui.TreeNode("Config"))
        {
            float framerate = (float)GameEngine.Framerate;
            int frameMode = (int)GameEngine.FrameMode_;

            if (ImGui.RadioButton("Unlimited", ref frameMode, (int)FrameMode.Unlimited))
            {
                GameEngine.FrameMode_ = FrameMode.Unlimited;
            }
            if (ImGui.RadioButton("Limited", ref frameMode, (int)FrameMode.Limited))
            {
                GameEngine.FrameMode_ = FrameMode.Limited;
            }
            if (ImGui.RadioButton("VSync", ref frameMode, (int)FrameMode.VSync))
            {
                GameEngine.FrameMode_ = FrameMode.VSync;
            }
            if (GameEngine.FrameMode_ == FrameMode.Limited)
            {
                if (ImGui.SliderFloat("Framerate", ref framerate, 10, 360))
                {
                    GameEngine.Framerate = framerate;
                }
            }

            ImGui.TreePop();
        }
        if (ImGui.TreeNode("Skin"))
        {
            if (ImGui.InputText("NextSkin", NextSkinBuf, (uint)NextSkinBuf.Length))
            {
            }
            if (ImGui.Button("ChangeSkin"))
            {
                string skin = System.Text.Encoding.ASCII.GetString(NextSkinBuf).Replace("\0", "");
                Game.Skin.ChangeSkin(skin);

                ReadBuffer();
            }
            if (ImGui.Button("ReadConfig"))
            {
                Game.Skin.Read();
                ReadBuffer();
                GameEngine.Resolution = new(Game.Skin.Value.Resolution.Width, Game.Skin.Value.Resolution.Height);
            }
            if (ImGui.Button("WriteConfig"))
            {
                Game.Skin.Write();
            }
            if (ImGui.InputText("Name", NameBuf, (uint)NameBuf.Length))
            {
                Game.Skin.Value.Name = System.Text.Encoding.ASCII.GetString(NameBuf).Replace("\0", "");
            }
            if (ImGui.InputText("Version", VerBuf, (uint)VerBuf.Length))
            {
                Game.Skin.Value.Version = System.Text.Encoding.ASCII.GetString(VerBuf).Replace("\0", "");
            }
            
            int width = Game.Skin.Value.Resolution.Width;
            if (ImGui.InputInt("Width", ref width))
            {
                Game.Skin.Value.Resolution.Width = width;
                GameEngine.Resolution = new(width, GameEngine.Resolution.Y);
            }

            int height = Game.Skin.Value.Resolution.Height;
            if (ImGui.InputInt("Height", ref height))
            {
                Game.Skin.Value.Resolution.Height = height;
                GameEngine.Resolution = new(GameEngine.Resolution.X, height);
            }

            ImGui.TreePop();
        }
    }
}