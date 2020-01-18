using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace AngularRotation
{
    public class Game : GameBase
    {
        private readonly RectangleShape _shape = new RectangleShape();
        private float _angularAcceleration = 0f;
        private float _angularVelocity = 0f;
        private float _angle = 0;
        
        private bool _applyingForce = false;
        private Vector2f? _pointOnShape = null;
        
        public Game(RenderWindow window) : base(window) { }

        public override void Initialize()
        {
            _shape.Size = new Vector2f(150, 150);
            _shape.Position = WindowCenter;
            _shape.OutlineThickness = -1;
            _shape.OutlineColor = Color.Magenta;
            _shape.Origin = _shape.Size * .5f;
        }

        protected override void Update()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var floatRect = new FloatRect(_shape.Position, _shape.Size);
                var mousePos = (Vector2f) GetMousePosition();
                _pointOnShape ??= mousePos;
                
                if (floatRect.Contains(mousePos.X, mousePos.Y))
                {
                    
                    
                    // var distanceFromCenter = _pointOnShape - _shape.Position;
                    // var direction = (mousePos - _pointOnShape)?.ToPolarCoordinates();
                    // _angularAcceleration = direction?.Angle
                    
                }
            }
            else
            {
                _pointOnShape = null;
            }
            
            
            _angularVelocity += _angularAcceleration;
            _angle += _angularVelocity * Timer.DeltaTimeSeconds;
            _shape.Rotation = _angle;
            _angularAcceleration = 0f;
        }

        protected override void Render()
        {
            Window.Draw(_shape);
        }
    }
}