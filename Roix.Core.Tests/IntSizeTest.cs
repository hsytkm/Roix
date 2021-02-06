using System;
using Xunit;

namespace Roix.Core.Tests
{
    public class IntSizeTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(12, 34)]
        [InlineData(int.MaxValue, int.MaxValue)]
        public void Ctor(int w, int h)
        {
            var size = new IntSize(w, h);
            size.Width.Is(w);
            size.Height.Is(h);

            var (w1, h1) = size;
            w1.Is(w);
            h1.Is(h);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -2)]
        [InlineData(-3, -3)]
        public void ïùÇ™ïâêîÇÕÉ_ÉÅ(int w, int h)
        {
            Assert.Throws<ArgumentException>(() => new IntSize(w, h));
        }

        [Fact]
        public void GetReverse()
        {
            var size0 = new IntSize(12, 34);
            var size1 = size0.GetReverse().GetReverse();

            size0.Is(size1);
            size0.Equals(size1).IsTrue();   // becouse of record
            (size0 == size1).IsTrue();      // becouse of record
        }

        [Fact]
        public void Explicit()
        {
            int x = 1234, y = 5678;
            var size = new IntSize(x, y);
            var point = (IntPoint)size;

            point.X.Is(x);
            point.Y.Is(y);
        }

    }
}
