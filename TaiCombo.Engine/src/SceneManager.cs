

namespace TaiCombo.Engine;

public class SceneManager : IDisposable
{
    private Scene CurrentScene;

    public SceneManager()
    {

    }

    public void Update()
    {
        CurrentScene?.Update();
    }

    public void Draw()
    {
        CurrentScene?.Draw();
    }

    public void ChangeScene(Scene scene)
    {
        CurrentScene?.DeActivate();
        CurrentScene = scene;
        CurrentScene.Activate();
    }

    public void Dispose()
    {
        CurrentScene?.DeActivate();
    }
}