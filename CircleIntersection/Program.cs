using System;
using SFML.Graphics;
using SFML.Window;
using Stho.SFML.Extensions;

namespace CircleIntersection
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = WindowFactory.CreateDefault();
            var game = new Game(window);
            window.KeyPressed += (sender, eventArgs) =>
            {
                if (eventArgs.Code == Keyboard.Key.Q)
                    game.Radius = Math.Max(0, game.Radius - 5);
                if (eventArgs.Code == Keyboard.Key.W)
                    game.Radius += 5;
                if (eventArgs.Code == Keyboard.Key.S)
                    Console.WriteLine("Test");
            };
            
            CreateDebugWindow(game);
            
            game.Initialize();
            game.Start();
        }

        static void CreateDebugWindow(Game game)
        {    
            var debugWindow = new DebugWindow<Game>(game);
            debugWindow.Add(g => $"Mouse pos {g.GetMousePosition()}");
            debugWindow.Add(g => $"Sqr distance from each other {g.SqrDistanceFromEachOther}");
            debugWindow.Add(g => $"Sqr radius combined {g.SqrRadius}");
            debugWindow.Add(g => $"Colliding {g.Colliding}");
            debugWindow.Show();
        }
    }
}