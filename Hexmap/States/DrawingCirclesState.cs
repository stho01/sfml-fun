using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using Stho.SFML.Extensions;

namespace Hexmap.States;

public class DrawingCirclesState : IState
{
    private readonly List<Hexagon[]> _rings = [];
    private int _currentRing = 0;
    private Timer.Interval _interval;
    
    private readonly HexShape _hex = new(50) {
        FillColor = Color.Blue,
        OutlineThickness = 1,
        Size = Game.HexSize
    };

    public string Name => "Drawing Circles";

    public void Load(Game game)
    {
        for (var i = 0; i <= Game.GridRadius; i++)
        {
            var rings = CubeCoordinate.GetRing(CubeCoordinate.Zero, i)
                .Select(coords => new Hexagon { Size = Game.HexSize, Coordinates = coords })
                .ToArray();
            
            _rings.Add(rings);
        }
        _interval = Timer.SetInterval(250, () => {
            _currentRing = (_currentRing + 1) % _rings.Count;
        });    
    }

    public void Pause(Game game)
    {
        _interval.Pause = true;
    }
    
    public void Resume(Game game)
    {
        _interval.Pause = false;
    }

    public void Update(Game game) { }

    public void Draw(Game game, RenderTarget target)
    {
        foreach (var hexagon in _rings[_currentRing])
        {
            _hex.Position = hexagon.Position + game.WindowCenter;
            _hex.Size = hexagon.Size;
            target.Draw(_hex);
        }
    }
}