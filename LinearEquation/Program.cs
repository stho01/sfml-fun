using SFML.Graphics;
using SFML.Window;
using Stho.SFML.Extensions;

namespace LinearEquation;

class Program
{
    private static Game _game;
    private static RenderWindow _window;

    static void Main(string[] args)
    {
        _window = WindowFactory.CreateDefault();
        _game = new Game(_window);

        _window.KeyPressed += OnKeyPress;


        var debugWindow = new DebugWindow<Game>(_game);
        debugWindow.Add(g => $"Intersection: {g.Intersection}");


        debugWindow.Add(g => $"Num1: IntersectionMode X");
        debugWindow.Add(g => $"Num2: IntersectionMode Y");
        debugWindow.Add(g => $"Intersection Mode: {g.IntersectionMode.ToString()}");
        debugWindow.Show();

        _game.Initialize();
        _game.Start();
    }

    static void OnKeyPress(object source, KeyEventArgs args)
    {
        if (args.Code == Keyboard.Key.Num1)
            _game.IntersectionMode = IntersectionMode.X;
        if (args.Code == Keyboard.Key.Num2)
            _game.IntersectionMode = IntersectionMode.Y;
    }
}