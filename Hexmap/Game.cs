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
    private readonly List<Hexagon> _ring = [];
    private CubeCoordinate? _hovered;
    
    public override void Initialize()
    {
        var center = CubeCoordinate.Zero;
        _hexagons.Add(new Hexagon { Size = Size, Coordinates = center });

        var range = 
            CubeCoordinate
                .GetRange(CubeCoordinate.Zero, Radius)
                .Select(c => new Hexagon { Size = Size, Coordinates = c });
        
        _hexagons.AddRange(range);

        // var ring = CubeCoordinate.GetRing(CubeCoordinate.Zero, 5)
        //     .Select(c => new Hexagon { Size = Size, Coordinates = c });
        // _ring.AddRange(ring);

    }

    // private static IEnumerable<CubeCoordinate> CubeRing(CubeCoordinate center, int radius) 
    // {
    //     var cursor = center + CubeCoordinate.East * radius;
    //     yield return cursor;
    //     
    //     for (var i = 0; i < 6; i++)
    //     for (var j = 0; j < radius; j++)
    //     {
    //         cursor = cursor.GetNeighbor((CubeCoordinate.Direction)i);
    //         yield return cursor;
    //     }
    // }

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
            _hex.FillColor = Color.Transparent;
            Window.Draw(_hex);
        }

        foreach (var hexagon in _ring)
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