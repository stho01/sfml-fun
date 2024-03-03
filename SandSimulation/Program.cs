using SandSimulation;
using Stho.SFML.Extensions;

// var grainSize = 3;
// var screenSize = (grainSize * 300);


var grainSize = 10;
var screenSize = (grainSize * 140);

var window = WindowFactory.CreateDefault(
    (uint)screenSize,
    (uint)screenSize);

var game = new Game(window, grainSize);
game.Initialize();
game.Start();