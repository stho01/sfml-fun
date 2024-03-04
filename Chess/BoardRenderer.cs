using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;
using Color = SFML.Graphics.Color;

namespace Chess;

public class BoardRenderer(RenderTarget renderTarget, Game game)
{
    private readonly RectangleShape _border = new() {
        OutlineThickness = 35
    };
    private readonly RectangleShape _selectedCell = new()
    {
        FillColor = new Color(0x00000000),
        OutlineColor = new Color(0xC2653CFF),
        OutlineThickness = 4,
    };
    private RectangleShape? _mouseHover;
    private readonly Text _text = new() {
        Font = Fonts.Roboto,
        CharacterSize = 14,
        FillColor = Color.White
    };
    private Sprite? _boardSprite;
    
    public Color BorderColor { get; set; } = new(0x2C1604FF);
    public Color DarkColor { get; set; } = new(0x683914FF);
    public Color LightColor { get; set; } = Color.White;

    public void Initialize()
    {
        var shape = new RectangleShape();
        shape.Size = new Vector2f(1f, 1f);
        
        var renderTexture = new RenderTexture(
            Board.MaxTileCount,
            Board.MaxTileCount);
        
        for (var i = 0; i < 64; i++) {
            var x = i % Board.MaxTileCount;
            var y = i / Board.MaxTileCount;
            shape.FillColor = IsDarkTile(x, y) ? DarkColor : LightColor;
            shape.Position = new Vector2f(x, y);
            renderTexture.Draw(shape);
        }

        _boardSprite = new Sprite(new Texture(renderTexture.Texture));
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
        
        for (var i = 0; i < Board.MaxTileCount; i++)
        {
            var number = Board.MaxTileCount - i;
            _text.DisplayedString = number.ToString();
            _text.Position = new Vector2f(-20, i * board.CellSize.Y + (board.CellSize.Y / 2f - _text.CharacterSize / 2f)) + board.Position ;
            renderTarget.Draw(_text);

            var character = (char)(i + 0x41);
            _text.DisplayedString = character.ToString();
            _text.Position = new Vector2f(i * board.CellSize.X + (board.CellSize.X / 2 - _text.CharacterSize / 2f), board.Size.Y + 10) + board.Position; 
            renderTarget.Draw(_text);
        }

        if (board.SelectedCell is not null)
        {
            var screenPosition = board.PositionToScreenCoords(board.SelectedCell.Position);
            const int padding = 2;
            
            _selectedCell.Position = new Vector2f(
                screenPosition.X + (_selectedCell.OutlineThickness + padding) / 2f,
                screenPosition.Y + (_selectedCell.OutlineThickness + padding) / 2f);
            _selectedCell.Size = new Vector2f(
                board.CellSize.X - (_selectedCell.OutlineThickness + padding),
                board.CellSize.Y - (_selectedCell.OutlineThickness + padding));
            
            renderTarget.Draw(_selectedCell);
        }
    }

    private static bool IsDarkTile(int index)
    {
        var x = index % Board.MaxTileCount;
        var y = index / Board.MaxTileCount;
        return IsDarkTile(x, y);
    }
    
    private static bool IsDarkTile(int x, int y)
    {
        var adder = y - 1 % 2;
        return (x + adder) % 2 == 0;
    }
}