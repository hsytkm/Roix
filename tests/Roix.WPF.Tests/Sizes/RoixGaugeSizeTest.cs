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
            gs1.Bounds.Width.Is(width2);
            gs1.Bounds.Height.Is(height2);
        }

        [Fact]
        public void Deconstruct()
        {
            var size1 = new RoixSize(1, 2);
            var size2 = new RoixSize(11, 22);
            var gs1 = new RoixGaugeSize(size1, size2);
            var (size, bounds) = gs1;
            size.Is(size1);
            bounds.Is(size2);
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

            gs1.IsInsideInBounds.Is(isInside);
            gs1.IsOutsideInBounds.Is(!isInside);
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
            var bounds = new RoixSize(100, 100);
            var gs1 = new RoixGaugeSize(size, bounds);

            var newBounds = new RoixSize(bounds.Width * ratio, bounds.Height * ratio);
            var gs2 = gs1.ConvertToNewGauge(newBounds);
            gs2.Size.Is(new RoixSize(size.Width * ratio, size.Height * ratio));

            Assert.Throws<ArgumentException>(() => gs1.ConvertToNewGauge(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => gs1.ConvertToNewGauge(new RoixSize(0, 0)));
        }
        #endregion

    }
}
