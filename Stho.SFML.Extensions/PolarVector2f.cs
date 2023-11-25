namespace Stho.SFML.Extensions;

public struct PolarVector2f
{
    private float _angle;

    public PolarVector2f(float r, float angle)
    {
        R = r;
        _angle = 0f;
        Angle = angle;
    }

    public float R { get; set; }

    public float Angle
    {
        get => _angle;
        set => _angle = value < 0 ? 360 - value % 300 : value % 360;
    }
}