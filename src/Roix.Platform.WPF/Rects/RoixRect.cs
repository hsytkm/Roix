using System;
using System.Linq;
using System.Text;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Rect.cs
    public readonly struct RoixRect : IEquatable<RoixRect>, IFormattable
    {
        public static RoixRect Zero { get; } = new(0, 0, 0, 0);
        public static RoixRect Empty { get; } = new(new(double.PositiveInfinity, double.PositiveInfinity), RoixSize.Empty);

        public readonly RoixPoint Location { get; }
        public readonly RoixSize Size { get; }

        #region ctor
        public RoixRect(double x, double y, double width, double height) => (Location, Size) = (new(x, y), new(width, height));
        public RoixRect(in RoixPoint point, in RoixSize size) => (Location, Size) = (point, size);
        public RoixRect(in RoixPoint point1, in RoixPoint point2)
        {
            var x = Math.Min(point1.X, point2.X);
            var y = Math.Min(point1.Y, point2.Y);
            var width = Math.Max(point1.X, point2.X) - x;
            var height = Math.Max(point1.Y, point2.Y) - y;
            Location = new(x, y);
            Size = new(width, height);  // exception will occur if the size is zero.
        }
        public RoixRect(in RoixPoint point, in RoixVector vector) : this(point, point + vector) { }

        public readonly void Deconstruct(out double x, out double y, out double width, out double height) => (x, y, width, height) = (Location.X, Location.Y, Size.Width, Size.Height);
        public readonly void Deconstruct(out RoixPoint point, out RoixSize size) => (point, size) = (Location, Size);
        #endregion

        #region Equals
        public readonly bool Equals(RoixRect other) => (Location, Size) == (other.Location, other.Size);
        public readonly override bool Equals(object? obj) => (obj is RoixRect other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Location, Size);
        public static bool operator ==(in RoixRect left, in RoixRect right) => left.Equals(right);
        public static bool operator !=(in RoixRect left, in RoixRect right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixRect)} {{ {nameof(Location)} = {Location}, {nameof(Size)} = {Size} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixRect)} {{ ");
            sb.Append($"{nameof(Location)} = {Location.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Size)} = {Size.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region implicit
        public static implicit operator RoixRect(System.Windows.Rect rect) => !rect.IsEmpty ? new(rect.X, rect.Y, rect.Width, rect.Height) : Empty;
        public static implicit operator System.Windows.Rect(in RoixRect rect) => !rect.IsEmpty ? new(rect.X, rect.Y, rect.Width, rect.Height) : System.Windows.Rect.Empty;
        #endregion

        #region Properties
        public readonly bool IsZero => this == Zero;
        public readonly bool IsEmpty => this == Empty;

        public readonly double X => Location.X;
        public readonly double Y => Location.Y;
        public readonly double Width => Size.Width;
        public readonly double Height => Size.Height;
        public readonly double Left => Location.X;
        public readonly double Right => Location.X + Size.Width;
        public readonly double Top => Location.Y;
        public readonly double Bottom => Location.Y + Size.Height;
        public readonly RoixPoint TopLeft => Location;
        public readonly RoixPoint TopRight => new(Right, Top);
        public readonly RoixPoint BottomLeft => new(Left, Bottom);
        public readonly RoixPoint BottomRight => new(Right, Bottom);
        #endregion

        #region Methods
        public readonly bool IsInside(in RoixSize bounds) => 0 <= Left && Right <= bounds.Width && 0 <= Top && Bottom <= bounds.Height;
        public readonly bool IsOutside(in RoixSize bounds) => !IsInside(bounds);

        public readonly System.Windows.Media.PointCollection ToPointCollection()
        {
            if (IsEmpty) throw new ArgumentException("rect is empty.");
            return new(new[] { TopLeft, TopRight, BottomRight, BottomLeft }.Select(static x => (System.Windows.Point)x));
        }

        #endregion

    }
}
