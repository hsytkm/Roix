using System;
using Xunit;

namespace Roix.Core.Tests
{
    public class RoixSizeIntTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(12, 34)]
        [InlineData(int.MaxValue, int.MaxValue)]
        public void Ctor(int w, int h)
        {
            var size = new RoixSizeInt(w, h);
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
            Assert.Throws<ArgumentException>(() => new RoixSizeInt(w, h));
            Assert.Throws<ArgumentException>(() =>
            {
                var s0 = new RoixSizeInt(0, 0);
                var s1 = s0 with { Width = w, Height = h };
            });
        }

        [Fact]
        public void Explicit()
        {
            int x = 1234, y = 5678;
            var size = new RoixSizeInt(x, y);
            var point = (RoixPointInt)size;

            point.X.Is(x);
            point.Y.Is(y);
        }

        [Fact]
        public void GetReverse()
        {
            var size0 = new RoixSizeInt(12, 34);
            var size1 = size0.ReverseLength().ReverseLength();

            size0.Is(size1);
            size0.Equals(size1).IsTrue();   // because of record
            (size0 == size1).IsTrue();      // because of record
        }

    }
}
