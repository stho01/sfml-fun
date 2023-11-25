using System.Linq;
using SFML.System;

namespace Fireworks;

public class Explosion(int density)
{
    //**********************************************************
    //** ctor:
    //**********************************************************

    //**********************************************************
    //** props:
    //**********************************************************

    public float Strength { get; set; } = 1f;
    public Vector2f Position { get; set; }
    public Particle[] Particles { get; } = new Particle[density];
    public bool Done => Particles.All(p => p.IsDead);

    //**********************************************************
    //** methods:
    //**********************************************************
}