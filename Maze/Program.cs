using System;
using Stho.SFML.Extensions;

namespace Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            var win = WindowFactory.CreateDefault(600, 600);
            var game = new Game(win, 10, 10);
            game.Initialize();
            game.Start();
        }
    }
}