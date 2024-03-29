using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace Hexmap.States;

public sealed class DrawingLinesState : IState
{
    private List<CubeCoordinate> _line = []; 
    private readonly HexagonShape _hexagon = new(Game.HexagonSize) {
        FillColor = Theme.BlueFill,
        OutlineColor = Theme.BlueOutline,
        OutlineThickness = 1
    };

    public string? Name => "Drawing lines";
    public void Load(Game game) { }
    public void Pause(Game game) { }
    public void Resume(Game game) { }
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
            _hexagon.Position = Hexagon.GetPosition(Game.HexagonSize, coords) + game.WindowCenter;
            target.Draw(_hexagon);
        }
    }
}