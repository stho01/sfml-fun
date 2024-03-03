namespace Chess;

public class Board
{
    public readonly Piece[] _cells = new Piece[64];

    public Piece[] Cells => _cells;

    public Piece GetCell(int x, int y)
    {
        if (!InRange(x) || !InRange(y))
            throw new InvalidOperationException("Out of range");
        
        var index = y * 8 + x;
        return _cells[index];    
    }

    private static bool InRange(int value) 
        => value is >= 0 and < 8;
}