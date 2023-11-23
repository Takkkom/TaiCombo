using TaiCombo.Common;
using TaiCombo.Engine;
using TaiCombo.Engine.Struct;
using TaiCombo.Structs;

namespace TaiCombo.Scenes;

class ResultScene : Scene
{
    private ResultValues[] Values;

    public ResultScene(Dictionary<string, object> args)
    {
        Values = (ResultValues[])args["values"];
    }

    public override void Activate()
    {
        
        base.Activate();
    }

    public override void DeActivate()
    {

        base.DeActivate();
    }

    public override void Update()
    {

        base.Update();
    }

    public override void Draw()
    {

        base.Draw();
    }
}