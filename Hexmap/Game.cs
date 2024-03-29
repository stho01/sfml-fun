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
    public const int HexagonSize = 35;
    public const int GridRadius = 10;

    private readonly HexagonShape _hexagon = new(50) {
        FillColor = Theme.DefaultFill,
        OutlineColor = Theme.DefaultFill,
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
        ClearColor = Color.White;
        _stateMachine.AddState(new DrawingCirclesState());
        _stateMachine.AddState(new DrawingLinesState());
        _stateMachine.Load<DrawingLinesState>();
        
        
        var center = CubeCoordinate.Zero;
        _hexagons.Add(new Hexagon { Size = HexagonSize, Coordinates = center });

        var range = 
            CubeCoordinate
                .GetRange(GridRadius)
                .Select(c => new Hexagon { Size = HexagonSize, Coordinates = c });
        
        _hexagons.AddRange(range);
    }

    protected override void Update()
    {
        Hovered = null;
        var hovered = Hexagon.GetCoordinates(HexagonSize, GetMousePosition() - (Vector2i)WindowCenter);
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
            _hexagon.Position = hexagon.Position + WindowCenter;
            _hexagon.Size = hexagon.Size;
            _hexagon.FillColor = hexagon.Coordinates == CubeCoordinate.Zero ? Theme.MagentaFill : Theme.DefaultFill;
            _hexagon.OutlineColor = Theme.DefaultOutline;
            _hexagon.OutlineThickness = 1;
            Window.Draw(_hexagon);
        }

        _stateMachine.Draw(Window);
        
        if (Hovered.HasValue)
        {
            _hexagon.Position = Hexagon.GetPosition(HexagonSize, Hovered.Value) + WindowCenter;
            _hexagon.Size = HexagonSize;
            _hexagon.FillColor = Theme.GreenFill;
            Window.Draw(_hexagon);
        }
    }
}