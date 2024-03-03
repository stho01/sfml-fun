using System.Drawing;
using SFML.System;

namespace Chess;

public class Board
{
    private const int MaxTileCount = 8;
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
    public Vector2f Position { get; set; } = new(0f, 0f);
    public Vector2f Size { get; set; } = new(600f, 600f);
    public Vector2f CellSize => Size / MaxTileCount;

    public Cell? GetCellByScreenPosition(Vector2i pos) => GetCellByScreenPosition(pos.X, pos.Y);
    public Cell? GetCellByScreenPosition(int x, int y)
    {
        var xn = (int)((x - Position.X) / Size.X * MaxTileCount);
        var yn = (int)((y - Position.Y) / Size.Y * MaxTileCount);
        return GetCell(xn, yn);
    }

    public Cell? GetCell(int x, int y)
    {
        if (!InRange(x) || !InRange(y))
            return null;
        
        var index = y * MaxTileCount + x;
        return _cells[index];    
    }

    public bool PlacePieceByScreenPosition(Vector2i pos, Piece piece) =>
        PlacePieceByScreenPosition(pos.X, pos.Y, piece);
    public bool PlacePieceByScreenPosition(int x, int y, Piece piece)
    {
        var cell = GetCellByScreenPosition(x, y);
        if (cell != null) {
            cell.Piece = piece;
            cell.Piece.Position = new Vector2i(cell.X, cell.Y);
            return true;
        }
        return false;
    }
    
    public bool PlacePiece(int x, int y, Piece piece)
    {
        var cell = GetCell(x, y);
        if (cell != null) {
            cell.Piece = piece;
            cell.Piece.Position = new Vector2i(cell.X, cell.Y);
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
        public int X { get; } = x;
        public int Y { get; } = y;
        public Piece? Piece { get; set; }
        public bool IsOccupied => Piece is not null;
    }

    public void Clear() 
    {
        foreach (var cell in _cells)
            cell.Piece = null;
    }
}

