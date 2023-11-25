using SFML.Graphics;

namespace Stho.SFML.Extensions;

public static class Fonts
{
    public static Font Roboto { get; } = new("Assets/Roboto-Regular.ttf");

    public static Font GetRoboto()
    {
        return new Font("Assets/Roboto-Regular.ttf");
    }
}