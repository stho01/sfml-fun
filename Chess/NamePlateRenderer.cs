using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Chess;

public class NamePlateRenderer(RenderTarget renderTarget, Game game)
{
    private const string White = "WHITE";
    private const string Black = "BLACK";
    private readonly Text _currentPlayerName = new() 
    {
        Font = Fonts.Roboto,
        Position = new Vector2f(0f, 0f)
    };

    private readonly RectangleShape _rectangle = new();
    
    public void Render()
    {
        _currentPlayerName.DisplayedString = game.CurrentPlayer == PieceColor.White ? White : Black;
        _currentPlayerName.FillColor = game.CurrentPlayer == PieceColor.White ? Color.Black : Color.White;
        var textBounds = _currentPlayerName.GetGlobalBounds();
        _currentPlayerName.Position = new Vector2f(game.WindowCenter.X - textBounds.Width/2, 0f) ;

        _rectangle.Size = new Vector2f(game.WindowWidth, textBounds.Height + 20f);
        _rectangle.Position = new Vector2f(0f, textBounds.Height / 2f - 10f);
        _rectangle.FillColor = game.CurrentPlayer == PieceColor.White ? Color.White : Color.Black;
        
        renderTarget.Draw(_rectangle);
        renderTarget.Draw(_currentPlayerName);
    }
}