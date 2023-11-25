using Stho.SFML.Extensions;

namespace QTree;

public static class QuadTreeExtensions
{
    public static bool Insert(this QuadTree<Particle> quadTree, Particle particle)
    {
            return quadTree.Insert(particle.Position, particle);
        }
}