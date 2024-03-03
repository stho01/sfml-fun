// See https://aka.ms/new-console-template for more information

using Chess;
using Stho.SFML.Extensions;

var window = WindowFactory.CreateDefault(1200, 1200);
var game = new Game(window);
game.Initialize();
game.Start();
