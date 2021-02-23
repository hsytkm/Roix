using Roix.Wpf;
using System;
using System.Linq;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixRectTest
    {
        #region Ctor
        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, -2, 3, 4)]
        public void Ctor(double x, double y, double width, double height)
        {
            var point = new RoixPoint(x, y);
            var size = new RoixSize(width, height);

            var rect1 = new RoixRect(point, size);
            rect1.Location.X.Is(point.X);
            rect1.Location.Y.Is(point.Y);
            rect1.Size.Width.Is(size.Width);
            rect1.Size.Height.Is(size.Height);

            var rect2 = new RoixRect(x, y, width, height);
            rect2.Location.X.Is(point.X);
            rect2.Location.Y.Is(point.Y);
            rect2.Size.Width.Is(size.Width);
            rect2.Size.Height.Is(size.Height);

            var point3 = point + (RoixVector)size;
            var rect3 = new RoixRect(point, point3);
            rect3.Location.X.Is(point.X);
            rect3.Location.Y.Is(point.Y);
            rect3.Size.Width.Is(size.Width);
            rect3.Size.Height.Is(size.Height);

            var rect4 = new RoixRect(point, (RoixVector)size);
            rect4.X.Is(point.X);
            rect4.Y.Is(point.Y);
            rect4.Size.Width.Is(size.Width);
            rect4.Size.Height.Is(size.Height);
        }

        [Theory]
        [InlineData(0, 0, -1.1, 0)]
        [InlineData(1, 2, 0, -2.2)]
        [InlineData(-1, -2, double.MinValue, double.MinValue)]
        public void Ctorサイズ負数ダメ(double x, double y, double width, double height)
        {
            Assert.Throws<ArgumentException>(() => new RoixRect(x, y, width, height));
        }

        [Fact]
        public void Deconstruct()
        {
            var rect = new RoixRect(1, 2, 3, 4);

            var (x, y, w, h) = rect;
            x.Is(rect.Location.X);
            y.Is(rect.Location.Y);
            w.Is(rect.Size.Width);
            h.Is(rect.Size.Height);

            var (p, s) = rect;
            p.Is(rect.Location);
            s.Is(rect.Size);
        }
        #endregion

        #region Equal
        [Fact]
        public void Equal()
        {
            double x = 1, y = 2, w = 3, h = 4;
            var rect1 = new RoixRect(x, y, w, h);
            var rect2 = new RoixRect(x, y, w, h);
            rect1.Equals(rect2).IsTrue();
            (rect1 == rect2).IsTrue();
            (rect1 != rect2).IsFalse();

            var obj2 = (object)rect2;
            rect1.Equals(obj2).IsTrue();

            rect1 = RoixRect.Empty;
            rect2 = RoixRect.Empty;
            rect1.Equals(rect2).IsTrue();
            (rect1 == rect2).IsTrue();
            (rect1 != rect2).IsFalse();
        }
        #endregion

        #region Casts
        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, -2, 3, 4)]
        public void ToWindows(double x, double y, double width, double height)
        {
            var rr1 = new RoixRect(x, y, width, height);
            System.Windows.Rect wr1 = (System.Windows.Rect)rr1;
            wr1.X.Is(rr1.X);
            wr1.Y.Is(rr1.Y);
            wr1.Width.Is(rr1.Width);
            wr1.Height.Is(rr1.Height);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, -2, 3, 4)]
        public void ToRoix(double x, double y, double width, double height)
        {
            var wr1 = new System.Windows.Rect(x, y, width, height);
            RoixRect rr1 = (RoixRect)wr1;
            rr1.X.Is(wr1.X);
            rr1.Y.Is(wr1.Y);
            rr1.Width.Is(wr1.Width);
            rr1.Height.Is(wr1.Height);
        }
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            new RoixRect(1, 2, 3, 0).IsZero.IsFalse();
            new RoixRect(0, 0, 0, 0).IsZero.IsTrue();
            RoixRect.Empty.IsZero.IsFalse();
            RoixRect.Zero.IsZero.IsTrue();
        }

        [Fact]
        public void IsEmpty()
        {
            new RoixRect(1, 2, 3, 0).IsEmpty.IsFalse();
            new RoixRect(0, 0, 0, 0).IsEmpty.IsFalse();
            RoixRect.Empty.IsEmpty.IsTrue();
            RoixRect.Zero.IsEmpty.IsFalse();
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, -2, 3, 4)]
        public void Properties(double x, double y, double width, double height)
        {
            var point = new RoixPoint(x, y);
            var size = new RoixSize(width, height);
            var r = new RoixRect(point, size);

            r.X.Is(r.Location.X);
            r.Y.Is(r.Location.Y);
            r.Width.Is(r.Size.Width);
            r.Height.Is(r.Size.Height);

            r.Left.Is(r.X);
            r.Right.Is(r.X + r.Width);
            r.Top.Is(r.Y);
            r.Bottom.Is(r.Y + r.Height);

            r.TopLeft.Is(new(r.Left, r.Top));
            r.TopRight.Is(new(r.Right, r.Top));
            r.BottomLeft.Is(new(r.Left, r.Bottom));
            r.BottomRight.Is(new(r.Right, r.Bottom));
        }

        #endregion

        #region Methods
        [Theory]
        [InlineData(0, 0, 0, 0, 10, 10, true)]
        [InlineData(1, 2, 3, 4, 10, 10, true)]
        [InlineData(10, 20, 3, 4, 10, 10, false)]
        [InlineData(-1, -2, 3, 4, 10, 10, false)]
        public void IsInside(double roiX, double roiY, double roiWidth, double roiHeight, double boundsWidth, double boundsHeight, bool isInside)
        {
            var rect = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var bounds = new RoixSize(boundsWidth, boundsHeight);

            rect.IsInside(bounds).Is(isInside);
            rect.IsOutside(bounds).Is(!isInside);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, -2, 3, 4)]
        public void ToPointCollection(double p1x, double p1y, double p2x, double p2y)
        {
            var point1 = new RoixPoint(p1x, p1y);
            var point2 = new RoixPoint(p2x, p2y);
            var rect = new RoixRect(point1, point2);
            var src = new[] { rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft };

            var pc = rect.ToPointCollection();
            pc.Select(p => (RoixPoint)p).Is(src);
        }
        #endregion

    }
}
