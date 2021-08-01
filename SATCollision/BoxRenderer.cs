﻿using System;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace SATCollision
{
    public class BoxRenderer
    {
        private readonly RenderTarget _renderTarget;
        private readonly ConvexShape _shape;
        
        public BoxRenderer(RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
            _shape = new ConvexShape(4);
            _shape.SetPoint(0, new Vector2f(-.5f, -.5f));
            _shape.SetPoint(1, new Vector2f(.5f, -.5f));
            _shape.SetPoint(2, new Vector2f(.5f, .5f));
            _shape.SetPoint(3, new Vector2f(-.5f, .5f));
        }
        
        public void Draw(Box box)
        {
            var scale = new Vector2f(box.Width, box.Height);
            var color = box.Intersected ? Color.Red : Color.Green;
            
            _shape.Position = box.Position;
            _shape.Rotation = box.Rotation;
            _shape.Scale = scale;
            _shape.OutlineColor = color;
            _shape.FillColor = Color.Transparent;
            _shape.OutlineThickness = 1f / scale.X;
            _renderTarget.Draw(_shape);

            var point = _shape.GetPoint(0);
            point = RotateVector(point, new Vector2f(0f, 0f), (float)MathUtils.DegreeToRadian(box.Rotation));
            var first = point.Multiply(scale) + box.Position;
            
            _renderTarget.Draw(new []
            {
                new Vertex(box.Position, color),
                new Vertex(first, color)
            }, 0, 2, PrimitiveType.Lines);


            var points = box.GetPoints();
            for(var i = 0; i < points.Length; i++)
            {
                var p1 = points[i];
                var p2 = points[(i + 1) % points.Length];
                
                _renderTarget.Draw(new[]
                {
                    new Vertex(p1, Color.Yellow),
                    new Vertex(p2, Color.Yellow)
                },0, 2, PrimitiveType.Lines);
            }
        }

        private Vector2f RotateVector(Vector2f vec, Vector2f origin, float radians)
        {
            var d = vec;

            return new Vector2f(
                (float)(d.X * Math.Cos(radians) - d.Y * Math.Sin(radians)),
                (float)(d.X * Math.Sin(radians) + d.Y * Math.Cos(radians))
            );
        }
    }
}