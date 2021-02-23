using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixGaugeVectorTest
    {
        #region Ctor
        [Theory]
        [InlineData(0, 0, 1.1, 2.2)]
        [InlineData(1.1, 2.2, 3.3, 4.4)]
        public void Ctor(double x, double y, double width, double height)
        {
            var vector = new RoixVector(x, y);
            var size = new RoixSize(width, height);
            var gv1 = new RoixGaugeVector(vector, size);

            gv1.Vector.X.Is(x);
            gv1.Vector.Y.Is(y);
            gv1.Border.Width.Is(width);
            gv1.Border.Height.Is(height);

            var gv2 = new RoixGaugeVector(x, y, width, height);
            gv2.Vector.Is(gv1.Vector);
            gv2.Border.Is(gv1.Border);

            new RoixGaugeVector(vector, RoixSize.Zero);     //OK
        }

        [Theory]
        [InlineData(0, 0, 1, -2)]
        [InlineData(0, 0, -1, 2)]
        public void CtorïâêîÉ_ÉÅ(double x, double y, double width, double height)
        {
            Assert.Throws<ArgumentException>(() => new RoixGaugeVector(x, y, width, height));
            Assert.Throws<ArgumentException>(() => new RoixGaugeVector(new RoixVector(0, 0), RoixSize.Empty));
        }

        [Fact]
        public void Deconstruct()
        {
            var vector = new RoixVector(1.1, 2.2);
            var size = new RoixSize(3.3, 4.4);
            var gv = new RoixGaugeVector(vector, size);
            var (vec, border) = gv;
            vec.Is(vector);
            border.Is(size);
        }
        #endregion

        #region Equal
        [Fact]
        public void Equal()
        {
            double x = 1.1, y = 2.2, width = 3.3, height = 4.4;
            var p1 = new RoixGaugeVector(x, y, width, height);
            var p2 = new RoixGaugeVector(x, y, width, height);

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

            new RoixGaugeVector(vector, size).IsZero.IsFalse();
            new RoixGaugeVector(vector, RoixSize.Zero).IsZero.IsFalse();
            new RoixGaugeVector(RoixVector.Zero, size).IsZero.IsFalse();
            new RoixGaugeVector(RoixVector.Zero, RoixSize.Zero).IsZero.IsTrue();
            RoixGaugeVector.Zero.IsZero.IsTrue();
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
            var vec = new RoixGaugeVector(x, y, width, height);
            vec.IsInsideBorder.Is(isInside);
            vec.IsOutsideBorder.Is(!isInside);
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData(0.5)]
        [InlineData(4.0)]
        [InlineData(12.34)]
        public void ConvertToNewGauge(double ratio)
        {
            var vector = new RoixVector(10, 20);
            var size = new RoixSize(100, 100);
            var gv1 = new RoixGaugeVector(vector, size);

            var newSize = new RoixSize(size.Width * ratio, size.Height * ratio);
            var gv2 = gv1.ConvertToNewGauge(newSize);
            gv2.Vector.Is(new RoixVector(vector.X * ratio, vector.Y * ratio));

            Assert.Throws<ArgumentException>(() => gv1.ConvertToNewGauge(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => gv1.ConvertToNewGauge(new RoixSize(0, 0)));
        }
        #endregion

    }
}
