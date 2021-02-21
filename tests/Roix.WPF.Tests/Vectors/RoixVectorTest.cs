using Roix.Wpf;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixVectorTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-0.1, 0.2)]
        [InlineData(double.MinValue, double.MaxValue)]
        public void Ctor(double x, double y)
        {
            Marshal.SizeOf<RoixVector>().Is(16);

            var vec = new RoixVector(x, y);
            vec.X.Is(x);
            vec.Y.Is(y);
        }

        [Fact]
        public void Deconstruct()
        {
            var vec = new RoixVector(1.1, 2.2);
            var (x, y) = vec;
            x.Is(vec.X);
            y.Is(vec.Y);
        }

        #region Equal
        [Fact]
        public void Equal()
        {
            double x = 1.1, y = 2.2;
            var v1 = new RoixVector(x, y);
            var v2 = new RoixVector(x, y);

            v1.Equals(v2).IsTrue();
            (v1 == v2).IsTrue();
            (v1 != v2).IsFalse();

            var obj2 = (object)v2;
            v1.Equals(obj2).IsTrue();
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
            var rv = new RoixVector(x, y);

            System.Windows.Vector wv = (System.Windows.Vector)rv;
            wv.X.Is(rv.X);
            wv.Y.Is(rv.Y);

            System.Windows.Point wp = (System.Windows.Point)rv;
            wp.X.Is(rv.X);
            wp.Y.Is(rv.Y);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-0.1, 0.2)]
        [InlineData(double.MinValue, double.MaxValue)]
        public void FromWindows(double x, double y)
        {
            var wv = new System.Windows.Vector(x, y);
            RoixVector rv1 = (RoixVector)wv;
            rv1.X.Is(wv.X);
            rv1.Y.Is(wv.Y);

            var wp = new System.Windows.Point(x, y);
            RoixVector rv2 = (RoixVector)wp;
            rv2.X.Is(wp.X);
            rv2.Y.Is(wp.Y);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, -2)]
        [InlineData(-0.1, 0.2)]
        [InlineData(double.MinValue, double.MaxValue)]
        public void ToRoix(double x, double y)
        {
            var rv = new RoixVector(x, y);

            RoixPoint rp = (RoixPoint)rv;
            rp.X.Is(rv.X);
            rp.Y.Is(rv.Y);
        }
        #endregion

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 30, 40)]
        [InlineData(-1, -2, 30, 40)]
        [InlineData(1, 2, -30, -40)]
        public void Operators(double x1, double y1, double x2, double y2)
        {
            var v1 = new RoixVector(x1, y1) + new RoixVector(x2, y2);
            v1.X.Is(x1 + x2);
            v1.Y.Is(y1 + y2);

            var v21 = -new RoixVector(x1, y1);
            v21.X.Is(-x1);
            v21.Y.Is(-y1);

            var v22 = -new RoixVector(x2, y2);
            v22.X.Is(-x2);
            v22.Y.Is(-y2);

            var v3 = new RoixVector(x1, y1) - new RoixVector(x2, y2);
            v3.X.Is(x1 - x2);
            v3.Y.Is(y1 - y2);
        }

        #region Properties
        [Fact]
        public void IsZero()
        {
            new RoixVector(1.1, 0).IsZero.IsFalse();
            new RoixVector(0, 0).IsZero.IsTrue();
            RoixVector.Zero.IsZero.IsTrue();
        }
        #endregion

    }
}
