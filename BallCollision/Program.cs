using System;
using System.Linq;
using Stho.SFML.Extensions;

namespace BallCollision
{
    static class Program
    {
        static void Main(string[] args)
        {
            var window = WindowFactory.CreateDefault();
            var game = new Game(window);
            
            var debug = new DebugWindow<Game>(game);
            debug.Add(g => g.Balls.Select(x => x.Position + "\n"));
            debug.Show();
            
            
            game.Initialize();
            game.Start();
        }
    }
}