using SFML.System;

namespace Chess;

public enum PieceColor
{
    White = 0,
    Black = 1
}

public enum PieceType
{
    Pawn,
    Bishop,
    Knight,
    Rook,
    Queen,
    King
}

public class Piece(PieceType type, PieceColor color)
{
  
    
    public Vector2i Position { get; private set; }
    public PieceType PieceType { get; } = type;
    public PieceColor Color { get; } = color;
    public int MoveCounter { get; private set; }

    public void SetPosition(Vector2i position)
    {
        Position = position;
        MoveCounter++;
    }
    
    

    public override string ToString() {
        return $"Piece {{ Pos={Position}, PieceType={PieceType}, Color={Color}, MoveCounter={MoveCounter} }}";
    }
}