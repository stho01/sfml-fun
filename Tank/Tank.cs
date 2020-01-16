using System;
using SFML.System;

namespace TankGame
{
    public class TankBody
    {
        public float Angle { get; set; }   
        public float AngularVelocity { get; set; }
        public float AngularAcceleration { get; set; }
        public float Mass { get; set; } = 1f;
        
        public void ApplyAngularForce(float force)
        {
            AngularAcceleration += force / Mass;
        }
    }

    public class TankBarrel
    {
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public float AngularAcceleration { get; set; }
        public float Mass { get; set; }
        
        public void ApplyAngularForce(float force)
        {
            AngularAcceleration += force / Mass;
        }
    }
    
    public class Tank
    {
        //**********************************************************
        //** ctors:
        //**********************************************************

        public Tank()
        {
            Barrel = new TankBarrel();
            Body = new TankBody();
        }
          
        //**********************************************************
        //** props:
        //**********************************************************

        public float BodySteeringStrength { get; set; } = 1f;
        public float EngineStrength { get; set; } = 1f;
        public Vector2f Velocity { get; set; }
        public Vector2f Position { get; set; }
        public Vector2f Acceleration { get; set; }
        public float Mass { get; set; } = 1f;
        public TankBody Body { get; }
        public TankBarrel Barrel { get; }
    }
}