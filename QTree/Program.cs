using System.Linq;
using SFML.Graphics;
using SFML.Window;
using Stho.SFML.Extensions;
using Timer = Stho.SFML.Extensions.Timer;

namespace QTree
{
    class Program
    {
        public const int ScreenWidth = 1200; 
        public const int ScreenHeight = 675; 
        public const int NumberOfParticles = 1000; 
        
        static void Main(string[] args)
        {
            var videoMode = new VideoMode(ScreenWidth, ScreenHeight);
            var settings = new ContextSettings { AntialiasingLevel = 8 };
            var window = new RenderWindow(videoMode, "Quad Tree", Styles.Default, settings);
            var game = new Game(window, NumberOfParticles);

            DebugWindow(game);
            game.Initialize();
            game.Start();
        }

        static void DebugWindow(Game game)
        {
            var debug = new DebugWindow<Game>(game);
            debug.Add(g => $"FPS: {Timer.Fps}");
            debug.Add(g => $"Particles colliding: {g.Particles.Count(x => x.Colliding)}");
            debug.Add(g => $"Largest cluster: {g.LargestCluster}");
            // debug.Add(g => $"Interactive particles position: {g.InteractiveParticle.Position}");
            // debug.Add(g => $"Interactive Colliding: {g.InteractiveParticle.Colliding}");
            // debug.Add(g => $"Interactive particles radius: {g.InteractiveParticle.Radius}");
            debug.Show();
        }
    }
}