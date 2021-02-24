using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixGaugePointTest
    {
        #region Ctor
        [Theory]
        [InlineData(0, 0, 1.1, 2.2)]
        [InlineData(1.1, 2.2, 3.3, 4.4)]
        public void Ctor(double x, double y, double width, double height)
        {
            var point = new RoixPoint(x, y);
            var size = new RoixSize(width, height);
            var gp1 = new RoixGaugePoint(point, size);

            gp1.Point.X.Is(x);
            gp1.Point.Y.Is(y);
            gp1.Border.Width.Is(width);
            gp1.Border.Height.Is(height);

            var gp2 = new RoixGaugePoint(x, y, width, height);
            gp2.Point.Is(gp1.Point);
            gp2.Border.Is(gp1.Border);

            new RoixGaugePoint(point, RoixSize.Zero);     //OK
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
            var (roi, border) = gp;
            roi.Is(point);
            border.Is(size);
        }
        #endregion

        #region Equal
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
        #endregion

        #region operator
        [Fact]
        public void Add_RoixGaugePoint_RoixGaugeVector()
        {
            double width = 10, height = 10;
            var point1 = new RoixGaugePoint(10, 10, width, height);
            var vector = new RoixGaugeVector(1, 1, width, height);

            var point2 = point1 + vector;
            point2.Is(new RoixGaugePoint(11, 11, width, height));

            Assert.Throws<NotImplementedException>(() => point1 + new RoixGaugeVector(0, 0, width + 1, height + 1));
        }

        [Fact]
        public void Substruct_RoixGaugePoint_RoixGaugeVector()
        {
            double width = 10, height = 10;
            var point1 = new RoixGaugePoint(10, 10, width, height);
            var vector = new RoixGaugeVector(1, 1, width, height);

            var point2 = point1 - vector;
            point2.Is(new RoixGaugePoint(9, 9, width, height));

            Assert.Throws<NotImplementedException>(() => point1 - new RoixGaugeVector(0, 0, width + 1, height + 1));
        }

        [Fact]
        public void Substruct_RoixGaugePoint_RoixGaugePoint()
        {
            double width = 10, height = 10;
            var point1 = new RoixGaugePoint(10, 10, width, height);
            var point2 = new RoixGaugePoint(1, 1, width, height);

            var vector = point1 - point2;
            vector.Is(new RoixGaugeVector(9, 9, width, height));

            Assert.Throws<NotImplementedException>(() => point1 - new RoixGaugePoint(0, 0, width + 1, height + 1));
        }
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            var point = new RoixPoint(1, 0);
            var size = new RoixSize(10, 10);

            new RoixGaugePoint(point, size).IsZero.IsFalse();
            new RoixGaugePoint(point, RoixSize.Zero).IsZero.IsFalse();
            new RoixGaugePoint(RoixPoint.Zero, size).IsZero.IsFalse();
            new RoixGaugePoint(RoixPoint.Zero, RoixSize.Zero).IsZero.IsTrue();
            RoixGaugePoint.Zero.IsZero.IsTrue();
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
            var point = new RoixGaugePoint(x, y, width, height);
            point.IsInsideBorder.Is(isInside);
            point.IsOutsideBorder.Is(!isInside);
        }

        [Theory]
        [InlineData(0, 0, 1, 1, 0, 0)]
        [InlineData(1, 1, 1, 1, 1, 1)]
        [InlineData(12, 0, 10, 10, 10, 0)]
        [InlineData(5, 34, 10, 10, 5, 10)]
        [InlineData(12, 34, 10, 10, 10, 10)]
        [InlineData(-10, 2, 10, 10, 0, 2)]
        [InlineData(5, -2, 10, 10, 5, 0)]
        [InlineData(-1, -2, 10, 10, 0, 0)]
        public void ClippedPoint(double x, double y, double width, double height, double answerX, double answerY)
        {
            var point = new RoixGaugePoint(x, y, width, height).ClippedRoixPoint;
            point.X.Is(answerX);
            point.Y.Is(answerY);
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

        [Theory]
        [InlineData(1.1, 1.9, 1, 2)]
        [InlineData(0, 10, 0, 9)]       // 0~Length-1
        public void ToRoixIntPoint_Ok(double x, double y, int ansX, int ansY)
        {
            var point = new RoixPoint(x, y);
            var size = new RoixSize(10, 10);
            var gp = new RoixGaugePoint(point, size).ToRoixIntPoint(isCheckBoundaries: true);
            gp.X.Is(ansX);
            gp.Y.Is(ansY);
        }

        [Fact]
        public void ToRoixIntPoint_Ng()
        {
            var point = new RoixPoint(11, 10);    // Outside
            var size = new RoixSize(10, 10);

            Assert.Throws<InvalidOperationException>(() => new RoixGaugePoint(point, size).ToRoixIntPoint(isCheckBoundaries: true));
            Assert.Throws<InvalidOperationException>(() => new RoixGaugePoint(point, RoixSize.Zero).ToRoixIntPoint(isCheckBoundaries: true));
        }

        [Fact]
        public void CreateRoixGaugeRect()
        {
            double x = 1, y = 2, width = 10, height = 10;
            double vx = 3, vy = 4;
            var point = new RoixPoint(x, y);
            var border = new RoixSize(width, height);
            var vector = new RoixVector(vx, vy);
            var ansRect = new RoixGaugeRect(new RoixRect(x, y, vx, vy), border);

            var gp = new RoixGaugePoint(point, border);
            var rect1 = gp.CreateRoixGaugeRect(vector);
            rect1.Is(ansRect);

            var gv = new RoixGaugeVector(vector, border);
            var rect2 = gp.CreateRoixGaugeRect(gv);
            rect2.Is(ansRect);
        }
        #endregion

    }
}
