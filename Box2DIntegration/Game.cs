using System;
using System.Collections.Generic;
using System.Numerics;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;
using Color = SFML.Graphics.Color;
using Timer = Stho.SFML.Extensions.Timer;

namespace PhysixSimulation;

public class Game(RenderWindow window) : GameBase(window)
{
    private readonly GameFpsRenderer _gameFpsRenderer = new(window);
    private readonly World _world = new(new Vector2(0f, -10f));
    private readonly List<Entity> _entities = [];
    private Vector2f _cameraPosition = new(0, 0);
    private const float Scale = 10f;
    private readonly Random _random = new(DateTime.Now.Millisecond);

    public override void Initialize()
    {
        _cameraPosition = new Vector2f(WindowWidth / 2f,WindowHeight / 2f);
        _gameFpsRenderer.ShowFps = true;
            
        var ground = CreateEntity(0,-20f, 100f,2f, BodyType.Static);
        ground.Shape.FillColor = Color.White;
        ground.Shape.OutlineThickness = 1;
        ground.Shape.OutlineColor = Color.Cyan;
        _entities.Add(ground);
            
            
        _entities.Add(CreateEntity(0f,0f, 6f,6f, BodyType.Dynamic));
        _entities.Add(CreateEntity(-20f,10f, 10f,15f, BodyType.Dynamic));
        _entities.Add(CreateEntity(20f,10f, 10f,10f, BodyType.Dynamic));
            
        Timer.SetInterval(16, () => _world.Step(0.016666668F, 8, 3));
    }

    private Entity CreateEntity(float x, float y, float w, float h, BodyType type)
    {
        var def = new BodyDef {
            type = type,
            position = new Vector2(x, y)
        };
        var body = _world.CreateBody(def);
            
        var physicsShape = new PolygonShape();
        physicsShape.SetAsBox(w*.5f, h*.5f);
            
        var fixture = new FixtureDef {
            shape = physicsShape,
            density = (float)_random.NextDouble() * 5.0f + 1f,
            friction = .3f
        };
            
        body.CreateFixture(fixture);

        var shapeSize =
            new Vector2f(
                w * Scale,
                h * Scale);
            
        var renderShape = new RectangleShape(shapeSize) {
            FillColor = Color.Green
        };

        return new Entity
        {
            Body = body,
            Shape = renderShape
        };
    }
        
    protected override void Update()
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            var worldPos = ((Vector2f)Mouse.GetPosition(Window) - _cameraPosition) / Scale;
            var entity = CreateEntity(worldPos.X, -worldPos.Y, 1f, 1f, BodyType.Dynamic);
            _entities.Add(entity);
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.A)) _cameraPosition.X -= 1f;
        if (Keyboard.IsKeyPressed(Keyboard.Key.D)) _cameraPosition.X += 1f;
        if (Keyboard.IsKeyPressed(Keyboard.Key.S)) _cameraPosition.Y += 1f;
        if (Keyboard.IsKeyPressed(Keyboard.Key.W)) _cameraPosition.Y -= 1f;
    }

    protected override void Render()
    {
        _gameFpsRenderer.Render();

        foreach(var entity in _entities)
        {
            var pos = entity.Body.GetPosition() * Scale;
                
            entity.Shape.Position = new Vector2f(pos.X, -pos.Y) + _cameraPosition;
            entity.Shape.Origin = entity.Shape.Size * .5f;
            entity.Shape.Rotation = -entity.Body.GetAngle() * (180f / (float)Math.PI);
                
                
            Window.Draw(entity.Shape);
        }

            
    }
}

public class Entity
{
    public Body Body { get; set; }
    public RectangleShape Shape { get; set; }
}