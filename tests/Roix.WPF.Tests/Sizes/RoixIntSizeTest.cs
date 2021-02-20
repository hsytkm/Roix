using Roix.Wpf;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixIntSizeTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-1, 2)]
        [InlineData(int.MinValue, int.MaxValue)]
        public void Ctor(int w, int h)
        {
            Marshal.SizeOf<RoixIntSize>().Is(8);

            var point = new RoixIntSize(w, h);
            point.Width.Is(w);
            point.Height.Is(h);
        }

        [Fact]
        public void Deconstruct()
        {
            var point = new RoixIntSize(1, 2);
            var (w, y) = point;
            w.Is(point.Width);
            y.Is(point.Height);
        }

        [Fact]
        public void Equal()
        {
            int x = 1, y = 2;
            var p1 = new RoixIntSize(x, y);
            var p2 = new RoixIntSize(x, y);

            p1.Equals(p2).IsTrue();
            (p1 == p2).IsTrue();
            (p1 != p2).IsFalse();

            var obj2 = (object)p2;
            p1.Equals(obj2).IsTrue();
        }

    }
}
