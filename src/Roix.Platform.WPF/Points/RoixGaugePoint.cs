using Roix.Wpf.Internals;
using System;
using System.Text;

namespace Roix.Wpf
{
    public readonly struct RoixGaugePoint : IEquatable<RoixGaugePoint>, IFormattable
    {
        public readonly RoixPoint Point { get; }
        public readonly RoixSize Canvas { get; }

        #region ctor
        public RoixGaugePoint(in RoixPoint point, in RoixSize canvas)
        {
            if (canvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(canvas)}");
            (Point, Canvas) = (point, canvas);
        }

        public RoixGaugePoint(double x, double y, double width, double height) => (Point, Canvas) = (new(x, y), new(width, height));

        public readonly void Deconstruct(out RoixPoint point, out RoixSize canvas) => (point, canvas) = (Point, Canvas);
        #endregion

        #region Equals
        public readonly bool Equals(RoixGaugePoint other) => (Point, Canvas) == (other.Point, other.Canvas);
        public readonly override bool Equals(object? obj) => (obj is RoixGaugePoint other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Point, Canvas);
        public static bool operator ==(in RoixGaugePoint left, in RoixGaugePoint right) => left.Equals(right);
        public static bool operator !=(in RoixGaugePoint left, in RoixGaugePoint right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixGaugePoint)} {{ {nameof(Point)} = {Point}, {nameof(Canvas)} = {Canvas} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixGaugePoint)} {{ ");
            sb.Append($"{nameof(Point)} = {Point.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Canvas)} = {Canvas.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region operator
        public static RoixGaugeRect Add(in RoixGaugePoint gaugePoint, in RoixVector vector) => new RoixGaugeRect(new RoixRect(gaugePoint.Point, vector), gaugePoint.Canvas);
        public static RoixGaugeRect operator +(in RoixGaugePoint gaugePoint, in RoixVector vector) => Add(gaugePoint, vector);
        public readonly RoixGaugeRect Add(in RoixVector vector) => Add(this, vector);
        #endregion

        #region Properties
        public readonly RoixPoint ClippedRoi => new(Math.Clamp(Point.X, 0, Canvas.Width), Math.Clamp(Point.Y, 0, Canvas.Height));

        public readonly bool IsInsideInCanvas => Point.X.IsInside(0, Canvas.Width) && Point.Y.IsInside(0, Canvas.Height);
        public readonly bool IsOutsideInCanvas => !IsInsideInCanvas;
        #endregion

        #region Methods
        public readonly RoixGaugePoint ConvertToNewGauge(in RoixSize newCanvas)
        {
            if (Canvas.IsInvalid) return this;
            if (newCanvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(newCanvas)}");

            var newPoint = new RoixPoint(Point.X * newCanvas.Width / Canvas.Width, Point.Y * newCanvas.Height / Canvas.Height);
            return new(newPoint, newCanvas);
        }
        #endregion

    }
}
