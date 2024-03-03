using SFML.System;

namespace Chess;

public enum PieceColor
{
    Light = 0,
    Dark = 1
}

public abstract class Piece(PieceColor color)
{
    public Vector2i Position { get; set; }
    public PieceColor Color { get; set; } = color;
    public abstract bool Move(int x, int y);
}

public class Pawn(PieceColor color) : Piece(color)
{
    public override bool Move(int x, int y)
    {
        throw new NotImplementedException();
    }
}

public class Bishop(PieceColor color) : Piece(color)
{
    public override bool Move(int x, int y)
    {
        throw new NotImplementedException();
    }
}

public class Knight(PieceColor color) : Piece(color)
{
    public override bool Move(int x, int y)
    {
        throw new NotImplementedException();
    }
}

public class Rook(PieceColor color) : Piece(color)
{
    public override bool Move(int x, int y)
    {
        throw new NotImplementedException();
    }
}

public class Queen(PieceColor color) : Piece(color)
{
    public override bool Move(int x, int y)
    {
        throw new NotImplementedException();
    }
}

public class King(PieceColor color) : Piece(color)
{
    public override bool Move(int x, int y)
    {
        throw new NotImplementedException();
    }
}