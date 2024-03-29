using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;
using Color = SFML.Graphics.Color;

namespace Hexmap;

public class Game(RenderWindow window) : GameBase(window)
{
    private const int Size = 35;
    private const int Radius = 10;
    private readonly HexShape _hex = new(50) {
        FillColor = Color.Transparent,
        OutlineColor = Color.Red,
        OutlineThickness = 1
    };
    private readonly List<Hexagon> _hexagons = [];
    private readonly List<Hexagon[]> _rings = [];
    private CubeCoordinate? _hovered;
    private int _currentRing = 0;
    
    public override void Initialize()
    {
        var center = CubeCoordinate.Zero;
        _hexagons.Add(new Hexagon { Size = Size, Coordinates = center });

        var range = 
            CubeCoordinate
                .GetRange(CubeCoordinate.Zero, Radius)
                .Select(c => new Hexagon { Size = Size, Coordinates = c });
        
        _hexagons.AddRange(range);

        for (var i = 0; i <= Radius; i++)
        {
            var rings = CubeCoordinate.GetRing(CubeCoordinate.Zero, i)
                .Select(c => new Hexagon { Size = Size, Coordinates = c })
                .ToArray();
            
            _rings.Add(rings);
        }

        Timer.SetInterval(250, () => {
            _currentRing = (_currentRing + 1) % _rings.Count;
        });
    }

    protected override void Update()
    {
        _hovered = null;
        var hovered = Hexagon.GetCoordinates(Size, GetMousePosition() - (Vector2i)WindowCenter);
        var distance = (int)CubeCoordinate.Distance(CubeCoordinate.Zero, hovered);

        if (distance <= Radius)
            _hovered = hovered.Round();
    }

    protected override void Render()
    {
        foreach (var hexagon in _hexagons)
        {
            _hex.Position = hexagon.Position + WindowCenter;
            _hex.Size = hexagon.Size;
            _hex.FillColor = hexagon.Coordinates == CubeCoordinate.Zero ? Color.Magenta : Color.Transparent;
            Window.Draw(_hex);
        }

        foreach (var hexagon in _rings[_currentRing])
        {
            _hex.Position = hexagon.Position + WindowCenter;
            _hex.Size = hexagon.Size;
            _hex.FillColor = Color.Blue;
            Window.Draw(_hex);
        }
        
        if (_hovered.HasValue)
        {
            _hex.Position = Hexagon.GetPosition(Size, _hovered.Value) + WindowCenter;
            _hex.Size = Size;
            _hex.FillColor = Color.Green;
            Window.Draw(_hex);
        }
    }
}