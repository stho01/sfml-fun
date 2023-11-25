using System.Linq;
using SFML.Graphics;
using SFML.Window;
using Stho.SFML.Extensions;
using Timer = Stho.SFML.Extensions.Timer;

namespace PhysixSimulation;

class Program
{
    public const int ScreenWidth = 1800;
    public const int ScreenHeight = 900;

    static void Main(string[] args)
    {
            var window = new RenderWindow(new VideoMode(ScreenWidth, ScreenHeight), "Flocking");
            var game = new Game(window);

            if (args.Contains("debug"))
                StartDebugScreen(game);

            game.Initialize();
            game.Start();
        }

    public static void StartDebugScreen(Game game)
    {
            var debugWindow = new DebugWindow<Game>(game) { Width = 300 };
            debugWindow.Add(g => $"FPS: {Timer.Fps}");
            debugWindow.Show();
        }
}