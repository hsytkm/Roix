using Roix.Wpf;
using System;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixGaugeRectTest
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

            var gr1 = new RoixGaugeRect(rect, border);
            gr1.Roi.Is(rect);
            gr1.Border.Is(border);
            new RoixGaugeRect(rect, RoixSize.Zero);     //OK
            Assert.Throws<ArgumentException>(() => new RoixGaugeRect(rect, RoixSize.Empty));

            var gpoint1 = new RoixGaugePoint(point, border);
            var gsize = new RoixGaugeSize(size, border);
            var gr2 = new RoixGaugeRect(gpoint1, gsize);
            gr2.Roi.Is(rect);
            gr2.Border.Is(border);
            Assert.Throws<ArgumentException>(() => new RoixGaugeRect(gpoint1, RoixGaugeSize.Zero));

            var gpoint2 = new RoixGaugePoint(point + (RoixVector)size, border);
            var gr3 = new RoixGaugeRect(gpoint1, gpoint2);
            gr3.Roi.Is(rect);
            gr3.Border.Is(border);
            Assert.Throws<ArgumentException>(() => new RoixGaugeRect(gpoint1, RoixGaugePoint.Zero));

            var gvector = new RoixGaugeVector((RoixVector)size, border);
            var gr4 = new RoixGaugeRect(gpoint1, gvector);
            gr4.Roi.Is(rect);
            gr4.Border.Is(border);
        }

        [Theory]
        [InlineData(0, 0, 4, 4, -1, 0)]
        [InlineData(0, 0, 4, 4, 0, -1)]
        public void CtorBorder負数ダメ(double roiX, double roiY, double roiWidth, double roiHeight, double borderWidth, double borderHeight)
        {
            Assert.Throws<ArgumentException>(() => new RoixGaugeRect(new RoixRect(roiX, roiY, roiWidth, roiHeight), new RoixSize(borderWidth, borderHeight)));
        }

        [Fact]
        public void Deconstruct()
        {
            var srcRect = new RoixRect(1, 2, 3, 4);
            var srcBorder = new RoixSize(10, 10);

            var (roi, border) = new RoixGaugeRect(srcRect, srcBorder);
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

            var gr1 = new RoixGaugeRect(rect, border);
            var gr2 = new RoixGaugeRect(rect, border);
            gr1.Equals(gr2).IsTrue();
            (gr1 == gr2).IsTrue();
            (gr1 != gr2).IsFalse();

            var obj2 = (object)gr2;
            gr1.Equals(obj2).IsTrue();
        }
        #endregion

        #region Properties
        [Fact]
        public void IsZero()
        {
            var rect = new RoixRect(1, 2, 3, 4);
            var size = new RoixSize(10, 10);

            new RoixGaugeRect(rect, size).IsZero.IsFalse();
            new RoixGaugeRect(rect, RoixSize.Zero).IsZero.IsFalse();
            new RoixGaugeRect(RoixRect.Zero, size).IsZero.IsFalse();
            new RoixGaugeRect(RoixRect.Zero, RoixSize.Zero).IsZero.IsTrue();
            RoixGaugeRect.Zero.IsZero.IsTrue();
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
            var gp = new RoixGaugeRect(rect, size);

            gp.IsInsideBorder.Is(isInside);
            gp.IsOutsideBorder.Is(!isInside);
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
            var size = new RoixSize(20, 40);
            var border = new RoixSize(100, 100);
            var gr1 = new RoixGaugeRect(new RoixRect(point, size), border);

            var newSize = new RoixSize(border.Width * ratio, border.Height * ratio);
            var gr2 = gr1.ConvertToNewGauge(newSize);

            gr2.Roi.TopLeft.Is(new RoixPoint(point.X * ratio, point.Y * ratio));
            gr2.Roi.Size.Is(new RoixSize(size.Width * ratio, size.Height * ratio));

            Assert.Throws<ArgumentException>(() => gr1.ConvertToNewGauge(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => gr1.ConvertToNewGauge(new RoixSize(0, 0)));
        }

        [Theory]
        [InlineData(1.1, 1.9, 3.1, 3.9, 1, 2, 3, 4)]
        [InlineData(0, 0, 10, 10, 0, 0, 10, 10)]
        [InlineData(9, 2, 1, 3, 9, 2, 1, 3)]       // 0~Length-1
        public void ToRoixIntRect_Ok(double x, double y, double width, double height, int ansX, int ansY, int ansWidth, int ansHeight)
        {
            var rect = new RoixRect(x, y, width, height);
            var border = new RoixSize(10, 10);

            var ir = new RoixGaugeRect(rect, border).ToRoixIntRect(isCheckBorder: true);
            var ansRect = new RoixIntRect(ansX, ansY, ansWidth, ansHeight);
            ir.Is(ansRect);
        }

        [Theory]
        [InlineData(10, 2, 1, 4)]   // Outside
        [InlineData(1, 0, 1, 11)]   // Outside
        public void ToRoixIntRect_Ng(double x, double y, double width, double height)
        {
            var rect = new RoixRect(x, y, width, height);
            var border = new RoixSize(10, 10);

            Assert.Throws<InvalidOperationException>(() => new RoixGaugeRect(rect, border).ToRoixIntRect(isCheckBorder: true));
            Assert.Throws<InvalidOperationException>(() => new RoixGaugeRect(rect, RoixSize.Zero).ToRoixIntRect(isCheckBorder: true));
        }

        [Theory]
        [InlineData(0, 0, 4, 4)]
        [InlineData(0, 0, 10, 10)]
        [InlineData(9, 9, 1, 1)]
        public void GetClippedRoiByPointPriority_1_1_そもそも収まっててOK(
            double roiX, double roiY, double roiWidth, double roiHeight)
        {
            var border = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var groi = new RoixGaugeRect(roi, border);

            var clippedRect = groi.GetClippedGaugeRect(isPointPriority: true);
            clippedRect.Roi.Is(roi);
            clippedRect.Border.Is(groi.Border);
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

            var groi = new RoixGaugeRect(roi, border);
            var clippedRect = groi.GetClippedGaugeRect(isPointPriority: true);
            clippedRect.Roi.Size.Is(ansSize);
            clippedRect.Roi.TopLeft.Is(roi.TopLeft);
            clippedRect.Border.Is(groi.Border);
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
            var groi = new RoixGaugeRect(roi, border);

            var clippedRect = groi.GetClippedGaugeRect(isPointPriority: false);
            clippedRect.Roi.Is(roi);
            clippedRect.Border.Is(groi.Border);
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

            var groi = new RoixGaugeRect(roi, border);
            var clippedRect = groi.GetClippedGaugeRect(isPointPriority: false);
            clippedRect.Roi.TopLeft.Is(ansPoint);
            clippedRect.Roi.Size.Is(roi.Size);
            clippedRect.Border.Is(groi.Border);
        }
        #endregion

    }
}
