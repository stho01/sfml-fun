using SFML.System;

namespace BallCollision;

public class Ball
{
    public float Mass { get; set; }
    public Vector2f Acceleration { get; set; }
    public Vector2f Velocity { get; set; }
    public Vector2f Position { get; set; }
    public float Size => Mass * 2;
    public float Radius => Mass;
    public bool Selected { get; set; }

    public void ApplyForce(Vector2f force)
    {
        Acceleration += force / Mass;
    }

    public void ResetAcceleration()
    {
        Acceleration = new Vector2f();
    }

    public void Select() => Selected = true;
    public void Deselect() => Selected = false;
}