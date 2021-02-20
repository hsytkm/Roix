using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Point.cs
    public readonly struct RoixPoint : IEquatable<RoixPoint>
    {
        public readonly double X;
        public readonly double Y;

        #region ctor
        public RoixPoint(double x, double y) => (X, Y) = (x, y);

        public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);
        #endregion

        #region Equals
        public bool Equals(RoixPoint other) => (X, Y) == (other.X, other.Y);
        public override bool Equals(object? obj) => (obj is RoixPoint other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(in RoixPoint left, in RoixPoint right) => left.Equals(right);
        public static bool operator !=(in RoixPoint left, in RoixPoint right) => !(left == right);
        #endregion

        public override string ToString() => $"{nameof(RoixPoint)} {{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";

        #region implicit
        public static implicit operator RoixPoint(System.Windows.Point point) => new(point.X, point.Y);
        public static implicit operator System.Windows.Point(in RoixPoint point) => new(point.X, point.Y);

        public static explicit operator RoixVector(in RoixPoint point) => new(point.X, point.Y);
        #endregion

        #region operator
        public static RoixPoint operator +(in RoixPoint point, in RoixVector vector) => new(point.X + vector.X, point.Y + vector.Y);
        public static RoixPoint operator -(in RoixPoint point, in RoixVector vector) => new(point.X - vector.X, point.Y - vector.Y);
        public static RoixVector operator -(in RoixPoint point1, in RoixPoint point2) => new(point1.X - point2.X, point1.Y - point2.Y);
        #endregion

        public RoixIntPoint ToRoixIntPoint() => new(X.RoundToInt(), Y.RoundToInt());

    }
}
