using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Hexmap;

public class Game(RenderWindow window) : GameBase(window)
{
    private readonly HexShape _hex = new(50) {
        FillColor = Color.Transparent,
        OutlineColor = Color.Red,
        OutlineThickness = 1
    };
    
    private readonly List<Hexagon> _hexagons = [];
    
    public override void Initialize()
    {
        _hexagons.Add(new Hexagon { Size = 50, Coordinates = (0,0,0) });
        _hexagons.Add(new Hexagon { Size = 50, Coordinates = (1,0,1) });
        _hexagons.Add(new Hexagon { Size = 50, Coordinates = (0,1,1) });
    }

    protected override void Update()
    {
        
    }

    protected override void Render()
    {
        foreach (var hexagon in _hexagons)
        {
            _hex.Position = hexagon.Position + WindowCenter;
            Window.Draw(_hex);    
        }
    }
}


public class Hexagon
{
    private static readonly float CubeSquared = MathF.Sqrt(3);
    private readonly CubeCoordinate _cubeCoordinate;
    private float _size;

    public CubeCoordinate Coordinates
    {
        get => _cubeCoordinate;
        init
        {
            _cubeCoordinate = value;
            UpdatePosition();
        }
    }

    public float Size
    {
        get => _size;
        set
        {
            _size = value;
            UpdateDimensions();
            UpdatePosition();
        }
    }

    public float Width { get; private set; }
    public float Height { get; private set; }
    public Vector2f Position { get; private set; }


    private void UpdateDimensions()
    {
        Width = CubeSquared * _size;
        Height = 1.5F * _size;
    }
    
    private void UpdatePosition()
    {
        var x = _size * (CubeSquared * Coordinates.Q + CubeSquared / 2 * Coordinates.R);
        var y = _size * (1.5F * Coordinates.R);
        Position = new Vector2f(x, y);
    }
}

public readonly record struct CubeCoordinate(float Q, float R, float S)
{
    public static readonly CubeCoordinate Zero = new();
    public static readonly CubeCoordinate NorthWest = (0, -1, 1);

    public static implicit operator CubeCoordinate((float, float, float) values) 
        => new(values.Item1, values.Item2, values.Item3);
}

