using System;
using System.Text;

namespace Roix.Wpf
{
    public readonly struct RoixGaugeRect : IEquatable<RoixGaugeRect>, IFormattable
    {
        public readonly RoixRect Roi { get; }
        public readonly RoixSize Bounds { get; }

        #region ctor
        public RoixGaugeRect(in RoixRect roi, in RoixSize bounds)
        {
            if (bounds.IsInvalid) throw new ArgumentException($"Invalid {nameof(bounds)}");
            (Roi, Bounds) = (roi, bounds);
        }

        public readonly void Deconstruct(out RoixRect roi, out RoixSize bounds) => (roi, bounds) = (Roi, Bounds);
        #endregion

        #region Equals
        public readonly bool Equals(RoixGaugeRect other) => (Roi, Bounds) == (other.Roi, other.Bounds);
        public readonly override bool Equals(object? obj) => (obj is RoixGaugeRect other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Roi, Bounds);
        public static bool operator ==(in RoixGaugeRect left, in RoixGaugeRect right) => left.Equals(right);
        public static bool operator !=(in RoixGaugeRect left, in RoixGaugeRect right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixGaugeRect)} {{ {nameof(Roi)} = {Roi}, {nameof(Bounds)} = {Bounds} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixGaugeRect)} {{ ");
            sb.Append($"{nameof(Roi)} = {Roi.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Bounds)} = {Bounds.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region Properties
        public readonly bool IsInsideInBounds => Roi.IsInside(Bounds);
        public readonly bool IsOutsideInBounds => !IsInsideInBounds;
        #endregion

        #region Methods
        public readonly RoixGaugeRect ConvertToNewGauge(in RoixSize newBounds)
        {
            if (Bounds.IsInvalid) return this;
            if (newBounds.IsInvalid) throw new ArgumentException($"Invalid {nameof(newBounds)}");

            var newPoint = new RoixPoint(Roi.X * newBounds.Width / Bounds.Width, Roi.Y * newBounds.Height / Bounds.Height);
            var newSize = new RoixSize(Roi.Width * newBounds.Width / Bounds.Width, Roi.Height * newBounds.Height / Bounds.Height);
            return new(new(newPoint, newSize), newBounds);
        }

        /// <summary>
        /// Roi の左上点を優先して Rect を Bounds サイズ内に納めます。
        /// Roi の左上点が Bounds の境界上に乗っている場合は、戻り値の Size が Zero になります。
        /// </summary>
        private readonly RoixGaugeRect GetClippedGaugeRectByPointPriority()
        {
            if (IsInsideInBounds) return this;

            var left = Math.Clamp(Roi.Left, 0, Bounds.Width);
            var top = Math.Clamp(Roi.Top, 0, Bounds.Height);

            // 最小側にめり込んで制限された場合は正数になる（その値だけ長さを伸縮する）
            var (deltaLeft, deltaTop) = (left - Roi.Left, top - Roi.Top);

            var width = Math.Clamp(Roi.Width - deltaLeft, 0, Bounds.Width - left);
            var height = Math.Clamp(Roi.Height - deltaTop, 0, Bounds.Height - top);
            var rect = new RoixRect(new RoixPoint(left, top), new RoixSize(width, height));
            return new(rect, Bounds);
        }

        /// <summary>
        /// Roi のサイズを優先して Rect を Bounds サイズに納めます。
        /// </summary>
        private readonly RoixGaugeRect GetClippedGaugeRectBySizePriority()
        {
            if (IsInsideInBounds) return this;

            var width = Math.Clamp(Roi.Width, 0, Bounds.Width);
            var height = Math.Clamp(Roi.Height, 0, Bounds.Height);
            var left = Math.Clamp(Roi.Left - GetJutLength(Roi.Left, Roi.Right, 0, Bounds.Width), 0, Bounds.Width - width);
            var top = Math.Clamp(Roi.Top - GetJutLength(Roi.Top, Roi.Bottom, 0, Bounds.Height), 0, Bounds.Height - height);
            var rect = new RoixRect(new RoixPoint(left, top), new RoixSize(width, height));
            return new(rect, Bounds);

            static double GetJutLength(double left, double right, double min, double max)
            {
                // 水平方向の場合、左に食み出てたら負数、右に食み出てたら正数を返す
                if (left < min) return min - left;
                if (max < right) return right - max;
                return 0;
            }
        }

        public readonly RoixGaugeRect GetClippedGaugeRect(bool isPointPriority = true) => isPointPriority ? GetClippedGaugeRectByPointPriority() : GetClippedGaugeRectBySizePriority();
        #endregion

    }
}
