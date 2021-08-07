using SFML.System;

namespace Chains
{
    public class Segment
    {
        public Segment(int length)
        {
            Length = length;
        }
        
        public int Length { get; }
        public Vector2f P1 { get; set; }
        public Vector2f P2 { get; set; }
        public Segment Child { get; set; }
    }
}