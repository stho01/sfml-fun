using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hexmap;

public sealed class HexagonRange : IEnumerable<CubeCoordinate>
{
    private List<CubeCoordinate> _range = new();
    private CubeCoordinate _center = CubeCoordinate.Zero;
    
    public static HexagonRange Create(int n)
    {
        var instance = new HexagonRange(n){
            _range = CubeCoordinate.GetRange(n).ToList()
        };
        return instance;
    }

    private HexagonRange(int n)
    {
        N = n;
    }
    
    public int N { get; }

    public CubeCoordinate CenterCoordinates
    {
        get => _center;
        set
        {
            var delta = (_center - value);
            Translate(-delta);
            _center = value;
        }
    }

    public void Translate(CubeCoordinate coordinate)
    {
        for (var i = 0; i < _range.Count; i++)
            _range[i] += coordinate;
    }

    public static IEnumerable<CubeCoordinate> GetIntersection(HexagonRange a, HexagonRange b)
    {
        return 
            from h1 in a 
            from h2 in b 
            where h1 == h2 
            select h1;
    }

    public IEnumerator<CubeCoordinate> GetEnumerator() => _range.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}