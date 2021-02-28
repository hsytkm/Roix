using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixGaugeSizeTest
    {
        [Theory]
        [InlineData(0, 0, 1.1, 2.2)]
        [InlineData(1.1, 2.2, 3.3, 4.4)]
        public void Ctor(double width1, double height1, double width2, double height2)
        {
            var size1 = new RoixSize(width1, height1);
            var size2 = new RoixSize(width2, height2);
            var gs1 = new RoixGaugeSize(size1, size2);

            gs1.Size.Width.Is(width1);
            gs1.Size.Height.Is(height1);
            gs1.Border.Width.Is(width2);
            gs1.Border.Height.Is(height2);

            new RoixGaugeSize(size1, RoixSize.Zero);     //OK
        }

        [Theory]
        [InlineData(0, -2, 1, 2)]
        [InlineData(-1, 0, 1, 2)]
        [InlineData(0, 0, 1, -2)]
        [InlineData(0, 0, -1, 2)]
        public void CtorïâêîÉ_ÉÅ(double width1, double height1, double width2, double height2)
        {
            Assert.Throws<ArgumentException>(() => new RoixGaugeSize(new(width1, height1), new(width2, height2)));
            Assert.Throws<ArgumentException>(() => new RoixGaugeSize(new RoixSize(0, 0), RoixSize.Empty));
        }

        [Fact]
        public void Deconstruct()
        {
            var size1 = new RoixSize(1, 2);
            var size2 = new RoixSize(11, 22);
            var gs1 = new RoixGaugeSize(size1, size2);
            var (size, border) = gs1;
            size.Is(size1);
            border.Is(size2);
        }

        [Fact]
        public void Equal()
        {
            var size1 = new RoixSize(1, 2);
            var size2 = new RoixSize(11, 22);
            var gs1 = new RoixGaugeSize(size1, size2);
            var gs2 = new RoixGaugeSize(size1, size2);

            gs1.Equals(gs2).IsTrue();
            (gs1 == gs2).IsTrue();
            (gs1 != gs2).IsFalse();

            var obj2 = (object)gs2;
            gs1.Equals(obj2).IsTrue();
        }

        #region Properties
        [Fact]
        public void IsZero()
        {
            var size1 = new RoixSize(1, 0);
            var size2 = new RoixSize(10, 10);

            new RoixGaugeSize(size1, size2).IsZero.IsFalse();
            new RoixGaugeSize(size1, RoixSize.Zero).IsZero.IsFalse();
            new RoixGaugeSize(RoixSize.Zero, size2).IsZero.IsFalse();
            new RoixGaugeSize(RoixSize.Zero, RoixSize.Zero).IsZero.IsTrue();
            RoixGaugeSize.Zero.IsZero.IsTrue();
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
            var gs1 = new RoixGaugeSize(size1, size2);

            gs1.IsInsideBorder.Is(isInside);
            gs1.IsOutsideBorder.Is(!isInside);
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData(0.5)]
        [InlineData(4.0)]
        [InlineData(12.34)]
        public void ConvertToNewGauge(double ratio)
        {
            var size = new RoixSize(10, 20);
            var border = new RoixSize(100, 100);
            var gs1 = new RoixGaugeSize(size, border);

            var newBorder = new RoixSize(border.Width * ratio, border.Height * ratio);
            var gs2 = gs1.ConvertToNewGauge(newBorder);
            gs2.Size.Is(new RoixSize(size.Width * ratio, size.Height * ratio));

            Assert.Throws<ArgumentException>(() => gs1.ConvertToNewGauge(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => gs1.ConvertToNewGauge(new RoixSize(0, 0)));
        }

        [Theory]
        [InlineData(1.1, 1.9, 1, 2)]
        [InlineData(0, 10, 0, 9)]       // 0~Length-1
        public void ToRoixIntSize_Ok(double width, double height, int ansWidth, int ansHeight)
        {
            var size = new RoixSize(width, height);
            var border = new RoixSize(10, 10);
            var gs = new RoixGaugeSize(size, border).ToRoixIntSize(isCheckBorder: true);
            gs.Width.Is(ansWidth);
            gs.Height.Is(ansHeight);
        }

        [Fact]
        public void ToRoixIntSize_Ng()
        {
            var size = new RoixSize(11, 10);    // Outside
            var border = new RoixSize(10, 10);

            Assert.Throws<InvalidOperationException>(() => new RoixGaugeSize(size, border).ToRoixIntSize(isCheckBorder: true));
            Assert.Throws<InvalidOperationException>(() => new RoixGaugeSize(size, RoixSize.Zero).ToRoixIntSize(isCheckBorder: true));
        }

        #endregion

    }
}
