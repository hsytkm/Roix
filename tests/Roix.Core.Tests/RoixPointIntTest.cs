using System;
using Xunit;

namespace Roix.Core.Tests
{
    public class RoixPointIntTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(12, -34)]
        [InlineData(int.MinValue, int.MaxValue)]
        public void Ctor(int x, int y)
        {
            var size = new RoixPointInt(x, y);
            size.X.Is(x);
            size.Y.Is(y);

            var (x1, y1) = size;
            x1.Is(x);
            y1.Is(y);
        }


        //[Fact]
        //public void Operators()
        //{
        //    int x0 = 10, y0 = -20, x1 = -33, y1 = 44;
        //    var p0 = new IntPoint(x0, y0);
        //    var p1 = new IntPoint(x1, y1);

        //    (p0 + p1).X.Is(x0 + x1);
        //    (p0 + p1).Y.Is(y0 + y1);

        //    (p0 - p1).X.Is(x0 - x1);
        //    (p0 - p1).Y.Is(y0 - y1);
        //}

        [Fact]
        public void Explicit()
        {
            int x = 1234, y = 5678;
            var point = new RoixPointInt(x, y);
            var size = (RoixSizeInt)point;

            size.Width.Is(x);
            size.Height.Is(y);
        }

        //[Fact]
        //public void GetReverse()
        //{
        //    var point0 = new RoixPointInt(12, -34);
        //    var point1 = point0.GetReverse().GetReverse();

        //    point0.Is(point1);
        //    point0.Equals(point1).IsTrue();   // because of record
        //    (point0 == point1).IsTrue();      // because of record
        //}

    }
}
