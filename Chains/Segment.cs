using SFML.System;

namespace Chains;

public class Segment(int length)
{
    public int Length { get; } = length;
    public Vector2f P1 { get; set; }
    public Vector2f P2 { get; set; }
    public Segment Child { get; set; }
}