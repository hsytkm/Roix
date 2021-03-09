using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixBorderPointTest
    {
        #region Ctor
        [Theory]
        [InlineData(0, 0, 1.1, 2.2)]
        [InlineData(1.1, 2.2, 3.3, 4.4)]
        public void Ctor(double x, double y, double width, double height)
        {
            var point = new RoixPoint(x, y);
            var size = new RoixSize(width, height);
            var bp1 = new RoixBorderPoint(point, size);

            bp1.Point.X.Is(x);
            bp1.Point.Y.Is(y);
            bp1.Border.Width.Is(width);
            bp1.Border.Height.Is(height);

            var bp2 = new RoixBorderPoint(new(x, y), new(width, height));
            bp2.Point.Is(bp1.Point);
            bp2.Border.Is(bp1.Border);

            new RoixBorderPoint(point, RoixSize.Zero);     //OK
        }

        [Theory]
        [InlineData(0, 0, 1, -2)]
        [InlineData(0, 0, -1, 2)]
        public void CtorïâêîÉ_ÉÅ(double x, double y, double width, double height)
        {
            Assert.Throws<ArgumentException>(() => new RoixBorderPoint(new(x, y), new(width, height)));
            Assert.Throws<ArgumentException>(() => new RoixBorderPoint(new RoixPoint(0, 0), RoixSize.Empty));
        }

        [Fact]
        public void Deconstruct()
        {
            var point = new RoixPoint(1.1, 2.2);
            var size = new RoixSize(3.3, 4.4);
            var bp = new RoixBorderPoint(point, size);
            var (roi, border) = bp;
            roi.Is(point);
            border.Is(size);
        }
        #endregion

        #region Equal
        [Fact]
        public void Equal()
        {
            double x = 1.1, y = 2.2, width = 3.3, height = 4.4;
            var p1 = new RoixBorderPoint(new(x, y), new(width, height));
            var p2 = new RoixBorderPoint(new(x, y), new(width, height));

            p1.Equals(p2).IsTrue();
            (p1 == p2).IsTrue();
            (p1 != p2).IsFalse();

            var obj2 = (object)p2;
            p1.Equals(obj2).IsTrue();
        }
        #endregion

        #region operator
        [Fact]
        public void Add_RoixBorderPoint_RoixBorderVector()
        {
            double width = 10, height = 10;
            var point1 = new RoixBorderPoint(new(10, 10), new(width, height));
            var vector = new RoixBorderVector(new(1, 1), new(width, height));

            var point2 = point1 + vector;
            point2.Is(new RoixBorderPoint(new(11, 11), new(width, height)));

            Assert.Throws<NotImplementedException>(() => point1 + new RoixBorderVector(new(0, 0), new(width + 1, height + 1)));
        }

        [Fact]
        public void Substruct_RoixBorderPoint_RoixBorderVector()
        {
            double width = 10, height = 10;
            var point1 = new RoixBorderPoint(new(10, 10), new(width, height));
            var vector = new RoixBorderVector(new(1, 1), new(width, height));

            var point2 = point1 - vector;
            point2.Is(new RoixBorderPoint(new(9, 9), new(width, height)));

            Assert.Throws<NotImplementedException>(() => point1 - new RoixBorderVector(new(0, 0), new(width + 1, height + 1)));
        }

        [Fact]
        public void Substruct_RoixBorderPoint_RoixBorderPoint()
        {
            double width = 10, height = 10;
            var point1 = new RoixBorderPoint(new(10, 10), new(width, height));
            var point2 = new RoixBorderPoint(new(1, 1), new(width, height));

            var vector = point1 - point2;
            vector.Is(new RoixBorderVector(new(9, 9), new(width, height)));

            Assert.Throws<NotImplementedException>(() => point1 - new RoixBorderPoint(new(0, 0), new(width + 1, height + 1)));
        }
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            var point = new RoixPoint(1, 0);
            var size = new RoixSize(10, 10);

            new RoixBorderPoint(point, size).IsZero.IsFalse();
            new RoixBorderPoint(point, RoixSize.Zero).IsZero.IsFalse();
            new RoixBorderPoint(RoixPoint.Zero, size).IsZero.IsFalse();
            new RoixBorderPoint(RoixPoint.Zero, RoixSize.Zero).IsZero.IsTrue();
            RoixBorderPoint.Zero.IsZero.IsTrue();
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
            var point = new RoixBorderPoint(new(x, y), new(width, height));
            point.IsInsideBorder.Is(isInside);
            point.IsOutsideBorder.Is(!isInside);
        }

        //[Theory]
        //[InlineData(0, 0, 1, 1, 0, 0)]
        //[InlineData(1, 1, 1, 1, 1, 1)]
        //[InlineData(12, 0, 10, 10, 10, 0)]
        //[InlineData(5, 34, 10, 10, 5, 10)]
        //[InlineData(12, 34, 10, 10, 10, 10)]
        //[InlineData(-10, 2, 10, 10, 0, 2)]
        //[InlineData(5, -2, 10, 10, 5, 0)]
        //[InlineData(-1, -2, 10, 10, 0, 0)]
        //public void ClippedPoint(double x, double y, double width, double height, double answerX, double answerY)
        //{
        //    var point = new RoixBorderPoint(new(x, y), new(width, height)).ClippedRoixPoint;
        //    point.X.Is(answerX);
        //    point.Y.Is(answerY);
        //}
        #endregion

        #region Methods
        [Theory]
        [InlineData(0.5)]
        [InlineData(4.0)]
        [InlineData(12.34)]
        public void ConvertToNewBorder(double ratio)
        {
            var point = new RoixPoint(10, 20);
            var size = new RoixSize(100, 100);
            var bp1 = new RoixBorderPoint(point, size);

            var newSize = new RoixSize(size.Width * ratio, size.Height * ratio);
            var bp2 = bp1.ConvertToNewBorder(newSize);
            bp2.Point.Is(new RoixPoint(point.X * ratio, point.Y * ratio));

            Assert.Throws<ArgumentException>(() => bp1.ConvertToNewBorder(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => bp1.ConvertToNewBorder(new RoixSize(0, 0)));
        }

        //[Theory]
        //[InlineData(1.1, 2.9, 1, 2)]
        //[InlineData(0, 10, 0, 9)]       // 0~Length-1
        //public void ToRoixIntPoint_Ok(double x, double y, int ansX, int ansY)
        //{
        //    var point = new RoixPoint(x, y);
        //    var size = new RoixSize(10, 10);
        //    var bp = new RoixBorderPoint(point, size).ToRoixIntPoint(isCheckBorder: true);
        //    bp.X.Is(ansX);
        //    bp.Y.Is(ansY);
        //}

        //[Fact]
        //public void ToRoixIntPoint_Ng()
        //{
        //    var point = new RoixPoint(11, 10);    // Outside
        //    var size = new RoixSize(10, 10);

        //    Assert.Throws<InvalidOperationException>(() => new RoixBorderPoint(point, size).ToRoixIntPoint(isCheckBorder: true));
        //    Assert.Throws<InvalidOperationException>(() => new RoixBorderPoint(point, RoixSize.Zero).ToRoixIntPoint(isCheckBorder: true));
        //}

        //[Fact]
        //public void CreateRoixBorderRect()
        //{
        //    double x = 1, y = 2, width = 10, height = 10;
        //    double vx = 3, vy = 4;
        //    var point = new RoixPoint(x, y);
        //    var border = new RoixSize(width, height);
        //    var vector = new RoixVector(vx, vy);
        //    var ansRect = new RoixBorderRect(new RoixRect(x, y, vx, vy), border);

        //    var bp = new RoixBorderPoint(point, border);
        //    var rect1 = bp.CreateRoixBorderRect(vector);
        //    rect1.Is(ansRect);

        //    var bv = new RoixBorderVector(vector, border);
        //    var rect2 = bp.CreateRoixBorderRect(bv);
        //    rect2.Is(ansRect);
        //}
        #endregion

    }
}
