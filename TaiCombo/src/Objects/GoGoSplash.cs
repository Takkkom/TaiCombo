using System.Drawing;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Enums;
using TaiCombo.Engine.Struct;
using TaiCombo.Enums;
using TaiCombo.Helper;

namespace TaiCombo.Objects;

class GoGoSplash
{
    private float Counter = 1;

    public GoGoSplash()
    {

    }

    public void Start()
    {
        Counter = 0;
    }
    
    public void Update()
    {
        if (Counter < 1)
        {
            Counter += 1f * GameEngine.Time_.DeltaTime;
        }
        else 
        {
            Counter = 1;
        }
    }
    
    public void Draw()
    {
        if (Counter >= 1) return;

        for(int i = 0; i < Game.Skin.Value.Play_GoGoSplashs.Length; i++)
        {
            Sprite sprite = Game.Skin.Assets.Play_GoGoSplashs[(int)(Counter * Game.Skin.Assets.Play_GoGoSplashs.Length)];
            int x = Game.Skin.Value.Play_GoGoSplashs[i].X;
            int y = Game.Skin.Value.Play_GoGoSplashs[i].Y;
            int value;

            if (i < 3)
            {
                value = 2 - i;
            }
            else 
            {
                value = 3 - i;
            }

            float rotation = -value * MathF.PI / 40.0f;
            sprite.Draw(x, y, rotation:rotation, drawOriginType:DrawOriginType.Center, blendType:BlendType.Add);
        }
    }
}