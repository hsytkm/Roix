using Roix.Wpf.Internals;
using System;
using System.Text;

namespace Roix.Wpf
{
    public readonly struct RoixGaugePoint : IEquatable<RoixGaugePoint>, IFormattable
    {
        public static RoixGaugePoint Zero { get; } = new(RoixPoint.Zero, RoixSize.Zero);

        public readonly RoixPoint Point { get; }
        public readonly RoixSize Border { get; }

        #region ctor
        public RoixGaugePoint(in RoixPoint point, in RoixSize border)
        {
            if (border.IsEmpty) throw new ArgumentException($"{nameof(border)} is empty");
            (Point, Border) = (point, border);
        }

        public RoixGaugePoint(double x, double y, double width, double height) => (Point, Border) = (new(x, y), new(width, height));

        public readonly void Deconstruct(out RoixPoint point, out RoixSize border) => (point, border) = (Point, Border);
        #endregion

        #region Equals
        public readonly bool Equals(RoixGaugePoint other) => (Point, Border) == (other.Point, other.Border);
        public readonly override bool Equals(object? obj) => (obj is RoixGaugePoint other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Point, Border);
        public static bool operator ==(in RoixGaugePoint left, in RoixGaugePoint right) => left.Equals(right);
        public static bool operator !=(in RoixGaugePoint left, in RoixGaugePoint right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixGaugePoint)} {{ {nameof(Point)} = {Point}, {nameof(Border)} = {Border} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixGaugePoint)} {{ ");
            sb.Append($"{nameof(Point)} = {Point.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Border)} = {Border.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region implicit
        #endregion

        #region explicit
        //public static explicit operator RoixGaugeVector(in RoixGaugePoint gaugePoint) => new((RoixVector)gaugePoint.Point, gaugePoint.Border);
        #endregion

        #region operator
        public static RoixGaugePoint operator +(in RoixGaugePoint gaugePoint, in RoixGaugeVector gaugeVector) => (gaugePoint.Border == gaugeVector.Border) ? new(gaugePoint.Point + gaugeVector.Vector, gaugePoint.Border) : throw new NotImplementedException("Border size is different.");

        public static RoixGaugePoint operator -(in RoixGaugePoint gaugePoint, in RoixGaugeVector gaugeVector) => (gaugePoint.Border == gaugeVector.Border) ? new(gaugePoint.Point - gaugeVector.Vector, gaugePoint.Border) : throw new NotImplementedException("Border size is different.");

        public static RoixGaugeVector operator -(in RoixGaugePoint gaugePoint1, in RoixGaugePoint gaugePoint2) => (gaugePoint1.Border == gaugePoint2.Border) ? new(gaugePoint1.Point - gaugePoint2.Point, gaugePoint1.Border) : throw new NotImplementedException("Border size is different.");
        #endregion

        #region Properties
        public readonly bool IsZero => this == Zero;
        public readonly bool IsInsideBorder => Point.X.IsInside(0, Border.Width) && Point.Y.IsInside(0, Border.Height);
        public readonly bool IsOutsideBorder => !IsInsideBorder;
        public readonly RoixPoint ClippedPoint => new(Math.Clamp(Point.X, 0, Border.Width), Math.Clamp(Point.Y, 0, Border.Height));
        #endregion

        #region Methods
        public readonly RoixGaugePoint ConvertToNewGauge(in RoixSize newBorder)
        {
            if (Border.IsInvalid) return this;
            if (newBorder.IsInvalid) throw new ArgumentException($"Invalid {nameof(newBorder)}");

            var newPoint = new RoixPoint(Point.X * newBorder.Width / Border.Width, Point.Y * newBorder.Height / Border.Height);
            return new(newPoint, newBorder);
        }

        public readonly RoixIntPoint ToRoixIntPoint(bool isCheckBoundaries = true)
        {
            if (isCheckBoundaries && IsOutsideBorder) throw new InvalidOperationException("must inside the border");

            var srcPoint = (RoixIntPoint)Point;
            var intSize = (RoixIntSize)Border;
            if (intSize.IsZero) throw new InvalidOperationException("size is zero");

            var x = Math.Clamp(srcPoint.X, 0, intSize.Width - 1);
            var y = Math.Clamp(srcPoint.Y, 0, intSize.Height - 1);
            return new(x, y);
        }

        public readonly RoixGaugeRect CreateRoixGaugeRect(in RoixVector vector) => new RoixGaugeRect(new RoixRect(Point, vector), Border);
        #endregion

    }
}
