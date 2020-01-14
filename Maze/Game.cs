using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Maze
{
    public class Game : GameBase
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private readonly Maze _maze;
        private readonly MazeRenderer _mazeRenderer;
        private Vector2i _currentPos = new Vector2i(0, 0);
        private Cell.WallIndex _dir = Cell.WallIndex.Bottom;
        
        //**********************************************************
        //** ctor:
        //**********************************************************

        public Game(RenderWindow window, int width, int height) : base(window)
        {
            Width = width;
            Height = height;
            _maze = new Maze(width, height);
            _mazeRenderer = new MazeRenderer(this, Window);
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public int Width { get; }
        public int Height { get; }
  
        //**********************************************************
        //** methods:
        //**********************************************************

        public override void Initialize()
        {
            ShowFps = true;
            Window.SetFramerateLimit(3);
            InitializeMaze();
        }

        private void InitializeMaze()
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                _maze.Cells[x, y] = new Cell(x, y) { Active = x == 0 && y == 0 };
        }

        protected override void Update()
        {
            var vel = GetVelocity(_dir);
            _currentPos += vel;
            _maze.Cells[_currentPos.X, _currentPos.Y].Visited = true;
        }

        private Vector2i GetVelocity(Cell.WallIndex dir)
        {
            switch (dir)
            {
                case Cell.WallIndex.Top: return new Vector2i(0, -1);
                case Cell.WallIndex.Right: return new Vector2i(1, 0);
                case Cell.WallIndex.Bottom: return new Vector2i(0, 1);
                case Cell.WallIndex.Left: return new Vector2i(-1, 0);
            }
            return new Vector2i();
        }
        
        protected override void Render()
        {
            _mazeRenderer.Render(_maze);
        }
    }
}