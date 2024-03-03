using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Chess;

public class PieceRenderer(RenderTarget renderTarget, SpriteAtlas spriteAtlas)
{
    public Vector2f Scale { get; set; } = new(1, 1);
    
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
        var name = GetSpriteName(piece);
        var sprite = spriteAtlas.GetSprite(name);

        if (sprite is not null) 
        {
            sprite.Scale = Scale;    
        }

        return sprite;
    }
    private static string GetSpriteName(Piece piece)
    {
        var name = piece.GetType().Name;
        return $"{name}-{piece.Color}";
    }
}