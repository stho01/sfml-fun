using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Springs;

public class Bob
{
    public Vector2f Position { get; set; }
    public Vector2f Acceleration { get; set; }
    public Vector2f Velocity { get; set; }
    public CircleShape Shape { get; set; }
    public float Dampening { get; set; } = .999f;

    public Bob()
    {
        Shape = new CircleShape(10f)
        {
            Position = Position, 
            Origin = new Vector2f(10f, 10f)
        };
    }
    
    public void ApplyForce(float x, float y)
    {
        Acceleration += new Vector2f(x, y);
    }
    public void ApplyForce(Vector2f force)
    {
        Acceleration += force;
    }

    public void Update()
    {
        Velocity += Acceleration * (float)Timer.DeltaTimeSeconds;
        Velocity *= Dampening;
        Position += Velocity * (float)Timer.DeltaTimeSeconds;
        Acceleration *= 0;
    }
    
    public void Render(RenderWindow window)
    {
        Shape.Position = Position;
        Shape.Origin = new Vector2f(Shape.Radius, Shape.Radius);
        
        window.Draw(Shape);
    }
}