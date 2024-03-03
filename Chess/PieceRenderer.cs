using SFML.Graphics;
using SFML.System;

namespace Chess;

public class PieceRenderer(RenderTarget renderTarget, SpriteAtlas spriteAtlas)
{
    private CircleShape _shape = new(10f);
    public Vector2f Scale { get; set; } = new(1, 1);
    
    public void Render(Piece piece, Vector2f position)
    {        
        var sprite = GetSprite(piece);
        
        if (sprite != null) {
            sprite.Position = position;
            renderTarget.Draw(sprite);    
        }
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