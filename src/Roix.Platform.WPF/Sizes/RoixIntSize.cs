using System;

namespace Roix.Wpf
{
    public readonly struct RoixIntSize : IEquatable<RoixIntSize>
    {
        public readonly int Width;
        public readonly int Height;

        #region ctor
        public RoixIntSize(int width, int height) => (Width, Height) = (width, height);

        public void Deconstruct(out int width, out int height) => (width, height) = (Width, Height);
        #endregion

        #region Equals
        public bool Equals(RoixIntSize other) => (Width, Height) == (other.Width, other.Height);
        public override bool Equals(object? obj) => (obj is RoixIntSize other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Width, Height);
        public static bool operator ==(in RoixIntSize left, in RoixIntSize right) => left.Equals(right);
        public static bool operator !=(in RoixIntSize left, in RoixIntSize right) => !(left == right);
        #endregion

        public override string ToString() => $"{nameof(RoixIntSize)} {{ {nameof(Width)} = {Width}, {nameof(Height)} = {Height} }}";
    }
}
