using System;
using System.Linq;
using SFML.System;
using Stho.SFML.Extensions;

namespace SATCollision;

public class BoxController
{
    public void Rotate(Box box, float angle)
    {
            box.Rotation += angle;
        }

    public bool SatCollision(Box box1, Box box2)
    {
            return SatCollisionResolver.Intersects(
                box1.GetPoints(), 
                box2.GetPoints()
            );
        }
}