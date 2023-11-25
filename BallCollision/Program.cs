using System.Linq;
using BallCollision;
using Stho.SFML.Extensions;


var window = WindowFactory.CreateDefault();
var game = new Game(window);

var debug = new DebugWindow<Game>(game);
debug.Add(g => g.Balls.Select(x => x.Position + "\n"));
debug.Show();

game.Initialize();
game.Start();