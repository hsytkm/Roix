using Roix.Wpf;
using System;
using System.Runtime.InteropServices;
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
            Marshal.SizeOf<RoixPoint>().Is(16);

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
        }
        #endregion

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
        [InlineData(0, 0, 0, 0)]
        [InlineData(1.49, 2.51, 1, 3)]
        [InlineData(-1.49, -2.51, -1, -3)]
        public void ToRoixIntPoint(double x, double y, int answerX, int answerY)
        {
            var ip = new RoixPoint(x, y).ToRoixIntPoint();
            ip.X.Is(answerX);
            ip.Y.Is(answerY);
        }
        #endregion

    }
}
