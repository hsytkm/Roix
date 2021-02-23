﻿using System;
using System.Text;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Vector.cs
    public readonly struct RoixVector : IEquatable<RoixVector>, IFormattable
    {
        public static RoixVector Zero { get; } = new(0, 0);

        public readonly double X { get; }
        public readonly double Y { get; }

        #region ctor
        public RoixVector(double x, double y) => (X, Y) = (x, y);

        public readonly void Deconstruct(out double x, out double y) => (x, y) = (X, Y);
        #endregion

        #region Equals
        public readonly bool Equals(RoixVector other) => (X, Y) == (other.X, other.Y);
        public readonly override bool Equals(object? obj) => (obj is RoixVector other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(in RoixVector left, in RoixVector right) => left.Equals(right);
        public static bool operator !=(in RoixVector left, in RoixVector right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixVector)} {{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixVector)} {{ ");
            sb.Append($"{nameof(X)} = {X.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Y)} = {Y.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region implicit
        public static implicit operator RoixVector(System.Windows.Vector vector) => new(vector.X, vector.Y);
        public static implicit operator System.Windows.Vector(in RoixVector vector) => new(vector.X, vector.Y);

        public static explicit operator System.Windows.Point(in RoixVector vector) => new(vector.X, vector.Y);
        public static explicit operator RoixVector(System.Windows.Point point) => new(point.X, point.Y);

        public static explicit operator RoixPoint(in RoixVector vector) => new(vector.X, vector.Y);
        #endregion

        #region operator
        public static RoixVector operator +(in RoixVector vector1, in RoixVector vector2) => new(vector1.X + vector2.X, vector1.Y + vector2.Y);
        public static RoixVector operator -(in RoixVector vector) => new(-vector.X, -vector.Y);
        public static RoixVector operator -(in RoixVector vector1, in RoixVector vector2) => new(vector1.X - vector2.X, vector1.Y - vector2.Y);
        #endregion

        #region Properties
        public readonly bool IsZero => this == Zero;
        #endregion

    }
}
