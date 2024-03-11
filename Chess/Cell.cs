using SFML.System;

namespace Chess;

public class Cell(int x, int y)
{
    private Piece? _piece;
        
    public Vector2i Position { get; } = new(x, y);

    public Piece? Piece
    {
        get => _piece;
        set 
        {
            _piece = value;
            _piece?.SetPosition(Position);
        }
    }

    public override string ToString() 
        => $"Cell {{ Position = {Position}, Piece = {Piece} }}";
}