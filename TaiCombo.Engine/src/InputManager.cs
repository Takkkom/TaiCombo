using Silk.NET.Windowing;
using Silk.NET.Input;

namespace TaiCombo.Engine;

public class InputManager : IDisposable
{
    public IInputContext Context { get; private set; }

    public bool[] KeyStates = new bool[349];
    public int[] KeyValues = new int[349];

    public InputManager(IWindow window)
    {
        Context = window.CreateInput();

        for(int i = 0; i < Context.Keyboards.Count; i++)
        {
            IKeyboard keyboard = Context.Keyboards[i];
            keyboard.KeyDown += OnKeyDown;
            keyboard.KeyUp += OnKeyUp;
            keyboard.KeyChar += OnKeyChar;
        }
    }

    private void OnKeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Unknown) return;
        KeyStates[(int)key] = true;
        KeyValues[(int)key] = 0;
    }

    private void OnKeyUp(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Unknown) return;
        KeyStates[(int)key] = false;
        KeyValues[(int)key] = 0;
    }

    private void OnKeyChar(IKeyboard keyboard, char ch)
    {
        
    }

    public bool GetKeyPressed(Key key)
    {
        return KeyValues[(int)key] == 1;
    }

    public bool GetKeyPressing(Key key)
    {
        return KeyValues[(int)key] >= 1;
    }

    public bool GetKeyReleased(Key key)
    {
        return KeyValues[(int)key] == -1;
    }

    public bool GetKeyReleasing(Key key)
    {
        return KeyValues[(int)key] <= -1;
    }

    public void Update()
    {
        for(int i = 0; i < KeyStates.Length; i++)
        {
            bool flag = KeyStates[i];
            if (flag)
            {
                if (KeyValues[i] < 2)
                {
                    KeyValues[i]++;
                }
            }
            else
            {
                if (KeyValues[i] > -2)
                {
                    KeyValues[i]--;
                }
            }
        }
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}