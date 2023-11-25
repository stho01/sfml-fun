using Xunit;

namespace Stho.SFML.Extensions.UnitTests;

public class TwoPointFormTests
{
    [Fact]
    public void PointOnLineX_WhenCalled_ReturnCorrectPoint()
    {
        var twoPointForm = new TwoPointForm
        {
            M = -5, // slope
            X1 = 1,
            Y1 = 6
        };

        var result1 = twoPointForm.PointOnLineX(0);
        Assert.Equal(0, result1.X);
        Assert.Equal(11, result1.Y);

        var result2 = twoPointForm.PointOnLineX(1);
        Assert.Equal(1, result2.X);
        Assert.Equal(6, result2.Y);

        var result3 = twoPointForm.PointOnLineX(3);
        Assert.Equal(3, result3.X);
        Assert.Equal(-4, result3.Y);
    }

    [Fact]
    public void PointOnLineY_WhenCalled_ReturnCorrectPoint()
    {
        var twoPointForm = new TwoPointForm
        {
            M = -5, // slope
            X1 = 1,
            Y1 = 6
        };

        var result1 = twoPointForm.PointOnLineY(6);
        Assert.Equal(1, result1.X);
        Assert.Equal(6, result1.Y);

        var result2 = twoPointForm.PointOnLineY(-4);
        Assert.Equal(3, result2.X);
        Assert.Equal(-4, result2.Y);
    }
}