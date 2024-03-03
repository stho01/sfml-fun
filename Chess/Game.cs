using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Chess;

public class Game(RenderWindow window) : GameBase(window)
{
    public readonly IPiece[,] _board = new IPiece[8,8];
    public RectangleShape _cell;
    
    public override void Initialize()
    {
        var cellSize = new Vector2i((int)WindowWidth, (int)WindowHeight) / 8;
        _cell = new RectangleShape((Vector2f)cellSize);
    }

    protected override void Update()
    {
        
    }

    protected override void Render()
    {
        for (var x = 0; x < _board.GetLength(0); x++)
        for (var y = 0; y < _board.GetLength(1); y++)
        {
            var adder = (y-1 % 2);
            var isDark = (x + adder) % 2 == 0;
            
            _cell.FillColor = isDark ? Color.Black : Color.White;
            _cell.Position = _cell.Size.Multiply(new Vector2f(x, y));
            Window.Draw(_cell);
        }
    }
}