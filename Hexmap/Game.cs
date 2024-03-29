using System;
using System.Collections.Generic;
using System.Linq;
using Hexmap.States;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;
using Color = SFML.Graphics.Color;

namespace Hexmap;

public class Game : GameBase
{
    public const int HexSize = 35;
    public const int GridRadius = 10;
    private readonly HexShape _hex = new(50) {
        FillColor = Color.Transparent,
        OutlineColor = Color.Red,
        OutlineThickness = 1
    };
    private readonly List<Hexagon> _hexagons = [];
    private readonly StateMachine _stateMachine;

    public Game(RenderWindow window) : base(window)
    {
        _stateMachine = new StateMachine(this);
    }
    
    public CubeCoordinate? Hovered { get; private set; }
    
    public override void Initialize()
    {
        _stateMachine.AddState(new DrawingCirclesState());
        _stateMachine.AddState(new DrawingLinesState());
        _stateMachine.Load<DrawingLinesState>();
        
        
        var center = CubeCoordinate.Zero;
        _hexagons.Add(new Hexagon { Size = HexSize, Coordinates = center });

        var range = 
            CubeCoordinate
                .GetRange(CubeCoordinate.Zero, GridRadius)
                .Select(c => new Hexagon { Size = HexSize, Coordinates = c });
        
        _hexagons.AddRange(range);
    }

    protected override void Update()
    {
        Hovered = null;
        var hovered = Hexagon.GetCoordinates(HexSize, GetMousePosition() - (Vector2i)WindowCenter);
        var distance = (int)CubeCoordinate.Distance(CubeCoordinate.Zero, hovered);

        if (distance <= GridRadius)
            Hovered = hovered.Round();
        
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.F1))
            _stateMachine.Load<DrawingCirclesState>();
        else if(Keyboard.IsKeyPressed(Keyboard.Key.F2))
            _stateMachine.Load<DrawingLinesState>();
        
        _stateMachine.Update();
    }

    protected override void Render()
    {
        foreach (var hexagon in _hexagons)
        {
            _hex.Position = hexagon.Position + WindowCenter;
            _hex.Size = hexagon.Size;
            _hex.FillColor = hexagon.Coordinates == CubeCoordinate.Zero ? Color.Magenta : Color.Transparent;
            Window.Draw(_hex);
        }

        _stateMachine.Draw(Window);
        
        if (Hovered.HasValue)
        {
            _hex.Position = Hexagon.GetPosition(HexSize, Hovered.Value) + WindowCenter;
            _hex.Size = HexSize;
            _hex.FillColor = Color.Green;
            Window.Draw(_hex);
        }
    }
}