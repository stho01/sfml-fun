using System.Drawing;
using SFML.System;
using Stho.SFML.Extensions;

namespace Chess;

public class Board
{
    public const int MaxTileCount = 8;
    private readonly Cell[] _cells = new Cell[64];
    
    public Board()
    {
        for (var x = 0; x < MaxTileCount; x++)
        for (var y = 0; y < MaxTileCount; y++)
        {
            var i = y * MaxTileCount + x;
            _cells[i] = new Cell(x, y);
        }
    }
    
    public IEnumerable<Cell> Cells => _cells;
    public Cell? SelectedCell { get; private set; } 
    public Vector2f Position { get; set; } = new(0f, 0f);
    public Vector2f Size { get; set; } = new(600f, 600f);
    public Vector2f CellSize => Size / MaxTileCount;


    public Cell? GetCell(Vector2i position) => GetCell(position.X, position.Y);
    public Cell? GetCell(int x, int y)
    {
        if (!InRange(x) || !InRange(y))
            return null;
        
        var index = y * MaxTileCount + x;
        return _cells[index];    
    }

    public bool PlacePiece(int x, int y, Piece piece)
    {
        var cell = GetCell(x, y);
        
        if (cell != null) 
        {
            cell.Piece = piece;
            return true;
        }
        
        return false;
    }

    private static bool InRange(int value) 
        => value is >= 0 and < MaxTileCount;

    public Rectangle GetBoundingBox(int index)
    {
        var x = index % MaxTileCount;
        var y = index / MaxTileCount;
        return GetBoundingBox(x, y);
    }
    
    public Rectangle GetBoundingBox(int x, int y)
    {
        if (!InRange(x) || !InRange(y))
            throw new InvalidOperationException("Out of range");
        
        return new Rectangle 
        {
            X = (int)(x * CellSize.X + Position.X),
            Y = (int)(y * CellSize.Y + Position.Y),
            Width = (int)CellSize.X,
            Height = (int)CellSize.Y
        };
    }
    
    public class Cell(int x, int y)
    {
        private Piece? _piece = null;
        
        public Vector2i Position { get; } = new(x, y);

        public Piece? Piece
        {
            get => _piece;
            set 
            {
                _piece = value;
                if (_piece is not null)
                    _piece.Position = Position;
            }
        }
        
        public bool IsOccupied => Piece is not null;
    }

    public void Clear() 
    {
        foreach (var cell in _cells)
            cell.Piece = null;
    }

    public static bool MovePiece(Cell? sourceCell, Cell? destinationCell)
    {
        if (sourceCell is { IsOccupied: true } && destinationCell is not null)
        {
            var sourcePiece = sourceCell.Piece!;
            var destPiece = destinationCell.Piece;

            if (!destinationCell.IsOccupied || (destPiece != null && destPiece.Color != sourcePiece.Color))
            {
                var piece = sourceCell.Piece!;
                destinationCell.Piece = piece;
                sourceCell.Piece = null;
                return true;    
            }
        }
        return false;
    }

    public bool MoveSelectedPiece(Vector2i position)
    {
        if (MovePiece(SelectedCell, GetCell(position)))
        {
            SelectedCell = null;
            return true;
        }
        
        return false;
    }
    
    public void SelectCell(Vector2i position)
    {
        var cell = GetCell(position);
        if (cell is { IsOccupied: true })
            SelectedCell = cell;
    }

    public bool IsSelectedCell(Vector2i position)
    {
        var cell = GetCell(position);
        return cell == SelectedCell;
    }

    public void DeselectCell()
    {
        SelectedCell = null;
    }
    
    public Vector2i PositionFromScreenCoords(Vector2i screenCoords)
    {
        var xn = (int)((screenCoords.X - Position.X) / Size.X * MaxTileCount);
        var yn = (int)((screenCoords.Y - Position.Y) / Size.Y * MaxTileCount);
        
        return new(xn, yn);
    }

    public Vector2i PositionToScreenCoords(Vector2i boardPosition)
    {
        var xn = (int)(boardPosition.X / 8f * Size.X + Position.X);
        var yn = (int)(boardPosition.Y / 8f * Size.Y + Position.Y);
        return new(xn, yn);
    }

    
}

