using Stho.SFML.Extensions;
using Stho.SFML.Template;

var window = WindowFactory.CreateDefault();
var game = new Game(window);
game.Initialize();
game.Start();