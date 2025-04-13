using System;
using SFML.System;

namespace Hexmap;

public class Hexagon
{
    private static readonly float CubeSquared = MathF.Sqrt(3);
    private readonly CubeCoordinate _cubeCoordinate;
    private int _size;

    public CubeCoordinate Coordinates
    {
        get => _cubeCoordinate;
        init
        {
            _cubeCoordinate = value;
            UpdatePosition();
        }
    }

    public int Size
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
        Position = GetPosition(_size, Coordinates);
    }

    public static CubeCoordinate GetCoordinates(int hexSize, Vector2i position) => 
        GetCoordinates(hexSize, (Vector2f)position);
    public static CubeCoordinate GetCoordinates(int hexSize, Vector2f position)
    {
        var q = (CubeSquared/3f * position.X - 0.33333334F * position.Y) / hexSize;
        var r = 0.6666667F * position.Y / hexSize;
        var s = -q - r;
        
        return (q, r, s);
    }

    public static Vector2f GetPosition(int hexSize, CubeCoordinate coordinates)
    {
        var x = hexSize * (CubeSquared * coordinates.Q + CubeSquared / 2 * coordinates.R);
        var y = hexSize * (1.5F * coordinates.R);
        return new Vector2f(x, y);
    } 
}