using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Stho.SFML.Extensions;

namespace Springs;

public class Game(RenderWindow window) 
    : GameBase(window)
{
    private readonly Spring _spring = new();
    private readonly Bob _bob = new();
    
    public override void Initialize()
    {
        _spring.Anchor = new Vector2f(WindowWidth/ 2f, 100f);
        _spring.Length = 190f;
        _spring.K = 60f;
        _bob.Position = new Vector2f(WindowWidth / 2f, 200f);
        _bob.Shape.Radius = 30f;
    }

    protected override void Update()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            _bob.ApplyForce(5000f, 0f);
        
        _bob.ApplyForce(0, 5000f);
        
        _spring.Connect(_bob);
        _bob.Update();

        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            var pos = Mouse.GetPosition(Window);
            if (_bob.Shape.GetGlobalBounds().Contains(pos.X, pos.Y))
            {
                _bob.Position = (Vector2f)pos;
            }
        }
    }

    protected override void Render()
    {
        _spring.Render(window);
        _bob.Render(Window);
    }
}