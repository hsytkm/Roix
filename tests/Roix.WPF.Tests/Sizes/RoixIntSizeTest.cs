using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixIntSizeTest
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 2)]
        [InlineData(int.MaxValue, int.MaxValue)]
        public void Ctor(int w, int h)
        {
            var point = new RoixIntSize(w, h);
            point.Width.Is(w);
            point.Height.Is(h);
        }

        [Fact]
        public void Deconstruct()
        {
            var point = new RoixIntSize(1, 2);
            var (w, y) = point;
            w.Is(point.Width);
            y.Is(point.Height);
        }

        [Fact]
        public void Equal()
        {
            int x = 1, y = 2;
            var p1 = new RoixIntSize(x, y);
            var p2 = new RoixIntSize(x, y);

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
            var ris1 = new RoixIntSize(x, y);
            RoixSize rs1 = (RoixSize)ris1;
            rs1.IsEmpty.IsFalse();
            rs1.Width.Is(ris1.Width);
            rs1.Height.Is(ris1.Height);
        }

        [Fact]
        public void FromRoix()
        {
            double x = 1.1, y = 2.2;
            var rs1 = new RoixSize(x, y);
            RoixIntSize ris1 = (RoixIntSize)rs1;
            ris1.Width.Is((int)Math.Round(rs1.Width));
            ris1.Height.Is((int)Math.Round(rs1.Height));

            Assert.Throws<ArgumentException>(() => (RoixIntSize)RoixSize.Empty);
        }

        [Fact]
        public void ToWindows()
        {
            int x = 1, y = 2;
            var ris1 = new RoixIntSize(x, y);
            System.Windows.Size ws1 = (System.Windows.Size)ris1;
            ws1.IsEmpty.IsFalse();
            ws1.Width.Is(ris1.Width);
            ws1.Height.Is(ris1.Height);
        }

        [Fact]
        public void FromWindows()
        {
            double x = 1.1, y = 2.2;
            var ws1 = new System.Windows.Size(x, y);
            RoixIntSize ris1 = (RoixIntSize)ws1;
            ris1.Width.Is((int)Math.Round(ws1.Width));
            ris1.Height.Is((int)Math.Round(ws1.Height));
        }
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            new RoixIntSize(1, 0).IsZero.IsFalse();
            new RoixIntSize(0, 0).IsZero.IsTrue();
            RoixIntSize.Zero.IsZero.IsTrue();
        }
        #endregion


    }
}
