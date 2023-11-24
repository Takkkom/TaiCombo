using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Enums;

namespace TaiCombo.Objects;

class FlyNotes
{
    private class FlyNoteInfo
    {
        public FlyNoteType FlyNoteType;

        public float X;
        public float Y;

        public float Value;

        public float BigEffectCounter;
    }

    private List<FlyNoteInfo> FlyNoteInfos = new();

    private BigEffect BigEffect;

    private FlyEndEffect FlyEndEffect;

    private int Player;

    private Script<float> MoveScriptX;
    private Script<float> MoveScriptY;

    private const int MAXNOTECOUNT = 20;
    private const int FRAMECOUNT = 30;
    private float[] X = new float[FRAMECOUNT];
    private float[] Y = new float[FRAMECOUNT];

    public void Add(FlyNoteType flyNoteType)
    {
        FlyNoteInfo flyNoteInfo = new()
        {
            FlyNoteType = flyNoteType,
            Value = 0,
            X = X[0],
            Y = Y[0]
        };
        FlyNoteInfos.Add(flyNoteInfo);

        if (MAXNOTECOUNT <= FlyNoteInfos.Count)
        {
            FlyNoteInfos.RemoveAt(0);
        }
    }

    public FlyNotes(int player)
    {
        Player = player;

        var option = ScriptOptions.Default.WithImports("System");
        MoveScriptX = CSharpScript.Create<float>(Game.Skin.Value.Play_FlyNotes.MoveScriptX[player], option, typeof(Skin.FlyNoteJson));
        MoveScriptY = CSharpScript.Create<float>(Game.Skin.Value.Play_FlyNotes.MoveScriptY[player], option, typeof(Skin.FlyNoteJson));

        for(int i = 0; i < FRAMECOUNT; i++)
        {
            async Task set()
            {
                Game.Skin.Value.Play_FlyNotes.Value = i / (float)FRAMECOUNT;
                X[i] = (await MoveScriptX.RunAsync(globals:Game.Skin.Value.Play_FlyNotes)).ReturnValue;
                Y[i] = (await MoveScriptY.RunAsync(globals:Game.Skin.Value.Play_FlyNotes)).ReturnValue;
            }
            set().Wait();
        }

        BigEffect = new(player);
        FlyEndEffect = new(player);
    }

    public void Update()
    {
        BigEffect.Update();
        FlyEndEffect.Update();

        for(int i = 0; i < FlyNoteInfos.Count; i++)
        {
            FlyNoteInfo flyNoteInfo = FlyNoteInfos[i];
            flyNoteInfo.Value += 2f * GameEngine.Time_.DeltaTime;

            int frame = Math.Min((int)(flyNoteInfo.Value * (FRAMECOUNT - 1)), (FRAMECOUNT - 1));

            flyNoteInfo.X = X[frame];
            flyNoteInfo.Y = Y[frame];

            if (flyNoteInfo.FlyNoteType == FlyNoteType.Don_Big || flyNoteInfo.FlyNoteType == FlyNoteType.Ka_Big)
            {
                flyNoteInfo.BigEffectCounter += 20f * GameEngine.Time_.DeltaTime;
                if (flyNoteInfo.BigEffectCounter >= 1)
                {
                    BigEffect.Add(flyNoteInfo.X, flyNoteInfo.Y);
                    flyNoteInfo.BigEffectCounter = 0;
                }
            }

            if (flyNoteInfo.Value > 1)
            {
                FlyEndEffect.Add(flyNoteInfo.FlyNoteType);
                FlyNoteInfos.Remove(flyNoteInfo);
                continue;
            }
        }
    }

    public void Draw()
    {
        BigEffect.Draw();
        FlyEndEffect.Draw();
        
        for(int i = 0; i < FlyNoteInfos.Count; i++)
        {
            FlyNoteInfo flyNoteInfo = FlyNoteInfos[i];

            switch(flyNoteInfo.FlyNoteType)
            {
                case FlyNoteType.Don:
                Game.Skin.Assets.Play_FlyNotes_Don.Draw(flyNoteInfo.X, flyNoteInfo.Y);
                break;
                case FlyNoteType.Ka:
                Game.Skin.Assets.Play_FlyNotes_Ka.Draw(flyNoteInfo.X, flyNoteInfo.Y);
                break;
                case FlyNoteType.Don_Big:
                Game.Skin.Assets.Play_FlyNotes_Don_Big.Draw(flyNoteInfo.X, flyNoteInfo.Y);
                break;
                case FlyNoteType.Ka_Big:
                Game.Skin.Assets.Play_FlyNotes_Ka_Big.Draw(flyNoteInfo.X, flyNoteInfo.Y);
                break;
            }
        }
    }
}