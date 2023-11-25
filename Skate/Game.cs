using System.Threading;
using SFML.Graphics;
using SFML.System;
using Stho.SFML.Extensions;

namespace Skate;

public class Game : GameBase
{
    //**********************************************************
    //** fields:
    //**********************************************************

    private readonly Skater _skater = new Skater();
    private readonly SkaterUpdater _skaterUpdater;
    private readonly SkaterRenderer _skaterRenderer;
          
    //**********************************************************
    //** ctor:
    //**********************************************************
        
    public Game(RenderWindow window) : base(window)
    {
            _skaterUpdater = new SkaterUpdater(this);   
            _skaterRenderer = new SkaterRenderer(Window);
        }
      
    //**********************************************************
    //** props:
    //**********************************************************

    public Skater Skater => _skater;
    public float WorldFriction { get; set; } = 0.97f; // range between 0 - 1.
        
    //**********************************************************
    //** methods:
    //**********************************************************

    public override void Initialize()
    {
            _skater.Position = WindowCenter;
            _skater.Mass = 10;
            _skater.Strength = 1;
        }

    protected override void Update()
    {
            _skaterUpdater.Update(_skater);
        }

    protected override void Render()
    {
            _skaterRenderer.Render(_skater);
        }
}