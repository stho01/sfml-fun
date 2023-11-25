using PhysixSimulation;
using SFML.Graphics;
using SFML.Window;

const int screenWidth = 1800;
const int screenHeight = 900;

var window = new RenderWindow(new VideoMode(screenWidth, screenHeight), "Flocking");
var game = new Game(window);
game.Initialize();
game.Start();