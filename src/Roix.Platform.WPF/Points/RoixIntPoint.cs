using System;

namespace Roix.Wpf
{
    public readonly struct RoixIntPoint : IEquatable<RoixIntPoint>
    {
        public readonly int X;
        public readonly int Y;

        public RoixIntPoint(int x, int y) => (X, Y) = (x, y);

        public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

        public bool Equals(RoixIntPoint other) => (X, Y) == (other.X, other.Y);
        public override bool Equals(object? obj) => (obj is RoixIntPoint other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(in RoixIntPoint left, in RoixIntPoint right) => left.Equals(right);
        public static bool operator !=(in RoixIntPoint left, in RoixIntPoint right) => !(left == right);
        public override string ToString() => $"{nameof(RoixIntPoint)} {{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";
    }
}
