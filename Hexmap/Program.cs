using Hexmap;
using Stho.SFML.Extensions;

var window = WindowFactory.CreateDefault(1920, 1280);
var game = new Game(window);
game.Initialize();
game.Start();