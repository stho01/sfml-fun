using SFML.Graphics;
using SFML.System;

namespace Chess;

public class MoveRenderer(RenderTarget renderTarget, Game game)
{
    private readonly RectangleShape _selectedCell = new()
    {
        FillColor = new Color(0x00000000),
        OutlineColor = new Color(0xC2653CFF),
        OutlineThickness = 4,
    };
    
    private readonly RectangleShape _possibleMove = new()
    {
        FillColor = new Color(0x00FF0088)
    };
    
    public void Render(MoveController moveController)
    {
        var board = game.Board;
        
        if (moveController.SelectedCell is not null)
        {
            var screenPosition = board.PositionToScreenCoords(moveController.SelectedCell.Position);
            const int padding = 2;
            
            _selectedCell.Position = new Vector2f(
                screenPosition.X + (_selectedCell.OutlineThickness + padding) / 2f,
                screenPosition.Y + (_selectedCell.OutlineThickness + padding) / 2f);
            _selectedCell.Size = new Vector2f(
                board.CellSize.X - (_selectedCell.OutlineThickness + padding),
                board.CellSize.Y - (_selectedCell.OutlineThickness + padding));
            
            renderTarget.Draw(_selectedCell);

            if (moveController.SelectedCellEligibleMoves is { Length: > 0 })
            {
                foreach (var cell in moveController.SelectedCellEligibleMoves)
                {
                    _possibleMove.Position = (Vector2f)board.PositionToScreenCoords(cell.Position);
                    _possibleMove.Size = board.CellSize;
                    renderTarget.Draw(_possibleMove);
                }
            }
        }
    }
}