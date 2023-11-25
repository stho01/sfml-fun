using LuaScripting.Models;
using SFML.Graphics;
using SFML.Window;
using Stho.SFML.Extensions;

namespace LuaScripting;

public class Game : GameBase
{
    private readonly GameObject _player = new GameObject() { Tag = "Player" };
    private readonly Script _script;
        
    public Game(RenderWindow window) : base(window)
    {
            _script = new Script(_player, "Scripts/movement.lua");
        }

    public override void Initialize()
    {
            _script.Load();
        }

    protected override void Update()
    {
            if (Keyboard.IsKeyPressed(Keyboard.Key.F1))
                _script.Load();
            
            _script.Update(Timer.DeltaTimeSeconds);
        }

    protected override void Render()
    {
            var shape = new CircleShape(10);
            shape.Position = _player.Position;
            shape.FillColor = Color.Blue;
            Window.Draw(shape);
        }
}