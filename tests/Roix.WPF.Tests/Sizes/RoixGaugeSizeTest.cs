using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixBorderSizeTest
    {
        [Theory]
        [InlineData(0, 0, 1.1, 2.2)]
        [InlineData(1.1, 2.2, 3.3, 4.4)]
        public void Ctor(double width1, double height1, double width2, double height2)
        {
            var size1 = new RoixSize(width1, height1);
            var size2 = new RoixSize(width2, height2);
            var bs1 = new RoixBorderSize(size1, size2);

            bs1.Size.Width.Is(width1);
            bs1.Size.Height.Is(height1);
            bs1.Border.Width.Is(width2);
            bs1.Border.Height.Is(height2);

            new RoixBorderSize(size1, RoixSize.Zero);     //OK
        }

        [Theory]
        [InlineData(0, -2, 1, 2)]
        [InlineData(-1, 0, 1, 2)]
        [InlineData(0, 0, 1, -2)]
        [InlineData(0, 0, -1, 2)]
        public void CtorïâêîÉ_ÉÅ(double width1, double height1, double width2, double height2)
        {
            Assert.Throws<ArgumentException>(() => new RoixBorderSize(new(width1, height1), new(width2, height2)));
            Assert.Throws<ArgumentException>(() => new RoixBorderSize(new RoixSize(0, 0), RoixSize.Empty));
        }

        [Fact]
        public void Deconstruct()
        {
            var size1 = new RoixSize(1, 2);
            var size2 = new RoixSize(11, 22);
            var bs1 = new RoixBorderSize(size1, size2);
            var (size, border) = bs1;
            size.Is(size1);
            border.Is(size2);
        }

        [Fact]
        public void Equal()
        {
            var size1 = new RoixSize(1, 2);
            var size2 = new RoixSize(11, 22);
            var bs1 = new RoixBorderSize(size1, size2);
            var bs2 = new RoixBorderSize(size1, size2);

            bs1.Equals(bs2).IsTrue();
            (bs1 == bs2).IsTrue();
            (bs1 != bs2).IsFalse();

            var obj2 = (object)bs2;
            bs1.Equals(obj2).IsTrue();
        }

        #region Properties
        [Fact]
        public void IsZero()
        {
            var size1 = new RoixSize(1, 0);
            var size2 = new RoixSize(10, 10);

            new RoixBorderSize(size1, size2).IsZero.IsFalse();
            new RoixBorderSize(size1, RoixSize.Zero).IsZero.IsFalse();
            new RoixBorderSize(RoixSize.Zero, size2).IsZero.IsFalse();
            new RoixBorderSize(RoixSize.Zero, RoixSize.Zero).IsZero.IsTrue();
            RoixBorderSize.Zero.IsZero.IsTrue();
        }

        [Theory]
        [InlineData(0, 0, 1, 1, true)]
        [InlineData(1, 1, 1, 1, true)]
        [InlineData(5, 5, 10, 10, true)]
        [InlineData(11, 0, 10, 10, false)]
        [InlineData(0, 11, 10, 10, false)]
        public void IsInside(double width1, double height1, double width2, double height2, bool isInside)
        {
            var size1 = new RoixSize(width1, height1);
            var size2 = new RoixSize(width2, height2);
            var bs1 = new RoixBorderSize(size1, size2);

            bs1.IsInsideBorder.Is(isInside);
            bs1.IsOutsideBorder.Is(!isInside);
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData(0.5)]
        [InlineData(4.0)]
        [InlineData(12.34)]
        public void ConvertToNewBorder(double ratio)
        {
            var size = new RoixSize(10, 20);
            var border = new RoixSize(100, 100);
            var bs1 = new RoixBorderSize(size, border);

            var newBorder = new RoixSize(border.Width * ratio, border.Height * ratio);
            var bs2 = bs1.ConvertToNewBorder(newBorder);
            bs2.Size.Is(new RoixSize(size.Width * ratio, size.Height * ratio));

            Assert.Throws<ArgumentException>(() => bs1.ConvertToNewBorder(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => bs1.ConvertToNewBorder(new RoixSize(0, 0)));
        }

        [Theory]
        [InlineData(1.1, 2.9, 1, 2)]
        [InlineData(0, 10, 0, 9)]       // 0~Length-1
        public void ToRoixIntSize_Ok(double width, double height, int ansWidth, int ansHeight)
        {
            var size = new RoixSize(width, height);
            var border = new RoixSize(10, 10);
            var bs = new RoixBorderSize(size, border).ToRoixIntSize(isCheckBorder: true);
            bs.Width.Is(ansWidth);
            bs.Height.Is(ansHeight);
        }

        [Fact]
        public void ToRoixIntSize_Ng()
        {
            var size = new RoixSize(11, 10);    // Outside
            var border = new RoixSize(10, 10);

            Assert.Throws<InvalidOperationException>(() => new RoixBorderSize(size, border).ToRoixIntSize(isCheckBorder: true));
            Assert.Throws<InvalidOperationException>(() => new RoixBorderSize(size, RoixSize.Zero).ToRoixIntSize(isCheckBorder: true));
        }

        #endregion

    }
}
