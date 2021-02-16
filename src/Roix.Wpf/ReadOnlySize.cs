using System;

namespace Roix.Wpf
{
    public readonly struct ReadOnlySize : IEquatable<ReadOnlySize>
    {
        public static ReadOnlySize Empty { get; } = new(true, double.NegativeInfinity, double.NegativeInfinity);

        public readonly bool IsEmpty;
        public readonly double Width;
        public readonly double Height;

        private ReadOnlySize(bool isEmpty, double width, double height) => (IsEmpty, Width, Height) = (isEmpty, width, height);

        public ReadOnlySize(double width, double height) => (IsEmpty, Width, Height) = (false, width, height);

        public void Deconstruct(out double width, out double height) => (width, height) = !IsEmpty ? (Width, Height) : (Empty.Width, Empty.Height);

        public static implicit operator ReadOnlySize(System.Windows.Size s) => !s.IsEmpty ? new(s.Width, s.Height) : Empty;
        public static implicit operator System.Windows.Size(in ReadOnlySize s) => !s.IsEmpty ? new(s.Width, s.Height) : System.Windows.Size.Empty;

        public bool Equals(ReadOnlySize other) => (Width, Height) == (other.Width, other.Height);
        public override bool Equals(object? obj) => (obj is ReadOnlySize other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Width, Height);
        public static bool operator ==(in ReadOnlySize left, in ReadOnlySize right) => left.Equals(right);
        public static bool operator !=(in ReadOnlySize left, in ReadOnlySize right) => !(left == right);
        public override string ToString() => $"{nameof(ReadOnlySize)} {{ {nameof(IsEmpty)} = {IsEmpty}, {nameof(Width)} = {Width}, {nameof(Height)} = {Height} }}";
    }
}
