using System;
using System.Threading;
using System.Threading.Channels;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;
using Timer = Stho.SFML.Extensions.Timer;

namespace GameOfLife;

public enum Mode
{
    Random = 1,
    MouseInput = 2
}
    
public class Game : GameBase
{
    private RectangleShape _shape;
    private bool[,] _grid;
    private int CellSize = 5;
    private readonly int[,] _neighbours = {
            { 0, -1 }, // North
            { 1, -1 }, // North - West.
            { 1, 0 },  // West
            { 1, 1 },  // South - West
            { 0, 1 },  // South
            { -1, 1 }, // South - East
            { -1, 0 }, // East
            { -1, -1 } // North East
        };

    private const int Frequency = 50; // ms
    private static readonly Mode Mode = Mode.MouseInput;
    private Timer.Interval _gameOfLifeUpdateInterval;
            
    public Game(RenderWindow window) : base(window)
    {
            Window.SetTitle("Game of Life");
        }

    private int GridWidth => (int)(WindowWidth / CellSize);
    private int GridHeight => (int)(WindowHeight / CellSize);
        
    public override void Initialize()
    {
            _shape = new RectangleShape(new Vector2f(CellSize, CellSize));
            InitGrid();
            _gameOfLifeUpdateInterval = Timer.SetInterval(Frequency, NextGeneration);
            Window.KeyPressed += OnKeyPressed;
        }

    private void InitGrid()
    {
            _grid = new bool[GridWidth,GridHeight];
            
            for (var x = 0; x < _grid.GetLength(0); x++)
            for (var y = 0; y < _grid.GetLength(1); y++)
            {
                if (Mode == Mode.Random) _grid[x, y] = RandomNumber.Get(0, 100) > 85;
                else _grid[x, y] = false;
            }
        }
        
    private void OnKeyPressed(object sender, KeyEventArgs args)
    {
            switch (args)
            {
                case { Code: Keyboard.Key.Space }:
                    _gameOfLifeUpdateInterval.Pause = !_gameOfLifeUpdateInterval.Pause;
                    break;
            };
        }

    protected override void Update()
    {
            if (Mode == Mode.MouseInput)
                Paint();
        }

    private void Paint()
    {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                _gameOfLifeUpdateInterval.Pause = true;
                var pos = (Vector2f)GetMousePosition();
                var x = (int)Math.Floor(pos.X / WindowWidth * GridWidth);
                var y = (int)Math.Floor(pos.Y / WindowHeight * GridHeight);
                _grid[x, y] = true;
            }
            else
            {
                _gameOfLifeUpdateInterval.Pause = false;
            }
        }

    private void NextGeneration()
    {
            var nextGen = new bool[GridWidth,GridHeight];
            
            for (var x = 0; x < _grid.GetLength(0); x++)
            for (var y = 0; y < _grid.GetLength(1); y++)
            {
                nextGen[x, y] = false; // clear previous calculations
                
                var neighboursAlive = CountAliveNeighbors(_grid, x, y);
                var current = _grid[x, y];
                
                if (!current && neighboursAlive is 3) nextGen[x, y] = true;
                else if (current && neighboursAlive is < 2 or > 3) nextGen[x, y] = false;
                else nextGen[x, y] = _grid[x, y];
            }

            _grid = nextGen;
        }

    private int CountAliveNeighbors(bool[,] grid, int x, int y)
    {
            var neighboursAlive = 0;
            
            for (var i = 0; i < _neighbours.GetLength(0); i++)
            {
                var neighbourX = (_neighbours[i, 0] + x + GridWidth) % GridWidth;
                var neighbourY = (_neighbours[i, 1] + y + GridHeight) % GridHeight;

                if (grid[neighbourX, neighbourY]) neighboursAlive++;
            }

            return neighboursAlive;
        }

    protected override void Render()
    {
            for (var x = 0; x < _grid.GetLength(0); x++)
            for (var y = 0; y < _grid.GetLength(1); y++)
            {
                if (!_grid[x, y])
                    continue;
                
                _shape.Position = new Vector2f(x, y) * CellSize;
                _shape.FillColor = Color.White;
                
                Window.Draw(_shape);
            }
        }
}