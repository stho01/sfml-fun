using LineCircleIntersection;
using Stho.SFML.Extensions;

var window = WindowFactory.CreateDefault();
var game = new Game(window) { ShowFps = true };
game.Initialize();
game.Start();