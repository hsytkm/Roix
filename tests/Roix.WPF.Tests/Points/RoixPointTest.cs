using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixPointTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-0.1, 0.2)]
        [InlineData(double.MinValue, double.MaxValue)]
        public void Ctor(double x, double y)
        {
            var point = new RoixPoint(x, y);
            point.X.Is(x);
            point.Y.Is(y);
        }

        [Fact]
        public void Deconstruct()
        {
            var point = new RoixPoint(1.1, 2.2);
            var (x, y) = point;
            x.Is(point.X);
            y.Is(point.Y);
        }

        #region Equal
        [Fact]
        public void Equal()
        {
            double x = 1.1, y = 2.2;
            var p1 = new RoixPoint(x, y);
            var p2 = new RoixPoint(x, y);

            p1.Equals(p2).IsTrue();
            (p1 == p2).IsTrue();
            (p1 != p2).IsFalse();

            var obj2 = (object)p2;
            p1.Equals(obj2).IsTrue();
        }
        #endregion

        #region Casts
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-0.1, 0.2)]
        [InlineData(double.MinValue, double.MaxValue)]
        public void ToWindows(double x, double y)
        {
            var rp1 = new RoixPoint(x, y);
            System.Windows.Point ws1 = (System.Windows.Point)rp1;
            ws1.X.Is(rp1.X);
            ws1.Y.Is(rp1.Y);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(1, -2)]
        [InlineData(-0.1, 0.2)]
        [InlineData(double.MinValue, double.MaxValue)]
        public void ToRoix(double x, double y)
        {
            var wp1 = new System.Windows.Point(x, y);
            RoixPoint rp1 = (RoixPoint)wp1;
            rp1.X.Is(wp1.X);
            rp1.Y.Is(wp1.Y);

            var rp2 = new RoixPoint(x, y);
            RoixVector rv2 = (RoixVector)rp2;
            rv2.X.Is(rp2.X);
            rv2.Y.Is(rp2.Y);

            if (rp2.X >= 0 && rp2.Y >= 0)
            {
                RoixSize rs2 = (RoixSize)rp2;
                rs2.Width.Is(rp2.X);
                rs2.Height.Is(rp2.Y);
            }
        }
        #endregion

        #region Operators
        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 30, 40)]
        [InlineData(-1, -2, 30, 40)]
        [InlineData(1, 2, -30, -40)]
        public void Operators(double x1, double y1, double x2, double y2)
        {
            var p1 = new RoixPoint(x1, y1) + new RoixVector(x2, y2);
            p1.X.Is(x1 + x2);
            p1.Y.Is(y1 + y2);

            var p2 = new RoixPoint(x1, y1) - new RoixVector(x2, y2);
            p2.X.Is(x1 - x2);
            p2.Y.Is(y1 - y2);

            var v1 = new RoixPoint(x1, y1) - new RoixPoint(x2, y2);
            v1.X.Is(x1 - x2);
            v1.Y.Is(y1 - y2);
        }
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            new RoixPoint(1.1, 0).IsZero.IsFalse();
            new RoixPoint(0, 0).IsZero.IsTrue();
            RoixPoint.Zero.IsZero.IsTrue();
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData(0, 0, true)]
        [InlineData(10, 10, true)]
        [InlineData(11, 0, false)]
        [InlineData(0, 11, false)]
        [InlineData(-1, 0, false)]
        [InlineData(0, -1, false)]
        [InlineData(11, -1, false)]
        public void IsInside(double x, double y, bool isInside)
        {
            var canvs = new RoixSize(10, 10);
            var ip = new RoixPoint(x, y);

            ip.IsInside(canvs).Is(isInside);
            ip.IsOutside(canvs).Is(!isInside);
        }
        #endregion

    }
}
