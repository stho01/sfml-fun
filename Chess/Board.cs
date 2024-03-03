namespace Chess;

public class Board
{
    public readonly IPiece[] _cells = new IPiece[64];

    public IEnumerable<IPiece> Cells => _cells.ToArray();

    public IPiece GetCell(int x, int y)
    {
        if (IsBetween(x, -1..8) && IsBetween(y, -1..8))
        {
            var index = y * 8 + x;
            return _cells[index];    
        }
    }


    private bool IsBetween(int value, Range minMax)
    {
        return value > minMax.Start.Value 
            && value < minMax.End.Value;
    } 
}