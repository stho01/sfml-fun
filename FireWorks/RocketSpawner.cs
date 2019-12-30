using System;
using SFML.System;
using Stho.SFML.Extensions;

namespace FireWorks
{
    public class RocketSpawner
    {
        //**********************************************************
        //** fields:
        //**********************************************************
        
        private readonly Game _game;

        //**********************************************************
        //** ctor:
        //**********************************************************
                
        public RocketSpawner(Game game)
        {
            _game = game;
        }

        //**********************************************************
        //** props:
        //**********************************************************

        public Range StrengthRange { get; set; } = new Range(22, 27);
        public Range MassRange { get; set; } = new Range(10, 15);
        public Range FuelRange { get; set; } = new Range(50, 75);
        public Range TotalLifeTime { get; set; } = new Range(500, 1500);

        //**********************************************************
        //** methods:
        //**********************************************************

        public void SpawnOnEarthSurface()
        {
            var angle = MathUtils.DegreeToRadian(RandomNumber.Get(0, 360));
            var dir = new Vector2f(
                (float) Math.Cos(angle),
                (float) Math.Sin(angle)
            );
            var position = _game.Earth.Position + dir * (_game.Earth.Radius + 10);

            // Just to make it more interesting. 
            var randomAngle = angle + RandomNumber.GetFloat(-1, 1) * Math.PI * .13f;
            var randomDir = new Vector2f(
                (float) Math.Cos(randomAngle),
                (float) Math.Sin(randomAngle)
            );

            var rocket = new Rocket(RandomNumber.Get(StrengthRange))
            {
                Position = position,
                Direction = randomDir,
                Mass = RandomNumber.Get(MassRange),
                Fuel = RandomNumber.Get(FuelRange),
                TotalLifetime = RandomNumber.Get(TotalLifeTime)
            }; 
            
            _game.AddRocket(rocket);
        }
    }
}