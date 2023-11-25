using System;

namespace Stho.SFML.Extensions;

public static class MathUtils
{
    public static double DegreeToRadian(double degree)
    {
        return degree * (Math.PI / 180);
    }

    public static double RadianToDegree(double radian)
    {
        return radian * (180 / Math.PI);
    }
}