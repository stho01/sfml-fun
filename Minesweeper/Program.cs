using System;
using SFML.Graphics;
using SFML.Window;
using Stho.SFML.Extensions;

namespace Minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = WindowFactory.CreateDefault(600, 600);
            var w = 20;
            var h = 20;
            var mc = w * h * 0.15f;
            
            using var game = new MinesweeperGame(
                window, 
                w, 
                h, 
                (uint)mc
            );

            window.KeyPressed += (sender, eventArgs) =>
            {
                if (eventArgs.Code == Keyboard.Key.R)
                    game.Reset();
            };
            
            game.Initialize();
            game.Start();
        }
    }
}