using System;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace Minesweeper
{
    public class MinesweeperGame : GameBase
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private readonly Cell[,] _cells;
        private readonly uint _mineCount;
        private readonly MinesweeperRenderer _renderer;
        private Vector2i _cellSize;

        //**********************************************************
        //** ctor:
        //**********************************************************
        
        public MinesweeperGame(RenderWindow window, int boardWidth, int boardHeight, uint mineCount) : base(window)
        {
            _cells = new Cell[boardWidth,boardHeight];
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            _mineCount = mineCount;
            CalculateAndSetCellSize();
            _renderer = new MinesweeperRenderer(this, Window);
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public Theme Theme { get; } = new Theme();
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public Vector2i CellSize => _cellSize;
        public bool GameOver { get; private set; }
        public string GameStatus { get; private set; }
        
        //**********************************************************
        //** methods:
        //**********************************************************

        public override void Initialize()
        {
            Window.SetTitle("Minesweeper");
            Reset();
        }

        public void Reset()
        {
            GameOver = false;
            CreateBoard();
            ForeachCell((cell, x, y) => {
                ScanAreaAround(x, y, (other, otherX, otherY) => {
                    if (other.IsMine) 
                        cell.NeighborMines++;
                });
            });
        }

        private void CreateBoard()
        {
            for (var x = 0; x < BoardWidth; x++)
            for (var y = 0; y < BoardHeight; y++) {
                _cells[x, y] = new Cell(x, y);
            }

            if (BoardWidth * BoardHeight < _mineCount)
                throw new InvalidOperationException("Mine count cannot be greater that max possible placements");

            var placedMines = 0;                
            while (placedMines < _mineCount && _mineCount > 0)
            {
                Cell cell;
                
                do
                {
                    var x = RandomNumber.Get(0, BoardWidth);
                    var y = RandomNumber.Get(0, BoardHeight);
                    cell = _cells[x, y];
                } while (cell.IsMine);

                cell.IsMine = true;
                placedMines++;
            }
        }

        public void ForeachCell(Action<Cell> action) => ForeachCell((cell, x, y) => action(cell));
        public void ForeachCell(Action<Cell, int, int> action)
        {
            for (var x = 0; x < BoardWidth; x++) 
            for (var y = 0; y < BoardHeight; y++)
                action(_cells[x, y], x, y);
        }

        public void ScanAreaAround(int x, int y, Action<Cell, int, int> action)
        {
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
                    action(other, xToCheck, yToCheck);
                }
            }
        }

        protected override void Update()
        {
            CalculateAndSetCellSize();
            var pos = GetMousePosition();
            
            if (!GameOver && Mouse.IsButtonPressed(Mouse.Button.Left) && WindowBounds.Contains(pos.X, pos.Y))
            {
                var cellPos = pos.Divide(CellSize);
                var mineHit = RevealCell(cellPos.X, cellPos.Y);
                
                if (mineHit)
                    EnterGameOver("Game over");
                else if (_cells.Cast<Cell>().Where(x => !x.IsMine).All(x => x.Revelead))
                    EnterGameOver("Congrats comrade!");
            }
        }

        private void EnterGameOver(string state)
        {
            GameOver = true;
            GameStatus = state; 
            ForeachCell((cell) => cell.Revelead = true);  
        }

        private bool RevealCell(int x, int y)
        {
            var cell = _cells[x, y];
            if (cell.Revelead)
                return cell.IsMine;
            
            cell.Revelead = true;
            if (cell.NeighborMines == 0)
                ScanAreaAround(x, y, (_, neighborX, neighborY) => RevealCell(neighborX, neighborY));

            return cell.IsMine;
        }

        protected override void Render()
        {
            _renderer.Render();
        }

        private void CalculateAndSetCellSize()
        {
            _cellSize = new Vector2i(
                (int)(WindowWidth / BoardWidth),    
                (int)(WindowHeight / BoardHeight)    
            );
        }
    }
}