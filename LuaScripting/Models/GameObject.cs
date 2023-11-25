using SFML.System;

namespace LuaScripting.Models;

public class GameObject
{
    public string Tag { get; set; }
    public Vector2f Position { get; set; }

    public override string ToString()
    {
        return $"GameObject {{ Tag = {Tag}, Position = {Position} }}";
    }
}