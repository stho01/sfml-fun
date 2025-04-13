using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace Hexmap.States;

public class DrawingCirclesState : IState
{
    private readonly List<Hexagon[]> _rings = [];
    private int _currentRing;
    
    private readonly HexagonShape _hexagon = new(50) {
        FillColor = Theme.BlueFill,
        OutlineColor = Theme.BlueOutline,
        OutlineThickness = 1,
        Size = Game.HexagonSize
    };

    public string Name => "Drawing Circles";

    public void Load(Game game)
    {
        for (var i = 0; i <= Game.GridRadius; i++)
        {
            var rings = CubeCoordinate.GetRing(CubeCoordinate.Zero, i)
                .Select(coords => new Hexagon { Size = Game.HexagonSize, Coordinates = coords })
                .ToArray();
            
            _rings.Add(rings);
        }
    }

    public void Pause(Game game)
    {
    }
    
    public void Resume(Game game)
    {
    }

    public void Update(Game game)
    {
        if (game.Hovered != null)
        {
            var d = CubeCoordinate.Distance(CubeCoordinate.Zero, game.Hovered.Value);
            _currentRing = (int)d;
        }
    }

    public void Draw(Game game, RenderTarget target)
    {
        foreach (var hexagon in _rings[_currentRing])
        {
            _hexagon.Position = hexagon.Position + game.WindowCenter;
            _hexagon.Size = hexagon.Size;
            target.Draw(_hexagon);
        }
    }
}