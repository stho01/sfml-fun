using Stho.SFML.Extensions;
using TankGame;

var window = WindowFactory.CreateDefault();
var game = new Game(window);
game.Initialize();
game.Start();