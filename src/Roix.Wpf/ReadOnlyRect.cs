using System;

namespace Roix.Wpf
{
    public readonly struct ReadOnlyRect : IEquatable<ReadOnlyRect>
    {
        public static ReadOnlyRect Empty { get; } = new(true, double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);

        public readonly bool IsEmpty;
        public readonly double X;
        public readonly double Y;
        public readonly double Width;
        public readonly double Height;

        private ReadOnlyRect(bool isEmpty, double x, double y, double width, double height) => (IsEmpty, X, Y, Width, Height) = (isEmpty, x, y, width, height);
        public ReadOnlyRect(double x, double y, double width, double height) => (IsEmpty, X, Y, Width, Height) = (false, x, y, width, height);

        public void Deconstruct(out double x, out double y, out double width, out double height)
            => (x, y, width, height) = !IsEmpty ? (X, Y, Width, Height) : (Empty.X, Empty.Y, Empty.Width, Empty.Height);

        public static implicit operator ReadOnlyRect(System.Windows.Rect r) => !r.IsEmpty ? new(r.X, r.Y, r.Width, r.Height) : Empty;

        public static implicit operator System.Windows.Rect(in ReadOnlyRect r) => !r.IsEmpty ? new(r.X, r.Y, r.Width, r.Height) : System.Windows.Rect.Empty;

        public bool Equals(ReadOnlyRect other) => (X, Y, Width, Height) == (other.X, other.Y, other.Width, other.Height);
        public override bool Equals(object? obj) => (obj is ReadOnlyRect other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);
        public static bool operator ==(in ReadOnlyRect left, in ReadOnlyRect right) => left.Equals(right);
        public static bool operator !=(in ReadOnlyRect left, in ReadOnlyRect right) => !(left == right);
        public override string ToString() => $"{nameof(ReadOnlyRect)} {{ {nameof(IsEmpty)} = {IsEmpty}, {nameof(X)} = {X}, {nameof(Y)} = {Y}, {nameof(Width)} = {Width}, {nameof(Height)} = {Height} }}";
    }
}
