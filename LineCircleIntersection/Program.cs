using System;
using Stho.SFML.Extensions;

namespace LineCircleIntersection;

class Program
{
    static void Main(string[] args)
    {
            var window = WindowFactory.CreateDefault();
            var game = new Game(window) {ShowFps = true};
            game.Initialize();
            game.Start();
        }
}