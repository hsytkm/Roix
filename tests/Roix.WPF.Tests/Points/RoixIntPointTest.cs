using Roix.Wpf;
using System;
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

        #region Casts
        [Fact]
        public void ToRoix()
        {
            int x = 1, y = 2;
            var rip1 = new RoixIntPoint(x, y);
            RoixPoint rp1 = (RoixPoint)rip1;
            rp1.X.Is(rip1.X);
            rp1.Y.Is(rip1.Y);
        }

        [Fact]
        public void FromRoix()
        {
            double x = 1.1, y = 2.2;
            var rp1 = new RoixPoint(x, y);
            RoixIntPoint rip1 = (RoixIntPoint)rp1;
            rip1.X.Is((int)Math.Round(rp1.X));
            rip1.Y.Is((int)Math.Round(rp1.Y));
        }

        [Fact]
        public void ToWindows()
        {
            int x = 1, y = 2;
            var rip1 = new RoixIntPoint(x, y);
            System.Windows.Point wp1 = (System.Windows.Point)rip1;
            wp1.X.Is(rip1.X);
            wp1.Y.Is(rip1.Y);
        }

        [Fact]
        public void FromWindows()
        {
            double x = 1.1, y = 2.2;
            var wp1 = new System.Windows.Point(x, y);
            RoixIntPoint rip1 = (RoixIntPoint)wp1;
            rip1.X.Is((int)Math.Round(wp1.X));
            rip1.Y.Is((int)Math.Round(wp1.Y));
        }
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            new RoixIntPoint(1, 0).IsZero.IsFalse();
            new RoixIntPoint(0, 0).IsZero.IsTrue();
            RoixIntPoint.Zero.IsZero.IsTrue();
        }
        #endregion

    }
}
