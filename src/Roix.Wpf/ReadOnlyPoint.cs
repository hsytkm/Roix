using System;

namespace Roix.Wpf
{
    public readonly struct ReadOnlyPoint : IEquatable<ReadOnlyPoint>
    {
        public readonly double X;
        public readonly double Y;

        public ReadOnlyPoint(double x, double y) => (X, Y) = (x, y);

        public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);

        public static implicit operator ReadOnlyPoint(System.Windows.Point p) => new(p.X, p.Y);
        public static implicit operator System.Windows.Point(in ReadOnlyPoint p) => new(p.X, p.Y);

        public bool Equals(ReadOnlyPoint other) => (X, Y) == (other.X, other.Y);
        public override bool Equals(object? obj) => (obj is ReadOnlyPoint other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(in ReadOnlyPoint left, in ReadOnlyPoint right) => left.Equals(right);
        public static bool operator !=(in ReadOnlyPoint left, in ReadOnlyPoint right) => !(left == right);
        public override string ToString() => $"{nameof(ReadOnlyPoint)} {{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";
    }
}
