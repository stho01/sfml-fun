namespace Maze;

public class Cell
{
    //**********************************************************
    //** enums:
    //**********************************************************

    public enum WallIndex
    {
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3
    }

    //**********************************************************
    //** ctor:
    //**********************************************************

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
        for (var i = 0; i < Walls.Length; i++)
            Walls[i] = true;
    }

    //**********************************************************
    //** props:
    //**********************************************************

    public bool Active { get; set; }
    public bool Visited { get; set; }
    public int X { get; }
    public int Y { get; }
    public bool[] Walls { get; } = new bool[4];

    //**********************************************************
    //** methods:
    //**********************************************************

    public void RemoveWall(WallIndex index) => Walls[(int)index] = false;
}