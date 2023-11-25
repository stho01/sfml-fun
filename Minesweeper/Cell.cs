namespace Minesweeper;

public class Cell(int x, int y)
{
    private int _neighborMines = 0;

    //**********************************************************
    //** method:
    //**********************************************************

    public int X { get; } = x;
    public int Y { get; } = y;
    public bool IsMine { get; set; }
    public bool Revelead { get; set; }

    public int NeighborMines
    {
        get => IsMine ? -1 : _neighborMines;
        set => _neighborMines = value;
    }
}