﻿using SFML.System;

namespace FireWorks
{
    public class Rocket
    {
        //**********************************************************
        //** ctor:
        //**********************************************************

        public Rocket(float strength = 100f)
        {
            Strength = strength;
        }

        //**********************************************************
        //** props:
        //**********************************************************

        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; set; }
        public Vector2f Acceleration { get; set; }
        public Vector2f Direction { get; set; }
        public float Fuel { get; set; } = 100f;
        public bool HasFuel => Fuel > 0;
        public float Mass { get; set; }
        public float Strength { get; }
        public bool Done { get; set; }
        
        public float Age { get; set; }
        public float TotalLifeTime { get; set; }
    }
}