﻿using System;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Minesweeper
{
    public class MinesweeperRenderer
    {
          
        //**********************************************************
        //** fields:
        //**********************************************************

        private readonly MinesweeperGame _minesweeperGame;
        private readonly RenderTarget _renderTarget;
        private readonly RectangleShape _cellShape = new RectangleShape();
        private readonly CircleShape _mineShape = new CircleShape();
        private readonly Text _text = new Text();

        //**********************************************************
        //** ctor:
        //**********************************************************

        public MinesweeperRenderer(MinesweeperGame minesweeperGame, RenderTarget renderTarget)
        {
            _minesweeperGame = minesweeperGame;
            _renderTarget = renderTarget;
            _mineShape.OutlineThickness = -2;
            _mineShape.OutlineColor = Color.Black;
            _text.Font = Fonts.Roboto;
        }
          
        //**********************************************************
        //** methods
        //**********************************************************

        public void Render()
        {
            _minesweeperGame.ForeachCell((cell, x, y) =>
            {
                var position = new Vector2f(
                    (x * _cellShape.Size.X),
                    (y * _cellShape.Size.Y)
                );
                
                RenderCell(cell, position);
                RenderCellContent(cell, position);
            });

            if ( _minesweeperGame.GameOver)
                RenderEndGameStatus(_minesweeperGame.GameStatus);
        }
        
        private void RenderCell(Cell cell, Vector2f position)
        {
            _cellShape.Size = (Vector2f)_minesweeperGame.CellSize;
            _cellShape.FillColor = cell.Revelead ? _minesweeperGame.Theme.RevealedCellColor : _minesweeperGame.Theme.UnrevealedCellColor;
            _cellShape.OutlineThickness = _minesweeperGame.Theme.CellStrokeWidth;
            _cellShape.OutlineColor = _minesweeperGame.Theme.CellStrokeColor;
            _cellShape.Position = position;
            _renderTarget.Draw(_cellShape);
        }

        public void RenderCellContent(Cell cell, Vector2f position)
        {
            if (!cell.Revelead)
                return;
            
            if (cell.IsMine)
            {
                RenderMine(position);
                return;                
            }
            
            if (cell.NeighborMines > 0)
                RenderMineNeighborCount(cell.NeighborMines, position);
        }

        private void RenderMine(Vector2f position)
        {
            _mineShape.Radius = (Math.Min(_minesweeperGame.CellSize.X, _minesweeperGame.CellSize.Y) * 0.25f);
            _mineShape.Origin = new Vector2f(_mineShape.Radius, _mineShape.Radius);
            _mineShape.Position = position + ((Vector2f)_minesweeperGame.CellSize) / 2;
            _mineShape.FillColor = _minesweeperGame.Theme.MineColor;
            _mineShape.OutlineColor = _minesweeperGame.Theme.MineStrokeColor;
            _mineShape.OutlineThickness = _minesweeperGame.Theme.MineStrokeWidth;
            _renderTarget.Draw(_mineShape);
        }

        private void RenderMineNeighborCount(int neighborMines, Vector2f position)
        {
            _text.DisplayedString = neighborMines.ToString();
            _text.FillColor = _minesweeperGame.Theme.TextColor;
            _text.CharacterSize = (uint)(_minesweeperGame.CellSize.Y * 0.8);
            _text.Origin = new Vector2f(_minesweeperGame.CellSize.X / 2, _cellShape.Size.Y/2);
            _text.Position = new Vector2f(
                (position.X + _minesweeperGame.CellSize.X / 2) + _text.GetLocalBounds().Width, 
                position.Y + _minesweeperGame.CellSize.Y / 2);
            _renderTarget.Draw(_text);
        }

        private void RenderEndGameStatus(string status)
        {
            var overlay = new RectangleShape((Vector2f) _renderTarget.Size) {FillColor = new Color(0x000000AA)};
            _renderTarget.Draw(overlay);
            var text = new Text(status, Fonts.Roboto) {CharacterSize = 60, FillColor = Color.White};
            var bounds = text.GetLocalBounds();
            text.Position = new Vector2f(
                _minesweeperGame.WindowWidth/2 - bounds.Width/2,
                _minesweeperGame.WindowHeight/2 - bounds.Height/2    
            );
            _renderTarget.Draw(text);
        }
    }
}