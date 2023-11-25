using SFML.Graphics;

namespace Stho.SFML.Extensions;

public static class Fonts
{
    public static Font Roboto { get; } = new Font("Assets/Roboto-Regular.ttf");
    public static Font GetRoboto() => new Font("Assets/Roboto-Regular.ttf");
}