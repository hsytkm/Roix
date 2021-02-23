using System;
using System.Text;

namespace Roix.Wpf
{
    public readonly struct RoixGaugeSize : IEquatable<RoixGaugeSize>, IFormattable
    {
        public readonly RoixSize Size { get; }
        public readonly RoixSize Bounds { get; }

        #region ctor
        public RoixGaugeSize(in RoixSize size, in RoixSize bounds) => (Size, Bounds) = (size, bounds);

        public readonly void Deconstruct(out RoixSize size, out RoixSize bounds) => (size, bounds) = (Size, Bounds);
        #endregion

        #region Equals
        public readonly bool Equals(RoixGaugeSize other) => (Size, Bounds) == (other.Size, other.Bounds);
        public readonly override bool Equals(object? obj) => (obj is RoixGaugeSize other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Size, Bounds);
        public static bool operator ==(in RoixGaugeSize left, in RoixGaugeSize right) => left.Equals(right);
        public static bool operator !=(in RoixGaugeSize left, in RoixGaugeSize right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixGaugeSize)} {{ {nameof(Size)} = {Size}, {nameof(Bounds)} = {Bounds} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixGaugeSize)} {{ ");
            sb.Append($"{nameof(Size)} = {Size.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Bounds)} = {Bounds.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region Properties
        public readonly bool IsInsideInBounds => Size.IsInside(Bounds);
        public readonly bool IsOutsideInBounds => !IsInsideInBounds;
        #endregion

        #region Methods
        public readonly RoixGaugeSize ConvertToNewGauge(in RoixSize newBounds)
        {
            if (Bounds.IsInvalid) return this;
            if (newBounds.IsInvalid) throw new ArgumentException($"Invalid {nameof(newBounds)}");

            var newSize = new RoixSize(Size.Width * newBounds.Width / Bounds.Width, Size.Height * newBounds.Height / Bounds.Height);
            return new(newSize, newBounds);
        }
        #endregion

    }
}
