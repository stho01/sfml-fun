using Maze;
using Stho.SFML.Extensions;

var win = WindowFactory.CreateDefault(600, 600);
var game = new Game(win, 10, 10);
game.Initialize();
game.Start();