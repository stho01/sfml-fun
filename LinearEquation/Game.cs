using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace LinearEquation
{
    public enum IntersectionMode
    {
        X,
        Y
    }
    
    public class Game : GameBase
    {
        //**********************************************************
        //** fields:
        //**********************************************************

        private FloatLine _lineShape;
        private readonly CircleShape _intersection = new CircleShape();
          
        //**********************************************************
        //** ctor:
        //**********************************************************
        
        public Game(RenderWindow window) : base(window)
        {
            Window.KeyPressed += (sender, args) =>
            {
                Console.WriteLine("Pressed");
                var displacement = _lineShape.P2 - _lineShape.P1;
                var dir = displacement.Normalize();
                var dirNew = new Vector2f(-dir.Y, dir.X);

                var length = displacement.Length();
                var center = displacement / 2;

                _lineShape.P2 = dirNew * length;

            };
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public Vector2f Intersection { get; set; }
        public IntersectionMode IntersectionMode { get; set; }
          
        //**********************************************************
        //** methods:
        //**********************************************************

        public override void Initialize()
        {
            GenerateLine();
            
            _intersection.Radius = 5f;
            _intersection.Origin = new Vector2f(_intersection.Radius, _intersection.Radius);
            _intersection.FillColor = Color.Red;


           
        }

        public void GenerateLine()
        {
            _lineShape = new FloatLine(100, Window.Size.Y * .7f, 1100, Window.Size.Y * .3f);
        }

        protected override void Update()
        {
            var mousePos = GetMousePosition();
            
            if (IntersectionMode == IntersectionMode.X)
            {
                var helper = LinearEquations.GetSlopeInterceptForm(_lineShape);
                Intersection = helper.PointOnLineX(mousePos.X);
            } 
            else if (IntersectionMode == IntersectionMode.Y)
            {
                var helper = LinearEquations.GetSlopeInterceptForm(_lineShape);
                Intersection = helper.PointOnLineY(mousePos.Y);
            }
        }

        protected override void Render()
        {
            Window.Draw(new []
            {
                new Vertex(_lineShape.P1), 
                new Vertex(_lineShape.P2), 
            }, 0, 2, PrimitiveType.Lines);

            _intersection.Position = Intersection;
            Window.Draw(_intersection);
        }
    }
}