using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixBorderVectorTest
    {
        #region Ctor
        [Theory]
        [InlineData(0, 0, 1.1, 2.2)]
        [InlineData(1.1, 2.2, 3.3, 4.4)]
        public void Ctor(double x, double y, double width, double height)
        {
            var vector = new RoixVector(x, y);
            var size = new RoixSize(width, height);
            var bv1 = new RoixBorderVector(vector, size);

            bv1.Vector.X.Is(x);
            bv1.Vector.Y.Is(y);
            bv1.Border.Width.Is(width);
            bv1.Border.Height.Is(height);

            var bv2 = new RoixBorderVector(new(x, y), new(width, height));
            bv2.Vector.Is(bv1.Vector);
            bv2.Border.Is(bv1.Border);

            new RoixBorderVector(vector, RoixSize.Zero);     //OK
        }

        [Theory]
        [InlineData(0, 0, 1, -2)]
        [InlineData(0, 0, -1, 2)]
        public void CtorïâêîÉ_ÉÅ(double x, double y, double width, double height)
        {
            Assert.Throws<ArgumentException>(() => new RoixBorderVector(new(x, y), new(width, height)));
            Assert.Throws<ArgumentException>(() => new RoixBorderVector(new RoixVector(0, 0), RoixSize.Empty));
        }

        [Fact]
        public void Deconstruct()
        {
            var vector = new RoixVector(1.1, 2.2);
            var size = new RoixSize(3.3, 4.4);
            var bv = new RoixBorderVector(vector, size);
            var (vec, border) = bv;
            vec.Is(vector);
            border.Is(size);
        }
        #endregion

        #region Equal
        [Fact]
        public void Equal()
        {
            double x = 1.1, y = 2.2, width = 3.3, height = 4.4;
            var p1 = new RoixBorderVector(new(x, y), new(width, height));
            var p2 = new RoixBorderVector(new(x, y), new(width, height));

            p1.Equals(p2).IsTrue();
            (p1 == p2).IsTrue();
            (p1 != p2).IsFalse();

            var obj2 = (object)p2;
            p1.Equals(obj2).IsTrue();
        }
        #endregion

        #region operator
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            var vector = new RoixVector(1, 0);
            var size = new RoixSize(10, 10);

            new RoixBorderVector(vector, size).IsZero.IsFalse();
            new RoixBorderVector(vector, RoixSize.Zero).IsZero.IsFalse();
            new RoixBorderVector(RoixVector.Zero, size).IsZero.IsFalse();
            new RoixBorderVector(RoixVector.Zero, RoixSize.Zero).IsZero.IsTrue();
            RoixBorderVector.Zero.IsZero.IsTrue();
        }

        [Theory]
        [InlineData(0, 0, 1, 1, true)]
        [InlineData(1, 1, 1, 1, true)]
        [InlineData(5, 5, 10, 10, true)]
        [InlineData(11, 0, 10, 10, false)]
        [InlineData(0, 11, 10, 10, false)]
        [InlineData(-1, 0, 10, 10, false)]
        [InlineData(0, -1, 10, 10, false)]
        public void IsInside(double x, double y, double width, double height, bool isInside)
        {
            var vec = new RoixBorderVector(new(x, y), new(width, height));
            vec.IsInsideBorder.Is(isInside);
            vec.IsOutsideBorder.Is(!isInside);
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData(0.5)]
        [InlineData(4.0)]
        [InlineData(12.34)]
        public void ConvertToNewBorder(double ratio)
        {
            var vector = new RoixVector(10, 20);
            var size = new RoixSize(100, 100);
            var bv1 = new RoixBorderVector(vector, size);

            var newSize = new RoixSize(size.Width * ratio, size.Height * ratio);
            var bv2 = bv1.ConvertToNewBorder(newSize);
            bv2.Vector.Is(new RoixVector(vector.X * ratio, vector.Y * ratio));

            Assert.Throws<ArgumentException>(() => bv1.ConvertToNewBorder(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => bv1.ConvertToNewBorder(new RoixSize(0, 0)));
        }
        #endregion

    }
}
