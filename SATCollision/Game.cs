using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace SATCollision
{
    public class Game : GameBase
    {
        private readonly BoxRenderer _boxRenderer;
        private readonly BoxController _boxController;
        private readonly Box _box1 = new(100, 100);
        private readonly Box _box2 = new(50, 50);
        
        // private readonly CircleShape _triangle = new CircleShape(10, 3);
        
        public Game(RenderWindow window) : base(window)
        {
            _boxRenderer = new BoxRenderer(window);
            _boxController = new BoxController();
        }

        public override void Initialize()
        {
            ShowFps = true;
            _box1.Position = new Vector2f(WindowWidth/2, WindowHeight/2);
            _box2.Position = new Vector2f(100, 100);

            // for (uint i = 0; i < _triangle.GetPointCount(); i++)
            //     Console.WriteLine(_triangle.GetPoint(i));
            // _triangle.Position = new Vector2f(100, 100);
            // for (uint i = 0; i < _triangle.GetPointCount(); i++)
            //     Console.WriteLine(_triangle.GetPoint(i));
        }

        protected override void Update()
        {
            var mousePos = GetMousePosition();
            _box2.Position = new Vector2f(mousePos.X, mousePos.Y);

            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) _box2.Rotation += 360 * Timer.DeltaTimeSeconds; 
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) _box2.Rotation -= 360 * Timer.DeltaTimeSeconds;
            
            _boxController.Rotate(_box1, 10 * Timer.DeltaTimeSeconds);
            _box2.Intersected = _boxController.SatCollision(_box1, _box2);
        }

        protected override void Render()
        {
            _boxRenderer.Draw(_box1);
            _boxRenderer.Draw(_box2);
        }
    }
}