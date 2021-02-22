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
        public void Ctor(double roiX, double roiY, double roiWidth, double roiHeight, double canvasWidth, double canvasHeight)
        {
            var rect = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var size = new RoixSize(canvasWidth, canvasHeight);

            var gr1 = new RoixGaugeRect(rect, size);
            gr1.Roi.X.Is(rect.X);
            gr1.Roi.Y.Is(rect.Y);
            gr1.Roi.Width.Is(rect.Width);
            gr1.Roi.Height.Is(rect.Height);
            gr1.Canvas.Width.Is(size.Width);
            gr1.Canvas.Height.Is(size.Height);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(0, 0, 4, 4, -1, 0)]
        [InlineData(0, 0, 4, 4, 0, -1)]
        public void CtorCanvas負数ダメ(double roiX, double roiY, double roiWidth, double roiHeight, double canvasWidth, double canvasHeight)
        {
            Assert.Throws<ArgumentException>(() => new RoixGaugeRect(new(roiX, roiY, roiWidth, roiHeight), new(canvasWidth, canvasHeight)));
        }

        [Fact]
        public void Deconstruct()
        {
            var srcRect = new RoixRect(1, 2, 3, 4);
            var srcCanvas = new RoixSize(10, 10);

            var (roi, canvas) = new RoixGaugeRect(srcRect, srcCanvas);
            roi.X.Is(srcRect.X);
            roi.Y.Is(srcRect.Y);
            roi.Width.Is(srcRect.Width);
            roi.Height.Is(srcRect.Height);
            canvas.Width.Is(srcCanvas.Width);
            canvas.Height.Is(srcCanvas.Height);
        }
        #endregion

        #region Equal
        [Fact]
        public void Equal()
        {
            var rect = new RoixRect(1, 2, 3, 4);
            var canvas = new RoixSize(10, 10);

            var gr1 = new RoixGaugeRect(rect, canvas);
            var gr2 = new RoixGaugeRect(rect, canvas);
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
        public void IsInside(double roiX, double roiY, double roiWidth, double roiHeight, double canvasWidth, double canvasHeight, bool isInside)
        {
            var rect = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var size = new RoixSize(canvasWidth, canvasHeight);
            var gp = new RoixGaugeRect(rect, size);

            gp.IsInsideInCanvas.Is(isInside);
            gp.IsOutsideInCanvas.Is(!isInside);
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
            var canvas = new RoixSize(100, 100);
            var gr1 = new RoixGaugeRect(new RoixRect(point, size), canvas);

            var newSize = new RoixSize(canvas.Width * ratio, canvas.Height * ratio);
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
            var canvas = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var groi = new RoixGaugeRect(roi, canvas);

            var clippedRoi = groi.GetClippedRoiByPointPriority();
            clippedRoi.Is(roi);
        }

        [Theory]
        [InlineData(0, 0, 12, 12, 10, 10)]
        [InlineData(9, 9, 10, 10, 1, 1)]
        [InlineData(10, 10, 1, 1, 0, 0)]  // Size=Zero (OK)
        public void GetClippedRoiByPointPriority_1_2_枠の食み出しをサイズ制限してOK(
            double roiX, double roiY, double roiWidth, double roiHeight, double ansWidth, double ansHeight)
        {
            var canvas = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var ansSize = new RoixSize(ansWidth, ansHeight);

            var groi = new RoixGaugeRect(roi, canvas);
            var clippedRoi = groi.GetClippedRoiByPointPriority();
            clippedRoi.Size.Is(ansSize);
            clippedRoi.TopLeft.Is(roi.TopLeft);
        }

        [Theory]
        [InlineData(0, 0, 4, 4)]
        [InlineData(0, 0, 10, 10)]
        [InlineData(9, 9, 1, 1)]
        public void GetClippedRoiBySizePriority_2_1_そもそも収まっててOK(
            double roiX, double roiY, double roiWidth, double roiHeight)
        {
            var canvas = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var groi = new RoixGaugeRect(roi, canvas);

            var clippedRoi = groi.GetClippedRoiBySizePriority();
            clippedRoi.Is(roi);
        }

        [Theory]
        [InlineData(9, 9, 2, 2, 8, 8)]
        [InlineData(8, 8, 10, 10, 0, 0)]
        public void GetClippedRoiBySizePriority_2_2_枠の食み出しを位置制限してOK(
            double roiX, double roiY, double roiWidth, double roiHeight, double ansX, double ansY)
        {
            var canvas = new RoixSize(10, 10);
            var roi = new RoixRect(roiX, roiY, roiWidth, roiHeight);
            var ansPoint = new RoixPoint(ansX, ansY);

            var groi = new RoixGaugeRect(roi, canvas);
            var clippedRoi = groi.GetClippedRoiBySizePriority();
            clippedRoi.TopLeft.Is(ansPoint);
            clippedRoi.Size.Is(roi.Size);
        }
        #endregion

    }
}
