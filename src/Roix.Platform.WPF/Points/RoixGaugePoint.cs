using Roix.Wpf.Internals;
using System;
using System.Text;

namespace Roix.Wpf
{
    public readonly struct RoixGaugePoint : IEquatable<RoixGaugePoint>, IFormattable
    {
        public readonly RoixPoint Point { get; }
        public readonly RoixSize Bounds { get; }

        #region ctor
        public RoixGaugePoint(in RoixPoint point, in RoixSize bounds)
        {
            if (bounds.IsInvalid) throw new ArgumentException($"Invalid {nameof(bounds)}");
            (Point, Bounds) = (point, bounds);
        }

        public RoixGaugePoint(double x, double y, double width, double height) => (Point, Bounds) = (new(x, y), new(width, height));

        public readonly void Deconstruct(out RoixPoint point, out RoixSize bounds) => (point, bounds) = (Point, Bounds);
        #endregion

        #region Equals
        public readonly bool Equals(RoixGaugePoint other) => (Point, Bounds) == (other.Point, other.Bounds);
        public readonly override bool Equals(object? obj) => (obj is RoixGaugePoint other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Point, Bounds);
        public static bool operator ==(in RoixGaugePoint left, in RoixGaugePoint right) => left.Equals(right);
        public static bool operator !=(in RoixGaugePoint left, in RoixGaugePoint right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixGaugePoint)} {{ {nameof(Point)} = {Point}, {nameof(Bounds)} = {Bounds} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixGaugePoint)} {{ ");
            sb.Append($"{nameof(Point)} = {Point.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Bounds)} = {Bounds.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region operator
        public static RoixGaugeRect Add(in RoixGaugePoint gaugePoint, in RoixVector vector) => new RoixGaugeRect(new RoixRect(gaugePoint.Point, vector), gaugePoint.Bounds);
        public static RoixGaugeRect operator +(in RoixGaugePoint gaugePoint, in RoixVector vector) => Add(gaugePoint, vector);
        public readonly RoixGaugeRect Add(in RoixVector vector) => Add(this, vector);
        #endregion

        #region Properties
        public readonly RoixPoint ClippedRoi => new(Math.Clamp(Point.X, 0, Bounds.Width), Math.Clamp(Point.Y, 0, Bounds.Height));

        public readonly bool IsInsideInBounds => Point.X.IsInside(0, Bounds.Width) && Point.Y.IsInside(0, Bounds.Height);
        public readonly bool IsOutsideInBounds => !IsInsideInBounds;
        #endregion

        #region Methods
        public readonly RoixGaugePoint ConvertToNewGauge(in RoixSize newBounds)
        {
            if (Bounds.IsInvalid) return this;
            if (newBounds.IsInvalid) throw new ArgumentException($"Invalid {nameof(newBounds)}");

            var newPoint = new RoixPoint(Point.X * newBounds.Width / Bounds.Width, Point.Y * newBounds.Height / Bounds.Height);
            return new(newPoint, newBounds);
        }
        #endregion

    }
}
