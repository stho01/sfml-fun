using System;
using System.Collections;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SATCollision
{
    public class Polygon : Shape, IEnumerable<Vector2f>
    {
        public List<Vector2f> Vertices { get; } = new();
        
        public override uint GetPointCount() => (uint)Vertices.Count;

        public override Vector2f GetPoint(uint index)
        {
            if (index >= GetPointCount())
                throw new ArgumentOutOfRangeException();
            
            return Vertices[(int)index];
        }

        public void Add(float x, float y) => Add(new Vector2f(x, y));
        public void Add(Vector2f vec) => Vertices.Add(vec); 
        
        public IEnumerator<Vector2f> GetEnumerator() => Vertices.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}