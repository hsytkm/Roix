using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    public readonly struct RoixIntPoint : IEquatable<RoixIntPoint>
    {
        public static RoixIntPoint Zero { get; } = new(0, 0);

        public readonly int X { get; }
        public readonly int Y { get; }

        #region ctor
        public RoixIntPoint(int x, int y) => (X, Y) = (x, y);

        public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
        #endregion

        #region Equals
        public bool Equals(RoixIntPoint other) => (X, Y) == (other.X, other.Y);
        public override bool Equals(object? obj) => (obj is RoixIntPoint other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(in RoixIntPoint left, in RoixIntPoint right) => left.Equals(right);
        public static bool operator !=(in RoixIntPoint left, in RoixIntPoint right) => !(left == right);
        #endregion

        public override string ToString() => $"{nameof(RoixIntPoint)} {{ {nameof(X)} = {X}, {nameof(Y)} = {Y} }}";

        #region implicit
        public static implicit operator RoixIntPoint(in RoixPoint point) => new(point.X.RoundToInt(), point.Y.RoundToInt());
        public static implicit operator RoixPoint(in RoixIntPoint point) => new(point.X, point.Y);

        public static implicit operator RoixIntPoint(System.Windows.Point point) => new(point.X.RoundToInt(), point.Y.RoundToInt());
        public static implicit operator System.Windows.Point(in RoixIntPoint point) => new(point.X, point.Y);
        #endregion

        #region Properties
        public readonly bool IsZero => this == Zero;
        #endregion

    }
}
