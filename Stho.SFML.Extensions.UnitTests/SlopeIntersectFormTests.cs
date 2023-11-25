using Xunit;

namespace Stho.SFML.Extensions.UnitTests;

public class SlopeIntersectFormTests
{
    [Fact]
    public void PointOnLineX_WhenCalled_ReturnCorrectPoint()
    {
        var slopeIntersect = new SlopeInterceptForm
        {
            M = -5, // slope
            B = 11 // y intercept
        };

        var result1 = slopeIntersect.PointOnLineX(0);
        Assert.Equal(0, result1.X);
        Assert.Equal(11, result1.Y);

        var result2 = slopeIntersect.PointOnLineX(1);
        Assert.Equal(1, result2.X);
        Assert.Equal(6, result2.Y);


        var result3 = slopeIntersect.PointOnLineX(3);
        Assert.Equal(3, result3.X);
        Assert.Equal(-4, result3.Y);
    }

    [Fact]
    public void PointOnLineY_WhenCalled_ReturnCorrectPoint()
    {
        var slopeIntersect = new SlopeInterceptForm
        {
            M = -5, // slope
            B = 11 // y intercept
        };

        var result1 = slopeIntersect.PointOnLineY(6);
        Assert.Equal(1, result1.X);
        Assert.Equal(6, result1.Y);

        var result2 = slopeIntersect.PointOnLineY(-4);
        Assert.Equal(3, result2.X);
        Assert.Equal(-4, result2.Y);
    }
}