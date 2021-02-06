using System;
using Xunit;

namespace Roix.Core.Tests
{
    public class IntPointTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(12, -34)]
        [InlineData(int.MinValue, int.MaxValue)]
        public void Ctor(int x, int y)
        {
            var size = new IntPoint(x, y);
            size.X.Is(x);
            size.Y.Is(y);

            var (x1, y1) = size;
            x1.Is(x);
            y1.Is(y);
        }

        [Fact]
        public void GetReverse()
        {
            var point0 = new IntPoint(12, -34);
            var point1 = point0.GetReverse().GetReverse();

            point0.Is(point1);
            point0.Equals(point1).IsTrue();   // becouse of record
            (point0 == point1).IsTrue();      // becouse of record
        }

        [Fact]
        public void Explicit()
        {
            int x = 1234, y = 5678;
            var point = new IntPoint(x, y);
            var size = (IntSize)point;

            size.Width.Is(x);
            size.Height.Is(y);
        }

    }
}
