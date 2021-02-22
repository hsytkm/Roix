using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    public readonly struct RoixGaugeRect : IEquatable<RoixGaugeRect>
    {
        public readonly RoixRect Roi;
        public readonly RoixSize Canvas;

        #region ctor
        public RoixGaugeRect(in RoixRect roi, in RoixSize canvas)
        {
            if (canvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(canvas)}");
            (Roi, Canvas) = (roi, canvas);
        }

        public void Deconstruct(out RoixRect roi, out RoixSize canvas) => (roi, canvas) = (Roi, Canvas);
        #endregion

        #region Equals
        public bool Equals(RoixGaugeRect other) => (Roi, Canvas) == (other.Roi, other.Canvas);
        public override bool Equals(object? obj) => (obj is RoixGaugeRect other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Roi, Canvas);
        public static bool operator ==(in RoixGaugeRect left, in RoixGaugeRect right) => left.Equals(right);
        public static bool operator !=(in RoixGaugeRect left, in RoixGaugeRect right) => !(left == right);
        #endregion

        public override string ToString() => $"{nameof(RoixGaugeRect)} {{ {nameof(Roi)} = {Roi}, {nameof(Canvas)} = {Canvas} }}";

        #region Properties
        public readonly bool IsInsideInCanvas => Roi.IsInside(Canvas);
        public readonly bool IsOutsideInCanvas => !IsInsideInCanvas;
        #endregion

        #region Methods
        public readonly RoixGaugeRect ConvertToNewGauge(in RoixSize newCanvas)
        {
            if (Canvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(Canvas)}");
            if (newCanvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(newCanvas)}");

            var newPoint = new RoixPoint(Roi.X * newCanvas.Width / Canvas.Width, Roi.Y * newCanvas.Height / Canvas.Height);
            var newSize = new RoixSize(Roi.Width * newCanvas.Width / Canvas.Width, Roi.Height * newCanvas.Height / Canvas.Height);
            return new(new(newPoint, newSize), newCanvas);
        }

        /// <summary>
        /// Roi の左上点を優先して Rect を Canvas サイズ内に納めます。
        /// Roi の左上点が Canvas の境界上に乗っている場合は、戻り値の Size が Zero になります。
        /// </summary>
        public readonly RoixRect GetClippedRoiByPointPriority()
        {
            if (IsInsideInCanvas) return Roi;

            var left = Math.Clamp(Roi.Left, 0, Canvas.Width);
            var top = Math.Clamp(Roi.Top, 0, Canvas.Height);

            // 最小側にめり込んで制限された場合は正数になる（その値だけ長さを伸縮する）
            var (deltaLeft, deltaTop) = (left - Roi.Left, top - Roi.Top);

            var width = Math.Clamp(Roi.Width - deltaLeft, 0, Canvas.Width - left);
            var height = Math.Clamp(Roi.Height - deltaTop, 0, Canvas.Height - top);
            return new(new RoixPoint(left, top), new RoixSize(width, height));
        }

        /// <summary>
        /// Roi のサイズを優先して Rect を Canvas サイズに納めます。
        /// </summary>
        public readonly RoixRect GetClippedRoiBySizePriority()
        {
            if (IsInsideInCanvas) return Roi;

            //var newWidth = Roi.Width;
            //if (Canvas.Width < Roi.Right)
            //{
            //    newWidth += Canvas.Width - Roi.Right;
            //}
            //if (Roi.Left < 0)
            //{
            //    newWidth += Roi.Left;
            //}
            //var newHeight = Roi.Height;
            //if (Canvas.Height < Roi.Bottom)
            //{
            //    newHeight += Canvas.Height - Roi.Bottom;
            //}
            //if (Roi.Top < 0)
            //{
            //    newHeight += Roi.Top;
            //}
            ////System.Diagnostics.Debug.WriteLine($"{Roi.Width:f2} -> {newWidth:f2} , {Roi.Height:f2} -> {newHeight:f2}");
            //var left = Math.Clamp(Roi.Left, 0, Canvas.Width - newWidth);
            //var top = Math.Clamp(Roi.Top, 0, Canvas.Height - newHeight);
            //return new(new RoixPoint(left, top), new RoixSize(newWidth, newHeight));

            var width = Math.Clamp(Roi.Width, 0, Canvas.Width);
            var height = Math.Clamp(Roi.Height, 0, Canvas.Height);
            var left = Math.Clamp(Roi.Left - GetJutLength(Roi.Left, Roi.Right, 0, Canvas.Width), 0, Canvas.Width - width);
            var top = Math.Clamp(Roi.Top - GetJutLength(Roi.Top, Roi.Bottom, 0, Canvas.Height), 0, Canvas.Height - height);
            return new(new RoixPoint(left, top), new RoixSize(width, height));

            static double GetJutLength(double left, double right, double min, double max)
            {
                // 水平方向の場合、左に食み出てたら負数、右に食み出てたら正数を返す
                if (left < min) return min - left;
                if (max < right) return right - max;
                return 0;
            }
        }

        public readonly RoixRect GetClippedRoi(bool isPointPriority = true) => isPointPriority ? GetClippedRoiByPointPriority() : GetClippedRoiBySizePriority();
        #endregion

    }
}
