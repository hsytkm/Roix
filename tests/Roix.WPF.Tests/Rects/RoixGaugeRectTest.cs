using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixBorderRectTest
    {
        #region Ctor
        [Theory]
        [InlineData(0, 0, 0, 0, 10, 10)]
        [InlineData(1, 2, 3, 4, 10, 10)]
        [InlineData(10, 20, 3, 4, 10, 10)]
        [InlineData(-1, -2, 3, 4, 10, 10)]
        public void Ctor(double roiX, double roiY, double roiWidth, double roiHeight, double borderWidth, double borderHeight)
        {
            var point = new RoixPoint(roiX, roiY);
            var size = new RoixSize(roiWidth, roiHeight);
            var rect = new RoixRect(point, size);
            var border = new RoixSize(borderWidth, borderHeight);

            var br1 = new RoixBorderRect(rect, border);
            br1.Roi.Is(rect);
            br1.Border.Is(border);
            new RoixBorderRect(rect, RoixSize.Zero);     //OK
            Assert.Throws<ArgumentException>(() => new RoixBorderRect(rect, RoixSize.Empty));

            var gpoint1 = new RoixBorderPoint(point, border);
            var gsize = new RoixBorderSize(size, border);
            var br2 = new RoixBorderRect(gpoint1, gsize);
            br2.Roi.Is(rect);
            br2.Border.Is(border);
            Assert.Throws<ArgumentException>(() => new RoixBorderRect(gpoint1, RoixBorderSize.Zero));

            var gpoint2 = new RoixBorderPoint(point + (RoixVector)size, border);
            var br3 = new RoixBorderRect(gpoint1, gpoint2);
            br3.Roi.Is(rect);
            br3.Border.Is(border);
            Assert.Throws<ArgumentException>(() => new RoixBorderRect(gpoint1, RoixBorderPoint.Zero));

            var gvector = new RoixBorderVector((RoixVector)size, border);
            var br4 = new RoixBorderRect(gpoint1, gvector);
            br4.Roi.Is(rect);
            br4.Border.Is(border);
        }

        [Theory]
        [InlineData(0, 0, 4, 4, -1, 0)]
        [InlineData(0, 0, 4, 4, 0, -1)]
        public void CtorBorder負数ダメ(double roiX, double roiY, double roiWidth, double roiHeight, double borderWidth, double borderHeight)
        {
            Assert.Throws<ArgumentException>(() => new RoixBorderRect(new RoixRect(roiX, roiY, roiWidth, roiHeight), new RoixSize(borderWidth, borderHeight)));
        }

        [Fact]
        public void Deconstruct()
        {
            var srcRect = new RoixRect(1, 2, 3, 4);
            var srcBorder = new RoixSize(10, 10);

            var (roi, border) = new RoixBorderRect(srcRect, srcBorder);
            roi.X.Is(srcRect.X);
            roi.Y.Is(srcRect.Y);
            roi.Width.Is(srcRect.Width);
            roi.Height.Is(srcRect.Height);
            border.Width.Is(srcBorder.Width);
            border.Height.Is(srcBorder.Height);
        }
        #endregion

        #region Equal
        [Fact]
        public void Equal()
        {
            var rect = new RoixRect(1, 2, 3, 4);
            var border = new RoixSize(10, 10);

            var br1 = new RoixBorderRect(rect, border);
            var br2 = new RoixBorderRect(rect, border);
            br1.Equals(br2).IsTrue();
            (br1 == br2).IsTrue();
            (br1 != br2).IsFalse();

            var obj2 = (object)br2;
            br1.Equals(obj2).IsTrue();
        }
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            var rect = new RoixRect(1, 2, 3, 4);
            var size = new RoixSize(10, 10);

            new RoixBorderRect(rect, size).IsZero.IsFalse();
            new RoixBorderRect(rect, RoixSize.Zero).IsZero.IsFalse();
            new RoixBorderRect(RoixRect.Zero, size).IsZero.IsFalse();
            new RoixBorderRect(RoixRect.Zero, RoixSize.Zero).IsZero.IsTrue();
            RoixBorderRect.Zero.IsZero.IsTrue();
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 10, 10, true)]
        [InlineData(1, 2, 3, 4, 10, 10, true)]
        [InlineData(10, 20, 3, 4, 10, 10, false)]
        [InlineData(-1, -2, 3, 4, 10, 10, false)]
        public void IsInside(double roiX, double roiY, double roiWidth, double roiHeight, double borderWidth, double borderHeight, bool isInside)
        {
            var rect = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var size = new RoixSize(borderWidth, borderHeight);
            var bp = new RoixBorderRect(rect, size);

            bp.IsInsideBorder.Is(isInside);
            bp.IsOutsideBorder.Is(!isInside);
        }
        #endregion

        #region Methods
        [Theory]
        [InlineData(0.5)]
        [InlineData(4.0)]
        [InlineData(12.34)]
        public void ConvertToNewBorder(double ratio)
        {
            var point = new RoixPoint(10, 20);
            var size = new RoixSize(20, 40);
            var border = new RoixSize(100, 100);
            var br1 = new RoixBorderRect(new RoixRect(point, size), border);

            var newSize = new RoixSize(border.Width * ratio, border.Height * ratio);
            var br2 = br1.ConvertToNewBorder(newSize);

            br2.Roi.TopLeft.Is(new RoixPoint(point.X * ratio, point.Y * ratio));
            br2.Roi.Size.Is(new RoixSize(size.Width * ratio, size.Height * ratio));

            Assert.Throws<ArgumentException>(() => br1.ConvertToNewBorder(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => br1.ConvertToNewBorder(new RoixSize(0, 0)));
        }

        //[Theory]
        //[InlineData(1.1, 2.9, 3.1, 4.9, 1, 2, 3, 4)]
        //[InlineData(0, 0, 10, 10, 0, 0, 10, 10)]
        //[InlineData(9, 2, 1, 3, 9, 2, 1, 3)]       // 0~Length-1
        //public void ToRoixIntRect_Ok(double x, double y, double width, double height, int ansX, int ansY, int ansWidth, int ansHeight)
        //{
        //    var rect = new RoixRect(x, y, width, height);
        //    var border = new RoixSize(10, 10);

        //    var ir = new RoixBorderRect(rect, border).ToRoixIntRect(isCheckBorder: true);
        //    var ansRect = new RoixIntRect(ansX, ansY, ansWidth, ansHeight);
        //    ir.Is(ansRect);
        //}

        //[Theory]
        //[InlineData(10, 2, 1, 4)]   // Outside
        //[InlineData(1, 0, 1, 11)]   // Outside
        //public void ToRoixIntRect_Ng(double x, double y, double width, double height)
        //{
        //    var rect = new RoixRect(x, y, width, height);
        //    var border = new RoixSize(10, 10);

        //    Assert.Throws<InvalidOperationException>(() => new RoixBorderRect(rect, border).ToRoixIntRect(isCheckBorder: true));
        //    Assert.Throws<InvalidOperationException>(() => new RoixBorderRect(rect, RoixSize.Zero).ToRoixIntRect(isCheckBorder: true));
        //}

        [Theory]
        [InlineData(0, 0, 4, 4)]
        [InlineData(0, 0, 10, 10)]
        [InlineData(9, 9, 1, 1)]
        public void GetClippedRoiByPointPriority_1_1_そもそも収まっててOK(
            double roiX, double roiY, double roiWidth, double roiHeight)
        {
            var border = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var broi = new RoixBorderRect(roi, border);

            var clippedRect = broi.GetClippedBorderRect(isPointPriority: true);
            clippedRect.Roi.Is(roi);
            clippedRect.Border.Is(broi.Border);
        }

        [Theory]
        [InlineData(0, 0, 12, 12, 10, 10)]
        [InlineData(9, 9, 10, 10, 1, 1)]
        [InlineData(10, 10, 1, 1, 0, 0)]  // Size=Zero (OK)
        public void GetClippedRoiByPointPriority_1_2_枠の食み出しをサイズ制限してOK(
            double roiX, double roiY, double roiWidth, double roiHeight, double ansWidth, double ansHeight)
        {
            var border = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var ansSize = new RoixSize(ansWidth, ansHeight);

            var broi = new RoixBorderRect(roi, border);
            var clippedRect = broi.GetClippedBorderRect(isPointPriority: true);
            clippedRect.Roi.Size.Is(ansSize);
            clippedRect.Roi.TopLeft.Is(roi.TopLeft);
            clippedRect.Border.Is(broi.Border);
        }

        [Theory]
        [InlineData(0, 0, 4, 4)]
        [InlineData(0, 0, 10, 10)]
        [InlineData(9, 9, 1, 1)]
        public void GetClippedRoiBySizePriority_2_1_そもそも収まっててOK(
            double roiX, double roiY, double roiWidth, double roiHeight)
        {
            var border = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var broi = new RoixBorderRect(roi, border);

            var clippedRect = broi.GetClippedBorderRect(isPointPriority: false);
            clippedRect.Roi.Is(roi);
            clippedRect.Border.Is(broi.Border);
        }

        [Theory]
        [InlineData(9, 9, 2, 2, 8, 8)]
        [InlineData(8, 8, 10, 10, 0, 0)]
        public void GetClippedRoiBySizePriority_2_2_枠の食み出しを位置制限してOK(
            double roiX, double roiY, double roiWidth, double roiHeight, double ansX, double ansY)
        {
            var border = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var ansPoint = new RoixPoint(ansX, ansY);

            var broi = new RoixBorderRect(roi, border);
            var clippedRect = broi.GetClippedBorderRect(isPointPriority: false);
            clippedRect.Roi.TopLeft.Is(ansPoint);
            clippedRect.Roi.Size.Is(roi.Size);
            clippedRect.Border.Is(broi.Border);
        }
        #endregion

    }
}
