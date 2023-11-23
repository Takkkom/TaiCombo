

namespace TaiCombo.Common;

static class Program
{
    static void Main(string[] args)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        
        using Game game = new();
        game.Run();
    }
}