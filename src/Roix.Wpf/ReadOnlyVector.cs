using System;

namespace Roix.Wpf
{
    public readonly struct ReadOnlyVector : IEquatable<ReadOnlyVector>
    {
        public readonly double X;
        public readonly double Y;

        public ReadOnlyVector(double x, double y) => (X, Y) = (x, y);

        public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);

        public static implicit operator ReadOnlyVector(System.Windows.Vector s) => new(s.X, s.Y);
        public static implicit operator System.Windows.Vector(in ReadOnlyVector s) => new(s.X, s.Y);

        public bool Equals(ReadOnlyVector other) => (X, Y) == (other.X, other.Y);
        public override bool Equals(object? obj) => (obj is ReadOnlyVector other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(in ReadOnlyVector left, in ReadOnlyVector right) => left.Equals(right);
        public static bool operator !=(in ReadOnlyVector left, in ReadOnlyVector right) => !(left == right);
        public override string ToString() => $"{nameof(ReadOnlyVector)} {{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";
    }
}
