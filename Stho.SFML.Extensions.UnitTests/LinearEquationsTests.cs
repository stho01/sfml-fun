using System;
using Xunit;

namespace Stho.SFML.Extensions.UnitTests
{
    public class LinearEquationsTests
    {
        [Fact]
        public void GetSlopeIntercept_WhenCalled_ReturnCorrectSlopeAndIntercept()
        {
            var line = new FloatLine(1, 6, 3, -4);
            var result = LinearEquations.GetSlopeInterceptForm(line);
            Assert.Equal(-5, result.M);
            Assert.Equal(11, result.B);
        }
        
        [Fact]
        public void GetTwoPointForm_WhenCalled_ReturnCorrectValues()
        {
            var line = new FloatLine(1, 6, 3, -4);
            var result = LinearEquations.GetTwoPointForm(line);
            Assert.Equal(-5, result.M);
            Assert.Equal(1, result.X1);
            Assert.Equal(6, result.Y1);
        }
        
        [Fact]
        public void GetStandardForm_whenCalled_ReturnCorrectStandardForm()
        {
            var line = new FloatLine(1,6,3,-4);

            // var result = LinearEquations.GetStandardFrom(line);
        }
    }
}