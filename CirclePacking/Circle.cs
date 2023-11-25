using SFML.System;

namespace CirclePacking;

public class Circle
{
    public float Radius { get; set; }
    public bool Growing { get; set; } = true;
    public Vector2f Position { get; set; }
}