using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace Hexmap.States;

public sealed class DrawingLinesState : IState
{
    private List<CubeCoordinate> _line = []; 
    private readonly HexShape _hex = new(50) {
        FillColor = Color.Blue,
        OutlineThickness = 1,
        Size = Game.HexSize
    };
    
    public void Load(Game game) { }
    public void Suspend(Game game) { }
    public void Update(Game game)
    {
        if (game.Hovered.HasValue)
        {
            _line = CubeCoordinate.GetLine(CubeCoordinate.Zero, game.Hovered.Value).ToList();
        }
    }

    public void Draw(Game game, RenderTarget target)
    {
        foreach (var coords in _line)
        {
            _hex.Position = Hexagon.GetPosition(Game.HexSize, coords) + game.WindowCenter;
            target.Draw(_hex);
        }
    }
}