using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    public readonly struct RoixIntSize : IEquatable<RoixIntSize>
    {
        public static RoixIntSize Zero { get; } = new(0, 0);

        public readonly int Width { get; }
        public readonly int Height { get; }

        #region ctor
        public RoixIntSize(int width, int height)
        {
            if (width < 0 || height < 0) throw new ArgumentException("width and height cannot be negative value.");
            (Width, Height) = (width, height);
        }

        public readonly void Deconstruct(out int width, out int height) => (width, height) = (Width, Height);
        #endregion

        #region Equals
        public readonly bool Equals(RoixIntSize other) => (Width, Height) == (other.Width, other.Height);
        public readonly override bool Equals(object? obj) => (obj is RoixIntSize other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Width, Height);
        public static bool operator ==(in RoixIntSize left, in RoixIntSize right) => left.Equals(right);
        public static bool operator !=(in RoixIntSize left, in RoixIntSize right) => !(left == right);
        #endregion

        public readonly override string ToString() => $"{nameof(RoixIntSize)} {{ {nameof(Width)} = {Width}, {nameof(Height)} = {Height} }}";

        #region implicit
        public static implicit operator RoixIntSize(in RoixSize size) => !size.IsEmpty ? new(size.Width.RoundToInt(), size.Height.RoundToInt()) : throw new ArgumentException("size is empty");
        public static implicit operator RoixSize(in RoixIntSize size) => new(size.Width, size.Height);

        public static implicit operator RoixIntSize(System.Windows.Size size) => !size.IsEmpty ? new(size.Width.RoundToInt(), size.Height.RoundToInt()) : throw new ArgumentException("size is empty");
        public static implicit operator System.Windows.Size(in RoixIntSize size) => new(size.Width, size.Height);
        #endregion

        #region explicit
        #endregion

        #region operator
        #endregion

        #region Properties
        public readonly bool IsZero => this == Zero;
        #endregion

    }
}
