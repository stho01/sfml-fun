using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

namespace Hexmap.States;

public class IntersectingRangesState : IState
{
    public string Name => "Intersecting ranges";

    private HexagonRange _range1;
    private HexagonRange _range2;
    private IEnumerable<CubeCoordinate> _intersection = [];

    private readonly HexagonShape _shape1 = new(Game.HexagonSize)
    {
        FillColor = Theme.BlueFill,
        OutlineColor = Theme.BlueOutline,
        OutlineThickness = 1
    };
    private readonly HexagonShape _shape2 = new(Game.HexagonSize)
    {
        FillColor = Theme.MagentaFill,
        OutlineColor = Theme.MagentaOutline,
        OutlineThickness = 1
    };
    private readonly HexagonShape _intersectionShape = new(Game.HexagonSize)
    {
        FillColor = Theme.GreenFill,
        OutlineColor = Theme.GreenOutline,
        OutlineThickness = 1
    };
    
    public void Load(Game game)
    {
        _range1 = HexagonRange.Create(3);
        _range2 = HexagonRange.Create(3);
    }

    public void Pause(Game game){}
    public void Resume(Game game){}

    public void Update(Game game)
    {
        if (game.Hovered != null)
            _range1.CenterCoordinates = game.Hovered.Value;
        
        _intersection = HexagonRange.GetIntersection(_range1, _range2);
    }

    public void Draw(Game game, RenderTarget target)
    {
        foreach (var hex1 in _range1)
        {
            var coords = hex1;// + _range1.CenterCoordinates;
            var distanceFromCenter = CubeCoordinate.Distance(CubeCoordinate.Zero, coords);
            if (distanceFromCenter > Game.GridRadius)
                continue;
            
            _shape1.Position = Hexagon.GetPosition(Game.HexagonSize, coords) + game.WindowCenter;
            target.Draw(_shape1);
        }
        
        foreach (var hex2 in _range2)
        {
            var distanceFromCenter = CubeCoordinate.Distance(hex2, CubeCoordinate.Zero);
            if (distanceFromCenter > Game.GridRadius)
                continue;
            
            _shape2.Position = Hexagon.GetPosition(Game.HexagonSize, hex2) + game.WindowCenter;
            target.Draw(_shape2);
        }

        foreach (var hex in _intersection)
        {
            var distanceFromCenter = CubeCoordinate.Distance(hex, CubeCoordinate.Zero);
            if (distanceFromCenter > Game.GridRadius)
                continue;
            
            _intersectionShape.Position = Hexagon.GetPosition(Game.HexagonSize, hex) + game.WindowCenter;
            target.Draw(_intersectionShape);
        }
    }
}