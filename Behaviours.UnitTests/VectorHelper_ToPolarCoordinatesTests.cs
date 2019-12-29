using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using SFML.System;
using Stho.SFML.Extensions;
using Xunit;

namespace Behaviours.UnitTests
{
    public class VectorHelper_ToPolarCoordinatesTests
    {
        [Theory]
        [ClassData(typeof(Vector2ToAngleData))]
        public void WhenCalled_ConvertCartesianToPolarCoordinates(PolarVector2f expected, Vector2f directionVector)
        {
            var result = directionVector.ToPolarCoordinates();
            
            Assert.Equal(expected.R, result.R);
            Assert.Equal(expected.Angle, result.Angle);
        }

        public class Vector2ToAngleData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new PolarVector2f(0, 0), Vector2fHelper.Zero};
                yield return new object[] { new PolarVector2f(1, 90),  new Vector2f(1, 0) };
                yield return new object[] { new PolarVector2f(1, 180), new Vector2f(0, 1) };
                yield return new object[] { new PolarVector2f(1, 270), new Vector2f(-1, 0) };
                yield return new object[] { new PolarVector2f(1, 0), new Vector2f(0, -1) };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
    
}