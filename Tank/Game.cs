using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace TankGame
{
    public class Game : GameBase
    {
        private TankController _tankController;
        private Tank _tank;
        private readonly TankRenderer _tankRenderer;
        
        public Game(RenderWindow window) : base(window)
        {
            _tankRenderer = new TankRenderer(window);
            _tankController = new TankController(this);
        }

        public override void Initialize()
        {
            ClearColor = new Color(0xeaeaeaff);
            
            _tank = new Tank {
                Position = WindowCenter,
                EngineStrength = 2f,
                Mass = 1,
                BodySteeringStrength = 180, // 180 degrees each sec
                Barrel = { Angle = 45 },
                Body = { Angle = -45 }
            };
        }

        protected override void Update()
        {
            _tankController.BarrelLookAtMousePosition(_tank);
            
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                _tankController.RotateBodyAntiClockWise(_tank);
            
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                _tankController.RotateBodyClockWise(_tank);
            
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                _tankController.MoveForward(_tank);
            
            if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                _tankController.Reset(_tank);

            _tankController.ApplyFriction(_tank);
            _tankController.Update(_tank);
        }

        protected override void Render()
        {
            _tankRenderer.Render(_tank);
        }
    }
}