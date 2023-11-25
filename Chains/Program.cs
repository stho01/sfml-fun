using Stho.SFML.Extensions;

namespace Chains;

class Program
{
    static void Main(string[] args)
    {
            var window = WindowFactory.CreateDefault();
            var game = new Game(window);
            game.Initialize();
            game.Start();
        }
}