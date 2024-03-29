using System;
using SFML.Graphics;
using SFML.System;

namespace Hexmap;

public class HexagonShape : Shape
{
    private const int PointCount = 6;
    private readonly Vector2f[] _vertices = new Vector2f[PointCount];
    private int _size;
    
    public HexagonShape(int size)
    {
        Size = size;
    }

    public int Size
    {
        get => _size;
        set
        {
            _size = value;
            CreateVertices();
        }
    }

    public override uint GetPointCount() => PointCount;
    public override Vector2f GetPoint(uint index) => _vertices[index];

    private void CreateVertices()
    {
        const float hexAngle = 2 * MathF.PI / PointCount;
        const float halfHexAngle = hexAngle / 2;
        
        for (var i = 0; i < PointCount; i++)
        {
            var angle =  i * hexAngle - halfHexAngle;
            var x = MathF.Cos(angle) * _size;
            var y = MathF.Sin(angle) * _size;
            _vertices[i] = new Vector2f(x, y);
        }
        
        Update();
    }
}