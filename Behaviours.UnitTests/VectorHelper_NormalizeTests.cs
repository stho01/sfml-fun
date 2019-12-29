using System.Collections;
using System.Collections.Generic;
using SFML.System;
using Stho.SFML.Extensions;
using Xunit;

namespace Behaviours.UnitTests
{
    public class VectorHelper_NormalizeTests
    {
        [Theory]
        [ClassData(typeof(NormalizeTestData))]
        public void WhenCalled_CalculateUnitVector(Vector2f expectedUnitVector, Vector2f vector)
        {
            var result = vector.Normalize();
            
            Assert.Equal(expectedUnitVector, result);
        }
        
        
        class NormalizeTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // [0] = expected, [1] = vector to calculate
                yield return new object[] {new Vector2f(0, 0), new Vector2f(0, 0)};
                yield return new object[] {new Vector2f(1, 0), new Vector2f(1, 0)};
                yield return new object[] {new Vector2f(1, 0), new Vector2f(2, 0)};
                yield return new object[] {new Vector2f(0, 1), new Vector2f(0, 1)};
                yield return new object[] {new Vector2f(0, 1), new Vector2f(0, 2)};
                yield return new object[] {new Vector2f(0.70710677f, 0.70710677f), new Vector2f(1, 1)};
                yield return new object[] {new Vector2f(0.70710677f, 0.70710677f), new Vector2f(2, 2)};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}