using System;
using System.Collections.Generic;

namespace Hexmap;

public readonly record struct CubeCoordinate(float Q, float R, float S)
{
    public static readonly CubeCoordinate Zero = new();
    
    public static readonly CubeCoordinate East = (1, 0, -1);
    public static readonly CubeCoordinate SouthEast = (0, 1, -1);
    public static readonly CubeCoordinate SouthWest = (-1, 1, 0);
    public static readonly CubeCoordinate West = (-1, 0, 1);
    public static readonly CubeCoordinate NorthWest = (0, -1, 1);
    public static readonly CubeCoordinate NorthEast = (1, -1, 0);
    

    public CubeCoordinate GetNeighbor(Direction direction)
    {
        return direction switch {
            Direction.East => this + East,
            Direction.SouthEast => this + SouthEast,
            Direction.SouthWest => this + SouthWest,
            Direction.West => this + West,
            Direction.NorthWest => this + NorthWest,
            Direction.NorthEast => this + NorthEast,
        };
    }

    public CubeCoordinate Round() => Round(this);
    
    
    public static float Distance(CubeCoordinate a, CubeCoordinate b)
    {
        var d = b - a;
        var max = MathF.Abs(d.Q);
        max = MathF.Max(max, MathF.Abs(d.R));
        max = MathF.Max(max, MathF.Abs(d.S));
        return max;
    }
    
    public static IEnumerable<CubeCoordinate> GetRange(CubeCoordinate center, int n)
    {
        for (var q = -n; q <= n; q++)
        for (var r = Math.Max(-n, -q-n); r <= Math.Min(n, -q+n); r++)
        {
            var s = -q - r;
            var coords = center + new CubeCoordinate(q, r, s);
            yield return coords;
        }
    }
    
    public static IEnumerable<CubeCoordinate> GetRing(CubeCoordinate center, int radius) 
    {
        var cursor = center + NorthWest * radius;
        yield return cursor;
        
        for (var i = 0; i < 6; i++)
        for (var j = 0; j < radius; j++)
        {
            cursor = cursor.GetNeighbor((Direction)i);
            yield return cursor;
        }
    }

    public static CubeCoordinate Round(CubeCoordinate coords)
    {
        var q = (int)coords.Q;
        var r = (int)coords.R;
        var s = (int)coords.S;

        var qDiff = MathF.Abs(q - coords.Q);
        var rDiff = MathF.Abs(r - coords.R);
        var sDiff = MathF.Abs(s - coords.S);

        if (qDiff > rDiff && qDiff > sDiff)
            q = -r - s;
        else if (rDiff > sDiff)
            r = -q - s;
        else
            s = -q - r;

        return (q, r, s);
    }

    public static implicit operator CubeCoordinate((float, float, float) values) 
        => new(values.Item1, values.Item2, values.Item3);

    public static CubeCoordinate operator +(CubeCoordinate a, CubeCoordinate b) =>
        new(a.Q + b.Q,
            a.R + b.R,
            a.S + b.S);

    public static CubeCoordinate operator -(CubeCoordinate a, CubeCoordinate b) =>
        new(a.Q - b.Q,
            a.R - b.R,
            a.S - b.S);

    public static CubeCoordinate operator *(CubeCoordinate a, CubeCoordinate b) =>
        new(a.Q * b.Q,
            a.R * b.R,
            a.S * b.S);
    
    public static CubeCoordinate operator *(CubeCoordinate a, float b) =>
        new(a.Q * b,
            a.R * b,
            a.S * b);
    public static CubeCoordinate operator *(float a, CubeCoordinate b) => b * a;
    
    public enum Direction
    {
        East = 0,
        SouthEast,
        SouthWest,
        West,
        NorthWest,
        NorthEast
    }
}