using SFML.System;

namespace Flocking;

public class Agent
{
    public float Speed { get; set; } = 150f; // 20px pr sec
    public float Size { get; set; } = 12f;
    public Vector2f Pos { get; set; }
    public Vector2f Acceleration { get; set; }
    public Vector2f Velocity { get; set; }
    public float NeighborhoodRadius { get; set; } = 72f;
}