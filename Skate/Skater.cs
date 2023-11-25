using SFML.System;

namespace Skate;

public class Skater
{
    public Vector2f Acceleration { get; set; }
    public Vector2f Velocity { get; set; }
    public Vector2f Position { get; set; }
    public float Angle { get; set; }
    public float Mass { get; set; }
    public float Strength { get; set; }
}