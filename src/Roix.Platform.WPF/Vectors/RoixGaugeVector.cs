using Roix.Wpf.Internals;
using System;
using System.Text;

namespace Roix.Wpf
{
    public readonly struct RoixGaugeVector : IEquatable<RoixGaugeVector>, IFormattable
    {
        public static RoixGaugeVector Zero { get; } = new(RoixVector.Zero, RoixSize.Zero);

        public readonly RoixVector Vector { get; }
        public readonly RoixSize Border { get; }

        #region ctor
        public RoixGaugeVector(in RoixVector vector, in RoixSize border)
        {
            if (border.IsEmpty) throw new ArgumentException($"{nameof(border)} is empty");
            (Vector, Border) = (vector, border);
        }

        public RoixGaugeVector(double x, double y, double width, double height) => (Vector, Border) = (new(x, y), new(width, height));

        public readonly void Deconstruct(out RoixVector vector, out RoixSize border) => (vector, border) = (Vector, Border);
        #endregion

        #region Equals
        public readonly bool Equals(RoixGaugeVector other) => (Vector, Border) == (other.Vector, other.Border);
        public readonly override bool Equals(object? obj) => (obj is RoixGaugeVector other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Vector, Border);
        public static bool operator ==(in RoixGaugeVector left, in RoixGaugeVector right) => left.Equals(right);
        public static bool operator !=(in RoixGaugeVector left, in RoixGaugeVector right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixGaugeVector)} {{ {nameof(Vector)} = {Vector}, {nameof(Border)} = {Border} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixGaugeVector)} {{ ");
            sb.Append($"{nameof(Vector)} = {Vector.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Border)} = {Border.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region implicit
        #endregion

        #region explicit
        //public static explicit operator RoixGaugePoint(in RoixGaugeVector gaugeVector) => new((RoixPoint)gaugeVector.Vector, gaugeVector.Border);
        #endregion

        #region operator
        #endregion

        #region Properties
        public readonly bool IsZero => this == Zero;
        public readonly bool IsInsideBorder => Vector.X.IsInside(0, Border.Width) && Vector.Y.IsInside(0, Border.Height);
        public readonly bool IsOutsideBorder => !IsInsideBorder;
        #endregion

        #region Methods
        public readonly RoixGaugeVector ConvertToNewGauge(in RoixSize newBorder)
        {
            if (Border.IsInvalid) return this;
            if (newBorder.IsInvalid) throw new ArgumentException($"Invalid {nameof(newBorder)}");

            var newVector = new RoixVector(Vector.X * newBorder.Width / Border.Width, Vector.Y * newBorder.Height / Border.Height);
            return new(newVector, newBorder);
        }
        #endregion

    }
}
