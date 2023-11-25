using System;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Pendulum;
// public class Arm
// {
//     
// }
//
// public class Bob
// {
//     
// }

public class Game : GameBase
{
    private Vector2f _origin;
    private float _armLength;
    private float _angle;
    private float _angularVelocity;
    private float _angularAcceleration;
    private float _bobMass;
        
    private Vector2f _bobPosition;
        
    public Game(RenderWindow window) : base(window)
    {
            
        }

    public override void Initialize()
    {
            _origin = new Vector2f(WindowWidth/2, 0);
            _armLength = 300f;
            _angle = (float)(Math.PI / 4);
            _angularAcceleration = 0f;
            _angularVelocity = 0f;
            _bobPosition = new Vector2f();
            _bobMass = 50;
        }

    protected override void Update()
    {
            _angularAcceleration = (float)(-0.01f * Math.Sin(_angle)); 
            _angularVelocity += _angularAcceleration;
            _angle += _angularVelocity * Timer.DeltaTimeSeconds;
            
            var bobX = (float) (_origin.X + _armLength * Math.Sin(_angle));
            var bobY = (float) (_origin.Y + _armLength * Math.Cos(_angle));
            
            _bobPosition = new Vector2f(bobX, bobY);
        }

    protected override void Render()
    {
            Window.Draw(new []
            {
                new Vertex(_origin),
                new Vertex(_bobPosition), 
            }, 0, 2, PrimitiveType.Lines);

            var bob = new CircleShape(30)
            {
                Position = _bobPosition,
                FillColor = Color.Green,
                OutlineColor = Color.White,
                OutlineThickness = -1,
                Origin = new Vector2f(30, 30)
            };
            Window.Draw(bob);
        }
}