using System;
using System.Threading;
using SFML.System;

namespace Flocking;

public static class AgentIdGenerator {
    private static int _idCounter = 0;
    public static int GetNextId()
    {
        return Interlocked.Increment(ref _idCounter);
    }
}

public class Agent
{
    public int Id { get; } = AgentIdGenerator.GetNextId();
    
    public float Speed { get; set; } = 150f; // 20px pr sec
    public float Size { get; set; } = 12f;
    public Vector2f Pos { get; set; }
    public Vector2f Acceleration { get; set; }
    public Vector2f Velocity { get; set; }
    public float NeighborhoodRadius { get; set; } = 72f;
}