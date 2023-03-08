using Stride.Engine;

namespace StrideNPC
{
    class StrideNPCApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
