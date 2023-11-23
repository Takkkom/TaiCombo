using ManagedBass;

namespace TaiCombo.Engine;

/// <summary>
/// 2Dの画像を描画するクラス
/// </summary>
public class Sound : IDisposable
{
    private int Handle;

    public float Time 
    {
        get 
        {
            return (float)Bass.ChannelBytes2Seconds(Handle, Bass.ChannelGetPosition(Handle));
        }
        set 
        {
            Bass.ChannelSetPosition(Handle, Bass.ChannelSeconds2Bytes(Handle, value));
        }
    }

    private float _Speed;
    public float Speed
    {
        get 
        {
            return _Speed;
        }
        set 
        {
            _Speed = value;
            Bass.ChannelSetAttribute(Handle, ChannelAttribute.Frequency, value * Frequency);
        }
    }

    public int Frequency { get; private set; }

    public static void Init()
    {
        Bass.Init();
    }

    public static void Terminate()
    {
        Bass.Free();
    }
    
    public Sound(string fileName)
    {
        Handle = Bass.CreateStream(fileName);

        Bass.ChannelGetAttribute( Handle, ChannelAttribute.Frequency, out float freq );
        Frequency = (int)freq;
    }

    public void Play(bool restart = true)
    {
        Bass.ChannelPlay(Handle, restart);
    }

    public void Pause()
    {
        Bass.ChannelPause(Handle);
    }

    public void Stop()
    {
        Bass.ChannelStop(Handle);
    }

    public void Dispose()
    {
        Bass.StreamFree(Handle);
    }
}