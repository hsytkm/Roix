using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixIntRectTest
    {
        #region Ctor
        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, -2, 3, 4)]
        public void Ctor(int x, int y, int width, int height)
        {
            var point = new RoixIntPoint(x, y);
            var size = new RoixIntSize(width, height);

            var rect1 = new RoixIntRect(point, size);
            rect1.Location.X.Is(point.X);
            rect1.Location.Y.Is(point.Y);
            rect1.Size.Width.Is(size.Width);
            rect1.Size.Height.Is(size.Height);

            var rect2 = new RoixIntRect(new RoixIntPoint(x, y), new RoixIntSize(width, height));
            rect2.Location.X.Is(point.X);
            rect2.Location.Y.Is(point.Y);
            rect2.Size.Width.Is(size.Width);
            rect2.Size.Height.Is(size.Height);
        }

        [Theory]
        [InlineData(0, 0, -1.1, 0)]
        [InlineData(1, 2, 0, -2.2)]
        [InlineData(-1, -2, int.MinValue, int.MinValue)]
        public void Ctorサイズ負数ダメ(int x, int y, int width, int height)
        {
            Assert.Throws<ArgumentException>(() => new RoixIntRect(new RoixIntPoint(x, y), new RoixIntSize(width, height)));
        }

        [Fact]
        public void Deconstruct()
        {
            var rect = new RoixIntRect(new RoixIntPoint(1, 2), new RoixIntSize(3, 4));

            //var (x, y, w, h) = rect;
            //x.Is(rect.Location.X);
            //y.Is(rect.Location.Y);
            //w.Is(rect.Size.Width);
            //h.Is(rect.Size.Height);

            var (p, s) = rect;
            p.Is(rect.Location);
            s.Is(rect.Size);
        }
        #endregion

        #region Equal
        [Fact]
        public void Equal()
        {
            int x = 1, y = 2, width = 3, height = 4;
            var rect1 = new RoixIntRect(new RoixIntPoint(x, y), new RoixIntSize(width, height));
            var rect2 = new RoixIntRect(new RoixIntPoint(x, y), new RoixIntSize(width, height));
            rect1.Equals(rect2).IsTrue();
            (rect1 == rect2).IsTrue();
            (rect1 != rect2).IsFalse();

            var obj2 = (object)rect2;
            rect1.Equals(obj2).IsTrue();
        }
        #endregion

        #region Casts
        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, -2, 3, 4)]
        public void ToRoix(int x, int y, int width, int height)
        {
            var rir1 = new RoixIntRect(new RoixIntPoint(x, y), new RoixIntSize(width, height));
            RoixRect rr1 = (RoixRect)rir1;
            rr1.X.Is(rir1.X);
            rr1.Y.Is(rir1.Y);
            rr1.Width.Is(rir1.Width);
            rr1.Height.Is(rir1.Height);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, -2, 3, 4)]
        public void FromRoix(int x, int y, int width, int height)
        {
            var rr1 = new RoixRect(x, y, width, height);
            RoixIntRect rir1 = rr1.ToRoixInt();
            rir1.X.Is((int)Math.Round(rr1.X));
            rir1.Y.Is((int)Math.Round(rr1.Y));
            rir1.Width.Is((int)Math.Round(rr1.Width));
            rir1.Height.Is((int)Math.Round(rr1.Height));
        }

        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            new RoixIntRect(new RoixIntPoint(1, 2), new RoixIntSize(3, 0)).IsZero.IsFalse();
            new RoixIntRect(new RoixIntPoint(0, 0), new RoixIntSize(0, 0)).IsZero.IsTrue();
            RoixIntRect.Zero.IsZero.IsTrue();
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, -2, 3, 4)]
        public void Properties(double x, double y, double width, double height)
        {
            var point = new RoixPoint(x, y);
            var size = new RoixSize(width, height);
            var r = new RoixIntRect(point.ToRoixInt(), size.ToRoixInt());

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

    }
}
