using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;
using Color = SFML.Graphics.Color;

namespace Chess;

public class BoardRenderer(RenderTarget renderTarget, Game game)
{
    private readonly RectangleShape _border = new() {
        OutlineColor = new Color(200, 200, 200),
        OutlineThickness = 35
    };
    private RectangleShape? _mouseHover;
    private readonly Text _text = new() {
        Font = Fonts.Roboto,
        CharacterSize = 14,
        FillColor = Color.White
    };
    
    private Sprite? _boardSprite;
    public Color BorderColor { get; set; } = new(0x2c1604ff);
    public Color DarkColor { get; set; } = new(0x683914ff);
    public Color LightColor { get; set; } = Color.White;

    public void Initialize()
    {
        var shape = new RectangleShape();
        shape.Size = new Vector2f(1f, 1f);
        var renderTexture = new RenderTexture(8,8);
        
        for (var i = 0; i < 64; i++) {
            var x = i % 8;
            var y = i / 8;
            shape.FillColor = IsDarkTile(x, y) ? DarkColor : LightColor;
            shape.Position = new Vector2f(x, y);
            renderTexture.Draw(shape);
        }

        var texture = new Texture(renderTexture.Texture);
        _boardSprite = new Sprite(texture);
    }

    public void Render(Board board)
    {
        _border.Size = board.Size;
        _border.Position = board.Position;
        _border.OutlineColor = BorderColor;
        renderTarget.Draw(_border);    
        
        if (_boardSprite != null) {
            _boardSprite.Scale = board.CellSize;
            _boardSprite.Position = board.Position;
            renderTarget.Draw(_boardSprite);
        }
        
        var mousePosition = game.GetMousePosition();
        for (var i = 0; i < 64; i++)
        {
            var boundingRect = board.GetBoundingBox(i);
            if (!boundingRect.Contains(mousePosition.ToPoint())) 
                continue;
            
            _mouseHover = new RectangleShape {
                Size = board.CellSize,
                Position = new Vector2f(boundingRect.X, boundingRect.Y) 
            };
            _mouseHover.FillColor =
                IsDarkTile(i)
                    ? new Color(0, 0, 0, 100)
                    : new Color(255, 255, 255, 100);
            
            break;
        }
        
        if (_mouseHover is not null)
        {
            renderTarget.Draw(_mouseHover);
            _mouseHover = null;
        }
        
        for (var i = 0; i < 8; i++)
        {
            var number = 8 - i;
            _text.DisplayedString = number.ToString();
            _text.Position = new Vector2f(-20, (i * board.CellSize.Y) + (board.CellSize.Y / 2f - _text.CharacterSize / 2f)) + board.Position ;
            renderTarget.Draw(_text);

            var character = (char)(i + 0x41);
            _text.DisplayedString = character.ToString();
            _text.Position = new Vector2f((i * board.CellSize.X) + (board.CellSize.X / 2 - _text.CharacterSize / 2f), board.Size.Y + 10) + board.Position; 
            renderTarget.Draw(_text);
        }
    }

    private static bool IsDarkTile(int index)
    {
        var x = index % 8;
        var y = index / 8;
        return IsDarkTile(x, y);
    }
    
    private static bool IsDarkTile(int x, int y)
    {
        var adder = y - 1 % 2;
        return (x + adder) % 2 == 0;
    }
}