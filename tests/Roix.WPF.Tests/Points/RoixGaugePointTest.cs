using Roix.Wpf;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixGaugePointTest
    {
        [Theory]
        [InlineData(0, 0, 1.1, 2.2)]
        [InlineData(1.1, 2.2, 3.3, 4.4)]
        public void Ctor(double x, double y, double width, double height)
        {
            Marshal.SizeOf<RoixGaugePoint>().Is(32);

            var point = new RoixPoint(x, y);
            var size = new RoixSize(width, height);
            var gp1 = new RoixGaugePoint(point, size);

            gp1.Point.X.Is(x);
            gp1.Point.Y.Is(y);
            gp1.Canvas.Width.Is(width);
            gp1.Canvas.Height.Is(height);

            var gp2 = new RoixGaugePoint(x, y, width, height);
            gp2.Point.Is(gp1.Point);
            gp2.Canvas.Is(gp1.Canvas);
        }

        [Theory]
        [InlineData(0, 0, 1, -2)]
        [InlineData(0, 0, -1, 2)]
        public void CtorïâêîÉ_ÉÅ(double x, double y, double width, double height)
        {
            Assert.Throws<ArgumentException>(() => new RoixGaugePoint(x, y, width, height));
            Assert.Throws<ArgumentException>(() => new RoixGaugePoint(new RoixPoint(0, 0), RoixSize.Empty));
        }

        [Fact]
        public void Deconstruct()
        {
            var point = new RoixPoint(1.1, 2.2);
            var size = new RoixSize(3.3, 4.4);
            var gp = new RoixGaugePoint(point, size);
            var (roi, canvas) = gp;
            roi.Is(point);
            canvas.Is(size);
        }

        [Fact]
        public void Equal()
        {
            double x = 1.1, y = 2.2, width = 3.3, height = 4.4;
            var p1 = new RoixGaugePoint(x, y, width, height);
            var p2 = new RoixGaugePoint(x, y, width, height);

            p1.Equals(p2).IsTrue();
            (p1 == p2).IsTrue();
            (p1 != p2).IsFalse();

            var obj2 = (object)p2;
            p1.Equals(obj2).IsTrue();
        }

        #region operator
        [Fact]
        public void Add_RoixGaugePoint_RoixVector()
        {
            double x = 1, y = 2, width = 10, height = 10;
            double vx = 3, vy = 4;

            var gp = new RoixGaugePoint(x, y, width, height);
            var v = new RoixVector(vx, vy);
            var ansRect = new RoixGaugeRect(new RoixRect(x, y, vx, vy), gp.Canvas);

            RoixGaugePoint.Add(gp, v).Is(ansRect);
            (gp + v).Is(ansRect);
            gp.Add(v).Is(ansRect);
        }
        #endregion

        #region Properties
        [Theory]
        [InlineData(0, 0, 1, 1, 0, 0)]
        [InlineData(1, 1, 1, 1, 1, 1)]
        [InlineData(12, 0, 10, 10, 10, 0)]
        [InlineData(5, 34, 10, 10, 5, 10)]
        [InlineData(12, 34, 10, 10, 10, 10)]
        [InlineData(-10, 2, 10, 10, 0, 2)]
        [InlineData(5, -2, 10, 10, 5, 0)]
        [InlineData(-1, -2, 10, 10, 0, 0)]
        public void ClippedRoi(double x, double y, double width, double height, double answerX, double answerY)
        {
            var roi = new RoixGaugePoint(x, y, width, height).ClippedRoi;
            roi.X.Is(answerX);
            roi.Y.Is(answerY);
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
            var roi = new RoixGaugePoint(x, y, width, height);
            roi.IsInside.Is(isInside);
            roi.IsOutside.Is(!isInside);
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData(0.5)]
        [InlineData(4.0)]
        [InlineData(12.34)]
        public void ConvertToNewGauge(double ratio)
        {
            var point = new RoixPoint(10, 20);
            var size = new RoixSize(100, 100);
            var gp1 = new RoixGaugePoint(point, size);

            var newSize = new RoixSize(size.Width * ratio, size.Height * ratio);
            var gp2 = gp1.ConvertToNewGauge(newSize);
            gp2.Point.Is(new RoixPoint(point.X * ratio, point.Y * ratio));

            Assert.Throws<ArgumentException>(() => gp1.ConvertToNewGauge(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => gp1.ConvertToNewGauge(new RoixSize(0, 0)));
        }
        #endregion

    }
}
