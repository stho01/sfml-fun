using System;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace Minesweeper;

public class MinesweeperGame : GameBase, IDisposable
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly Cell[,] _cells;
    private readonly uint _mineCount;
    private readonly MinesweeperRenderer _renderer;
    private bool _initialized;

    //**********************************************************
    //** ctor:
    //**********************************************************

    public MinesweeperGame(RenderWindow window, int boardWidth, int boardHeight, uint mineCount) : base(window)
    {
        _cells = new Cell[boardWidth, boardHeight];
        BoardWidth = boardWidth;
        BoardHeight = boardHeight;
        _mineCount = mineCount;
        CalculateAndSetCellSize();
        _renderer = new MinesweeperRenderer(this, Window);
    }

    //**********************************************************
    //** props:
    //**********************************************************

    public Theme Theme { get; } = new();
    public int BoardWidth { get; }
    public int BoardHeight { get; }
    public Vector2i CellSize { get; private set; }
    public bool GameOver { get; private set; }
    public string GameStatus { get; private set; }

    //**********************************************************
    //** methods:
    //**********************************************************

    public override void Initialize()
    {
        _initialized = true;
        Window.SetTitle("Minesweeper");
        Window.SetFramerateLimit(10);
        Window.MouseButtonPressed += HandleMouseClick;
        ShowFps = true;
        Reset();
    }

    public void Reset()
    {
        if (!_initialized)
            throw new Exception("Game not initialized...");

        GameOver = false;
        InitializeBoard();
        PlaceMines();
        ForeachCell((cell) =>
        {
            ScanAreaAround(cell, (neighbor) =>
            {
                if (neighbor.IsMine)
                    cell.NeighborMines++;
            });
        });
    }

    private void InitializeBoard()
    {
        for (var x = 0; x < BoardWidth; x++)
        for (var y = 0; y < BoardHeight; y++)
        {
            _cells[x, y] = new Cell(x, y);
        }
    }

    private void PlaceMines()
    {
        if (BoardWidth * BoardHeight < _mineCount)
            throw new InvalidOperationException("Mine count cannot be greater that max possible placements");

        var placedMines = 0;
        while (placedMines < _mineCount && _mineCount > 0)
        {
            Cell cell;
            do
            {
                cell = _cells[
                    RandomNumber.Get(..BoardWidth),
                    RandomNumber.Get(..BoardHeight)
                ];
            } while (cell.IsMine);

            cell.IsMine = true;
            placedMines++;
        }
    }

    public void ForeachCell(Action<Cell> action)
    {
        for (var x = 0; x < BoardWidth; x++)
        for (var y = 0; y < BoardHeight; y++)
            action(_cells[x, y]);
    }

    public void ScanAreaAround(Cell cell, Action<Cell> action)
    {
        var x = cell.X;
        var y = cell.Y;

        for (var xOff = -1; xOff <= 1; xOff++)
        for (var yOff = -1; yOff <= 1; yOff++)
        {
            var xToCheck = x + xOff;
            var yToCheck = y + yOff;

            if (xToCheck >= 0
                && yToCheck >= 0
                && xToCheck < BoardWidth
                && yToCheck < BoardHeight
                && _cells[xToCheck, yToCheck] != _cells[x, y])
            {
                var other = _cells[xToCheck, yToCheck];
                action(other);
            }
        }
    }

    protected override void Update()
    {
        CalculateAndSetCellSize();
    }

    private void EnterGameOver(string status)
    {
        GameOver = true;
        GameStatus = status;
        ForeachCell((cell) => cell.Revelead = true);
    }

    private bool RevealCell(Cell cell)
    {
        if (cell.Revelead)
            return cell.IsMine;

        cell.Revelead = true;
        if (cell.NeighborMines == 0)
            ScanAreaAround(cell, (neighbor) => RevealCell(neighbor));

        return cell.IsMine;
    }

    protected override void Render() => _renderer.Render();

    private void CalculateAndSetCellSize()
    {
        CellSize = new Vector2i(
            (int)(WindowWidth / BoardWidth),
            (int)(WindowHeight / BoardHeight)
        );
    }

    public void Dispose()
    {
        Window.MouseButtonPressed -= HandleMouseClick;
    }

    //**********************************************************
    //** event handlers
    //**********************************************************

    private void HandleMouseClick(object source, MouseButtonEventArgs args)
    {
        if (args.Button != Mouse.Button.Left)
            return;

        var pos = GetMousePosition();
        if (!GameOver && Mouse.IsButtonPressed(Mouse.Button.Left) && WindowBounds.Contains(pos.X, pos.Y))
        {
            var cellPos = pos.Divide(CellSize);
            var cell = _cells[cellPos.X, cellPos.Y];
            var mineHit = RevealCell(cell);

            if (mineHit)
                EnterGameOver("Game over");
            else if (_cells.Cast<Cell>().Where(x => !x.IsMine).All(x => x.Revelead))
                EnterGameOver("Congrats comrade!");
        }
    }
}