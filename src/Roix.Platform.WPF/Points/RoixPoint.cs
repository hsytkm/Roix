using System;
using System.Text;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Point.cs
    public readonly struct RoixPoint : IEquatable<RoixPoint>, IFormattable
    {
        public static RoixPoint Zero { get; } = new(0, 0);

        public readonly double X { get; }
        public readonly double Y { get; }

        #region ctor
        public RoixPoint(double x, double y) => (X, Y) = (x, y);

        public readonly void Deconstruct(out double x, out double y) => (x, y) = (X, Y);
        #endregion

        #region Equals
        public readonly bool Equals(RoixPoint other) => (X, Y) == (other.X, other.Y);
        public readonly override bool Equals(object? obj) => (obj is RoixPoint other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(in RoixPoint left, in RoixPoint right) => left.Equals(right);
        public static bool operator !=(in RoixPoint left, in RoixPoint right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixPoint)} {{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixPoint)} {{ ");
            sb.Append($"{nameof(X)} = {X.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Y)} = {Y.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region implicit
        public static implicit operator RoixPoint(System.Windows.Point point) => new(point.X, point.Y);
        public static implicit operator System.Windows.Point(in RoixPoint point) => new(point.X, point.Y);
        #endregion

        #region explicit
        //public static explicit operator RoixSize(in RoixPoint point) => new(point.X, point.Y);
        public static explicit operator RoixVector(in RoixPoint point) => new(point.X, point.Y);
        #endregion

        #region operator
        public static RoixPoint operator +(in RoixPoint point, in RoixVector vector) => new(point.X + vector.X, point.Y + vector.Y);
        public static RoixVector operator -(in RoixPoint point1, in RoixPoint point2) => new(point1.X - point2.X, point1.Y - point2.Y);
        public static RoixPoint operator -(in RoixPoint point, in RoixVector vector) => new(point.X - vector.X, point.Y - vector.Y);
        #endregion

        #region Properties
        public readonly bool IsZero => this == Zero;
        #endregion

        #region Methods
        public readonly bool IsInside(in RoixSize border) => 0 <= X && X <= border.Width && 0 <= Y && Y <= border.Height;
        public readonly bool IsOutside(in RoixSize border) => !IsInside(border);
        #endregion

    }
}
