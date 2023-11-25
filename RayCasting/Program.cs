using System;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace RayCasting;

class Program
{
    static void Main(string[] args)
    {
            var window = WindowFactory.CreateDefault();
            var game = new Game(window);

            window.KeyPressed += (sender, eventArgs) =>
            {
                if (eventArgs.Code == Keyboard.Key.R)
                    game.Reset();
            };
            
            // var debugWindow = new DebugWindow<Game>(game);
            // debugWindow.Add(g => $"Intersects {g.Intersection}");
            // debugWindow.Show();
            
            game.Initialize();
            game.Start();
        }
}