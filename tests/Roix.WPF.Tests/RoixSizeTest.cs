using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixSizeTest
    {
        #region Ctor
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 2)]
        [InlineData(1.1, 2.2)]
        [InlineData(double.MaxValue, double.MaxValue)]
        public void Ctor(double w, double h)
        {
            var size = new RoixSize(w, h);
            size.IsEmpty.IsFalse();
            size.Width.Is(w);
            size.Height.Is(h);
        }

        [Theory]
        [InlineData(-1.1, 0)]
        [InlineData(0, -2.2)]
        [InlineData(double.MinValue, double.MinValue)]
        public void CtorïâêîÉ_ÉÅ(double w, double h)
        {
            Assert.Throws<ArgumentException>(() => new RoixSize(w, h));
        }

        [Fact]
        public void Empty()
        {
            var empty = RoixSize.Empty;
            empty.IsEmpty.IsTrue();
            empty.Width.Is(double.NegativeInfinity);
            empty.Height.Is(double.NegativeInfinity);
        }

        [Fact]
        public void Deconstruct()
        {
            var size = new RoixSize(1.1, 2.2);
            var (w, h) = size;
            w.Is(size.Width);
            h.Is(size.Height);
        }
        #endregion

        #region Cast
        [Fact]
        public void WindowsToRoix()
        {
            var win1 = new System.Windows.Size(1.1, 2.2);
            RoixSize roix1 = win1;
            roix1.IsEmpty.Is(win1.IsEmpty);
            roix1.Width.Is(win1.Width);
            roix1.Height.Is(win1.Height);

            var win2 = System.Windows.Size.Empty;
            RoixSize roix2 = win2;
            roix2.IsEmpty.Is(win2.IsEmpty);
            roix2.Width.Is(win2.Width);
            roix2.Height.Is(win2.Height);
        }

        [Fact]
        public void RoixToWindows()
        {
            var roix1 = new RoixSize(1.1, 2.2);
            System.Windows.Size win1 = roix1;
            win1.IsEmpty.Is(roix1.IsEmpty);
            win1.Width.Is(roix1.Width);
            win1.Height.Is(roix1.Height);

            var roix2 = RoixSize.Empty;
            System.Windows.Size win2 = roix2;
            win2.IsEmpty.Is(roix2.IsEmpty);
            win2.Width.Is(roix2.Width);
            win2.Height.Is(roix2.Height);
        }
        #endregion

        #region Equals
        [Fact]
        public void Equal()
        {
            double w = 1.1, h = 2.2;
            var size1 = new RoixSize(w, h);
            var size2 = new RoixSize(w, h);
            size1.Equals(size2).IsTrue();
            (size1 == size2).IsTrue();
            (size1 != size2).IsFalse();

            var obj2 = (object)size2;
            size1.Equals(obj2).IsTrue();

            size1 = RoixSize.Empty;
            size2 = RoixSize.Empty;
            size1.Equals(size2).IsTrue();
            (size1 == size2).IsTrue();
            (size1 != size2).IsFalse();
        }

        #endregion

        #region Properties
        [Fact]
        public void IsValid()
        {
            new RoixSize(1.1, 2.2).IsValid.IsTrue();
            new RoixSize(1.1, 2.2).IsInvalid.IsFalse();

            new RoixSize(0, 0).IsValid.IsFalse();
            new RoixSize(0, 0).IsInvalid.IsTrue();

            RoixSize.Empty.IsValid.IsFalse();
            RoixSize.Empty.IsInvalid.IsTrue();
        }
        #endregion


    }
}
