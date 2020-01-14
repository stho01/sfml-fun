using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Maze
{
    public class MazeRenderer
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private readonly Game _game;
        private readonly RenderTarget _target;
        private readonly RectangleShape _cellShape = new RectangleShape();
        private readonly Dictionary<Cell.WallIndex, WallRenderFunction> _wallRenderFunctions;
        private Vector2f _currentCellSize;
              
        //**********************************************************
        //** delegates
        //**********************************************************

        delegate (Vector2f, Vector2f) WallRenderFunction(Cell cell, Vector2f position);
          
        //**********************************************************
        //** ctor:
        //**********************************************************

        public MazeRenderer(Game game, RenderTarget target)
        {
            _game = game;
            _target = target;
            _wallRenderFunctions = new Dictionary<Cell.WallIndex, WallRenderFunction>();
            _wallRenderFunctions.Add(Cell.WallIndex.Top, GetTopWallPoints);
            _wallRenderFunctions.Add(Cell.WallIndex.Left, RenderLeftWall);
            _wallRenderFunctions.Add(Cell.WallIndex.Bottom, GetBottomWallPoints);
            _wallRenderFunctions.Add(Cell.WallIndex.Right, GetRightWallPoints);
        }
            
        //**********************************************************
        //** props
        //**********************************************************

        public Color CellFillColor { get; set; } = new Color(0xeaeaeaff);
        public Color WallColor { get; set; } = Color.Black;
        public Color ActiveCellFillColor { get; set; } = Color.Magenta;
        public Color VisitedCellFillColor { get; set; } = Color.Cyan;
        
        //**********************************************************
        //** methods:
        //**********************************************************

        public void Render(Maze maze)
        {
            _currentCellSize = GetCellSize(maze);
            
            for (var x = 0; x < maze.Width; x++)
            for (var y = 0; y < maze.Height; y++)
            {
                var cell = maze.Cells[x, y];
                var position = _currentCellSize.Multiply(x, y);
                RenderSquare(cell, position);
                RenderWalls(cell, position);
            }
        }

        private void RenderSquare(Cell cell, Vector2f position)
        {
            _cellShape.Size = _currentCellSize;
            _cellShape.Position = position;
            
            _cellShape.FillColor = 
                  cell.Active ? ActiveCellFillColor 
                : cell.Visited ? VisitedCellFillColor
                : CellFillColor;
            
            _target.Draw(_cellShape);
        }

        private void RenderWalls(Cell cell, Vector2f position)
        {
            for (var i = 0; i < cell.Walls.Length; i++)
                if (cell.Walls[i] && _wallRenderFunctions.TryGetValue((Cell.WallIndex) i, out var renderFunction))
                {
                    var (p1, p2) = renderFunction(cell, position);
                    _target.Draw(new []
                    {
                        new Vertex(p1, WallColor), 
                        new Vertex(p2, WallColor), 
                    }, PrimitiveType.Lines);
                }
        }

        private (Vector2f, Vector2f) GetTopWallPoints(Cell cell, Vector2f position)
        {
            var p1 = position;
            var p2 = position + new Vector2f(_currentCellSize.X, 0);
            return (p1, p2);
        }
        
        private (Vector2f, Vector2f) GetRightWallPoints(Cell cell, Vector2f position)
        {
            var p1 = position + new Vector2f(_currentCellSize.X, 0);
            var p2 = position + _currentCellSize;
            return (p1, p2);
        }
        
        public (Vector2f, Vector2f) GetBottomWallPoints(Cell cell, Vector2f position)
        {
            var p1 = position + _currentCellSize;
            var p2 = position + new Vector2f(0, _currentCellSize.Y);
            return (p1, p2);
        }
        
        public (Vector2f, Vector2f) RenderLeftWall(Cell cell, Vector2f position)
        {
            var p1 = position + new Vector2f(0, _currentCellSize.Y);
            var p2 = position;
            return (p1, p2);
        }
        
        private Vector2f GetCellSize(Maze maze)
        {
            var size = new Vector2f(
                _target.Size.X / maze.Width,
                _target.Size.Y / maze.Height
            );

            return size;
        }
        
    }
}