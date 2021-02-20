using Roix.Wpf;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixIntPointTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-1, 2)]
        [InlineData(int.MinValue, int.MaxValue)]
        public void Ctor(int x, int y)
        {
            Marshal.SizeOf<RoixIntPoint>().Is(8);

            var point = new RoixIntPoint(x, y);
            point.X.Is(x);
            point.Y.Is(y);
        }

        [Fact]
        public void Deconstruct()
        {
            var point = new RoixIntPoint(1, 2);
            var (x, y) = point;
            x.Is(point.X);
            y.Is(point.Y);
        }

        [Fact]
        public void Equal()
        {
            int x = 1, y = 2;
            var p1 = new RoixIntPoint(x, y);
            var p2 = new RoixIntPoint(x, y);

            p1.Equals(p2).IsTrue();
            (p1 == p2).IsTrue();
            (p1 != p2).IsFalse();

            var obj2 = (object)p2;
            p1.Equals(obj2).IsTrue();
        }

    }
}
