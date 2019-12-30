using System;
using System.Threading;
using SFML.Window;
using Stho.SFML.Extensions;

namespace FireWorks
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = WindowFactory.CreateDefault(950, 950);
            window.SetTitle("Fireworks");
            var game = new Game(window);

            window.KeyPressed += (sender, eventArgs) =>
            {
                if (eventArgs.Code == Keyboard.Key.R)
                    game.SpawnRocket();
            };
            
            
            var debug = new DebugWindow<Game>(game);
            debug.Add(g => $"CurrentSpawnTimer: {g.CurrentSpawnTimer}");
            debug.Add(g => $"CurrentSpawnTimeAccumulator: {g.CurrentSpawnTimeAccumulator}");
            debug.Add(g => $"Rocket pos : {g.Rocket?.Position}");
            debug.Add(g => $"Rocket vel : {g.Rocket?.Velocity}");
            debug.Add(g => $"Rocket acl : {g.Rocket?.Acceleration}");
            debug.Add(g => $"Rocket dir : {g.Rocket?.Direction}");
            debug.Add(g => $"Rocket fuel: {g.Rocket?.Fuel}");
            // debug.Add(g => $"Rocket force: {g.Rocket.Fuel}");
            debug.Show();
            
            
            game.Initialize();
            game.Start();
        }
    }
}