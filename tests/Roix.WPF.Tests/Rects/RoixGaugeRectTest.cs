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
        public void Ctor(double roiX, double roiY, double roiWidth, double roiHeight, double boundsWidth, double boundsHeight)
        {
            var rect = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var size = new RoixSize(boundsWidth, boundsHeight);

            var gr1 = new RoixGaugeRect(rect, size);
            gr1.Roi.X.Is(rect.X);
            gr1.Roi.Y.Is(rect.Y);
            gr1.Roi.Width.Is(rect.Width);
            gr1.Roi.Height.Is(rect.Height);
            gr1.Bounds.Width.Is(size.Width);
            gr1.Bounds.Height.Is(size.Height);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(0, 0, 4, 4, -1, 0)]
        [InlineData(0, 0, 4, 4, 0, -1)]
        public void CtorBounds負数ダメ(double roiX, double roiY, double roiWidth, double roiHeight, double boundsWidth, double boundsHeight)
        {
            Assert.Throws<ArgumentException>(() => new RoixGaugeRect(new(roiX, roiY, roiWidth, roiHeight), new(boundsWidth, boundsHeight)));
        }

        [Fact]
        public void Deconstruct()
        {
            var srcRect = new RoixRect(1, 2, 3, 4);
            var srcBounds = new RoixSize(10, 10);

            var (roi, bounds) = new RoixGaugeRect(srcRect, srcBounds);
            roi.X.Is(srcRect.X);
            roi.Y.Is(srcRect.Y);
            roi.Width.Is(srcRect.Width);
            roi.Height.Is(srcRect.Height);
            bounds.Width.Is(srcBounds.Width);
            bounds.Height.Is(srcBounds.Height);
        }
        #endregion

        #region Equal
        [Fact]
        public void Equal()
        {
            var rect = new RoixRect(1, 2, 3, 4);
            var bounds = new RoixSize(10, 10);

            var gr1 = new RoixGaugeRect(rect, bounds);
            var gr2 = new RoixGaugeRect(rect, bounds);
            gr1.Equals(gr2).IsTrue();
            (gr1 == gr2).IsTrue();
            (gr1 != gr2).IsFalse();

            var obj2 = (object)gr2;
            gr1.Equals(obj2).IsTrue();
        }
        #endregion

        #region Properties
        [Theory]
        [InlineData(0, 0, 0, 0, 10, 10, true)]
        [InlineData(1, 2, 3, 4, 10, 10, true)]
        [InlineData(10, 20, 3, 4, 10, 10, false)]
        [InlineData(-1, -2, 3, 4, 10, 10, false)]
        public void IsInside(double roiX, double roiY, double roiWidth, double roiHeight, double boundsWidth, double boundsHeight, bool isInside)
        {
            var rect = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var size = new RoixSize(boundsWidth, boundsHeight);
            var gp = new RoixGaugeRect(rect, size);

            gp.IsInsideInBounds.Is(isInside);
            gp.IsOutsideInBounds.Is(!isInside);
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
            var bounds = new RoixSize(100, 100);
            var gr1 = new RoixGaugeRect(new RoixRect(point, size), bounds);

            var newSize = new RoixSize(bounds.Width * ratio, bounds.Height * ratio);
            var gr2 = gr1.ConvertToNewGauge(newSize);

            gr2.Roi.TopLeft.Is(new RoixPoint(point.X * ratio, point.Y * ratio));
            gr2.Roi.Size.Is(new RoixSize(size.Width * ratio, size.Height * ratio));

            Assert.Throws<ArgumentException>(() => gr1.ConvertToNewGauge(RoixSize.Empty));
            Assert.Throws<ArgumentException>(() => gr1.ConvertToNewGauge(new RoixSize(0, 0)));
        }

        [Theory]
        [InlineData(0, 0, 4, 4)]
        [InlineData(0, 0, 10, 10)]
        [InlineData(9, 9, 1, 1)]
        public void GetClippedRoiByPointPriority_1_1_そもそも収まっててOK(
            double roiX, double roiY, double roiWidth, double roiHeight)
        {
            var bounds = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var groi = new RoixGaugeRect(roi, bounds);

            var clippedRect = groi.GetClippedGaugeRect(isPointPriority: true);
            clippedRect.Roi.Is(roi);
            clippedRect.Bounds.Is(groi.Bounds);
        }

        [Theory]
        [InlineData(0, 0, 12, 12, 10, 10)]
        [InlineData(9, 9, 10, 10, 1, 1)]
        [InlineData(10, 10, 1, 1, 0, 0)]  // Size=Zero (OK)
        public void GetClippedRoiByPointPriority_1_2_枠の食み出しをサイズ制限してOK(
            double roiX, double roiY, double roiWidth, double roiHeight, double ansWidth, double ansHeight)
        {
            var bounds = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var ansSize = new RoixSize(ansWidth, ansHeight);

            var groi = new RoixGaugeRect(roi, bounds);
            var clippedRect = groi.GetClippedGaugeRect(isPointPriority: true);
            clippedRect.Roi.Size.Is(ansSize);
            clippedRect.Roi.TopLeft.Is(roi.TopLeft);
            clippedRect.Bounds.Is(groi.Bounds);
        }

        [Theory]
        [InlineData(0, 0, 4, 4)]
        [InlineData(0, 0, 10, 10)]
        [InlineData(9, 9, 1, 1)]
        public void GetClippedRoiBySizePriority_2_1_そもそも収まっててOK(
            double roiX, double roiY, double roiWidth, double roiHeight)
        {
            var bounds = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var groi = new RoixGaugeRect(roi, bounds);

            var clippedRect = groi.GetClippedGaugeRect(isPointPriority: false);
            clippedRect.Roi.Is(roi);
            clippedRect.Bounds.Is(groi.Bounds);
        }

        [Theory]
        [InlineData(9, 9, 2, 2, 8, 8)]
        [InlineData(8, 8, 10, 10, 0, 0)]
        public void GetClippedRoiBySizePriority_2_2_枠の食み出しを位置制限してOK(
            double roiX, double roiY, double roiWidth, double roiHeight, double ansX, double ansY)
        {
            var bounds = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var ansPoint = new RoixPoint(ansX, ansY);

            var groi = new RoixGaugeRect(roi, bounds);
            var clippedRect = groi.GetClippedGaugeRect(isPointPriority: false);
            clippedRect.Roi.TopLeft.Is(ansPoint);
            clippedRect.Roi.Size.Is(roi.Size);
            clippedRect.Bounds.Is(groi.Bounds);
        }
        #endregion

    }
}
