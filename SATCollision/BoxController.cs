using System;
using System.Linq;
using SFML.System;
using Stho.SFML.Extensions;

namespace SATCollision
{
    public class BoxController
    {
        public void Rotate(Box box, float angle)
        {
            box.Rotation += angle;
        }

        public bool SatCollision(Box box1, Box box2)
        {
            bool PrimaryCollisionCheck(Box p, Box q)
            {
                var pPoints = p.GetPoints();
                var qPoints = q.GetPoints();
            
                for (var i = 0; i < pPoints.Length; i++)
                {
                    var p1 = pPoints[i];
                    var p2 = pPoints[(i + 1) % pPoints.Length];
                    var direction = (p2 - p1);
                    var normal = new Vector2f(-direction.Y, direction.X);

                    var (pMin, pMax) = GetMaxMin(normal, pPoints);
                    var (qMin, qMax) = GetMaxMin(normal, qPoints);

                    if (!(pMax > qMin || qMax < pMin)) 
                        return false;
                }

                return true;
            }
            
            (float min, float max) GetMaxMin(Vector2f normal, Vector2f[] points)
            {
                var max = float.MinValue;
                var min = float.MaxValue;

                foreach (var point in points)
                {
                    max = Math.Max(point.Dot(normal), max);
                    min = Math.Min(point.Dot(normal), min);
                }

                return (min, max);
            }
            
            return PrimaryCollisionCheck(box1, box2) 
                || PrimaryCollisionCheck(box2, box1);
        }
    }
}