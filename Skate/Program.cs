using System;
using Stho.SFML.Extensions;

namespace Skate;

class Program
{
    static void Main(string[] args)
    {
            var window = WindowFactory.CreateDefault();
            var game = new Game(window);
            
            var debug = new DebugWindow<Game>(game);
            debug.Add(g => $"Pos: {g.Skater.Position}");
            debug.Add(g => $"Acl: {g.Skater.Acceleration}");
            debug.Add(g => $"Vel: {g.Skater.Velocity}");
            debug.Add(g => $"Str: {g.Skater.Strength}");
            debug.Add(g => $"Mass: {g.Skater.Mass}");
            debug.Add(g => $"Direction: {g.Skater.Velocity.Normalize()}");
            debug.Show();
            
            game.Initialize();
            game.Start();
        }
}