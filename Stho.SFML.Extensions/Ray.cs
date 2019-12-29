using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace Stho.SFML.Extensions
{
    public class Ray
    {
        public Vector2f Position { get; set; }
        public Vector2f Direction { get; set; }
        
        public Vector2f? Cast(Shape[] shapes)
        {
            var p1 = Position;
            var p2 = Position + (Direction * 100_000_000f);
            var ray = new FloatLine(p1, p2);

            var intersectionPoints 
                = shapes.SelectMany(shape => IntersectionPoints(ray, shape));

            if (!intersectionPoints.Any())
                return null;
            
            return intersectionPoints
                .Select(point => (point, (point - Position).SqrLength()))
                .Aggregate((shortest, x) => x.Item2 < shortest.Item2 ? x : shortest).point;
        }

        private Vector2f[] IntersectionPoints(FloatLine ray, Shape shape)
        {
            switch (shape)
            {
                case LineShape line:
                    return IntersectionPoints(ray, line);
                case CircleShape circle:
                    return IntersectionPoints(ray, circle);
                case RectangleShape rect:
                    return IntersectionPoints(ray, rect);
                
                default:
                    throw new NotImplementedException("Shape intersection not implemented");
            }
        }

        private Vector2f[] IntersectionPoints(FloatLine ray, LineShape shape)
        {
            var line = new FloatLine(shape.P1, shape.P2);
            var intersectionPoint = ray.Intersects(line);

            return intersectionPoint.HasValue ? new[] {intersectionPoint.Value} : new Vector2f[0];
        }
        
        private Vector2f[] IntersectionPoints(FloatLine ray, CircleShape shape)
        {
            var circle = new FloatCircle(shape.Position, shape.Radius);

            var circleDelta = (shape.Position - Position);
            var product = Direction.Dot(circleDelta);
            
            if (product <= 0) // if ray is facing in the opposite direction of the circle
                return Array.Empty<Vector2f>();

            return ray.Intersects(circle);
        }


        private Vector2f[] IntersectionPoints(FloatLine ray, RectangleShape shape)
        {
            var l1 = new FloatLine(shape.Position, shape.Position + new Vector2f(shape.Size.X, 0));
            var l2 = new FloatLine(shape.Position, shape.Position + new Vector2f(0, shape.Size.Y));
            var l3 = new FloatLine(
                shape.Position.X,
                shape.Position.Y + shape.Size.Y,
                shape.Position.X + shape.Size.X,
                shape.Position.Y + shape.Size.Y
            );
            var l4 = new FloatLine(
                shape.Position.X + shape.Size.X, 
                shape.Position.Y,
                shape.Position.X + shape.Size.X,
                shape.Position.Y + shape.Size.Y
            );

            var intersections = new List<Vector2f?>
            {
                ray.Intersects(l1),
                ray.Intersects(l2),
                ray.Intersects(l3),
                ray.Intersects(l4),
            };

            return intersections
                .Where(x => x != null)
                .Cast<Vector2f>()
                .ToArray();
        }
    }
}