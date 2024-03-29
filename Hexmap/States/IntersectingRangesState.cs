using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using SFML.Graphics;

namespace Hexmap.States;

public class IntersectingRangesState : IState
{
    public string Name => "Intersecting ranges";

    private List<CubeCoordinate> _range1;
    private List<CubeCoordinate> _range2;

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
    
    public void Load(Game game)
    {
        _range1 = CubeCoordinate.GetRange(3).ToList();
        _range2 = CubeCoordinate.GetRange(3)
            .Select(coords => coords + CubeCoordinate.SouthWest)
            .ToList();
    }

    public void Pause(Game game)
    {
        
    }

    public void Resume(Game game)
    {
        
    }

    public void Update(Game game)
    {
        
    }

    public void Draw(Game game, RenderTarget target)
    {
        foreach (var hex1 in _range1)
        {
            _shape1.Position = Hexagon.GetPosition(Game.HexagonSize, hex1);
            target.Draw(_shape1);
        }
        foreach (var hex2 in _range2)
        {
            _shape2.Position = Hexagon.GetPosition(Game.HexagonSize, hex2);
            target.Draw(_shape2);
        }
    }
}