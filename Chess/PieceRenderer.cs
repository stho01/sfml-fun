using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Chess;

public class PieceRenderer(RenderTarget renderTarget, SpriteAtlas spriteAtlas)
{
    public Vector2f Scale { get; set; } = new(1, 1);

    private readonly Dictionary<(PieceType, PieceColor), string> _spriteNames = new();

    public void Initialize()
    {
        foreach (var pieceType in Enum.GetValues<PieceType>())
        foreach (var pieceColor in Enum.GetValues<PieceColor>())
        {
            _spriteNames.Add((pieceType, pieceColor), SpriteAtlas.NormalizeName($"{pieceType}-{pieceColor}"));
        }
    }
    
    public void Render(Board board, Piece piece)
    {        
        var sprite = GetSprite(piece);
        
        if (sprite != null)
        {
            var spriteCenter = Scale.Multiply(GetSpriteCenter(sprite));
            
            sprite.Position = 
                board.CellSize.Multiply(piece.Position) 
                + (board.CellSize / 2) 
                + board.Position 
                - spriteCenter;
            
            renderTarget.Draw(sprite);    
        }
    }

    private static Vector2f GetSpriteCenter(Sprite sprite)
    {
        var dim = new Vector2f(sprite.TextureRect.Width, sprite.TextureRect.Height);
        return dim / 2;
    }

    private Sprite? GetSprite(Piece piece)
    {
        var sprite = spriteAtlas.GetSprite(_spriteNames[(piece.PieceType, piece.Color)]);

        if (sprite is not null) 
            sprite.Scale = Scale;    

        return sprite;
    }
}