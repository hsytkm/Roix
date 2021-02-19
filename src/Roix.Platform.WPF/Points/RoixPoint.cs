using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Point.cs
    public readonly struct RoixPoint : IEquatable<RoixPoint>
    {
        public readonly double X;
        public readonly double Y;

        public RoixPoint(double x, double y) => (X, Y) = (x, y);

        public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);

        public static implicit operator RoixPoint(System.Windows.Point p) => new(p.X, p.Y);
        public static implicit operator System.Windows.Point(in RoixPoint p) => new(p.X, p.Y);

        public bool Equals(RoixPoint other) => (X, Y) == (other.X, other.Y);
        public override bool Equals(object? obj) => (obj is RoixPoint other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(in RoixPoint left, in RoixPoint right) => left.Equals(right);
        public static bool operator !=(in RoixPoint left, in RoixPoint right) => !(left == right);
        public override string ToString() => $"{nameof(RoixPoint)} {{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";

        public RoixIntPoint ToRoixIntPoint() => new(X.RoundToInt(), Y.RoundToInt());
    }
}
