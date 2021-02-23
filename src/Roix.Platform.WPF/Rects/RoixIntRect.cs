using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    public readonly struct RoixIntRect : IEquatable<RoixIntRect>
    {
        public static RoixIntRect Zero { get; } = new(0, 0, 0, 0);

        public readonly RoixIntPoint Location { get; }
        public readonly RoixIntSize Size { get; }

        #region ctor
        public RoixIntRect(int x, int y, int width, int height) => (Location, Size) = (new(x, y), new(width, height));
        public RoixIntRect(in RoixIntPoint point, in RoixIntSize size) => (Location, Size) = (point, size);

        public readonly void Deconstruct(out int x, out int y, out int width, out int height) => (x, y, width, height) = (Location.X, Location.Y, Size.Width, Size.Height);
        public readonly void Deconstruct(out RoixIntPoint point, out RoixIntSize size) => (point, size) = (Location, Size);
        #endregion

        #region Equals
        public readonly bool Equals(RoixIntRect other) => (Location, Size) == (other.Location, other.Size);
        public readonly override bool Equals(object? obj) => (obj is RoixIntRect other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Location, Size);
        public static bool operator ==(in RoixIntRect left, in RoixIntRect right) => left.Equals(right);
        public static bool operator !=(in RoixIntRect left, in RoixIntRect right) => !(left == right);
        #endregion

        public readonly override string ToString() => $"{nameof(RoixIntRect)} {{ {nameof(Location)} = {Location}, {nameof(Size)} = {Size} }}";

        #region implicit
        public static implicit operator RoixIntRect(in RoixRect rect) => !rect.IsEmpty ? new((RoixIntPoint)rect.Location, (RoixIntSize)rect.Size) : throw new ArgumentException("rect is empty");
        public static implicit operator RoixRect(in RoixIntRect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        public static implicit operator RoixIntRect(System.Windows.Rect rect) => !rect.IsEmpty ? new((RoixIntPoint)rect.Location, (RoixIntSize)rect.Size) : throw new ArgumentException("size is empty");
        public static implicit operator System.Windows.Rect(in RoixIntRect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);
        #endregion

        #region explicit
        #endregion

        #region operator
        #endregion

        #region Properties
        public readonly bool IsZero => this == Zero;

        public readonly int X => Location.X;
        public readonly int Y => Location.Y;
        public readonly int Width => Size.Width;
        public readonly int Height => Size.Height;
        public readonly int Left => Location.X;
        public readonly int Right => Location.X + Size.Width;
        public readonly int Top => Location.Y;
        public readonly int Bottom => Location.Y + Size.Height;
        public readonly RoixIntPoint TopLeft => Location;
        public readonly RoixIntPoint TopRight => new(Right, Top);
        public readonly RoixIntPoint BottomLeft => new(Left, Bottom);
        public readonly RoixIntPoint BottomRight => new(Right, Bottom);
        #endregion

    }
}
