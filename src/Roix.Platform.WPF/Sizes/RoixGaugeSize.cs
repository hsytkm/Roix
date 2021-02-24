using System;
using System.Text;

namespace Roix.Wpf
{
    public readonly struct RoixGaugeSize : IEquatable<RoixGaugeSize>, IFormattable
    {
        public static RoixGaugeSize Zero { get; } = new(RoixSize.Zero, RoixSize.Zero);

        public readonly RoixSize Size { get; }
        public readonly RoixSize Border { get; }

        #region ctor
        public RoixGaugeSize(in RoixSize size, in RoixSize border)
        {
            if (border.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            (Size, Border) = (size, border);
        }

        public readonly void Deconstruct(out RoixSize size, out RoixSize border) => (size, border) = (Size, Border);
        #endregion

        #region Equals
        public readonly bool Equals(RoixGaugeSize other) => (Size, Border) == (other.Size, other.Border);
        public readonly override bool Equals(object? obj) => (obj is RoixGaugeSize other) && Equals(other);
        public readonly override int GetHashCode() => HashCode.Combine(Size, Border);
        public static bool operator ==(in RoixGaugeSize left, in RoixGaugeSize right) => left.Equals(right);
        public static bool operator !=(in RoixGaugeSize left, in RoixGaugeSize right) => !(left == right);
        #endregion

        #region ToString
        public readonly override string ToString() => $"{nameof(RoixGaugeSize)} {{ {nameof(Size)} = {Size}, {nameof(Border)} = {Border} }}";
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixGaugeSize)} {{ ");
            sb.Append($"{nameof(Size)} = {Size.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Border)} = {Border.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region implicit
        #endregion

        #region explicit
        #endregion

        #region operator
        //public static RoixGaugeSize operator *(in RoixGaugeSize gaugeSize, double mul) => new(gaugeSize.Size * mul, gaugeSize.Border);

        //public static RoixGaugeSize operator /(in RoixGaugeSize gaugeSize, double div) => (div != 0) ? new(gaugeSize.Size / div, gaugeSize.Border) : throw new DivideByZeroException();
        #endregion

        #region Properties
        public readonly bool IsZero => this == Zero;
        public readonly bool IsInsideBorder => Size.IsInside(Border);
        public readonly bool IsOutsideBorder => !IsInsideBorder;
        #endregion

        #region Methods
        public readonly RoixGaugeSize ConvertToNewGauge(in RoixSize newBorder)
        {
            if (Border.IsInvalid) return this;
            if (newBorder.IsInvalid) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);

            var newSize = new RoixSize(Size.Width * newBorder.Width / Border.Width, Size.Height * newBorder.Height / Border.Height);
            return new(newSize, newBorder);
        }

        public readonly RoixIntSize ToRoixIntSize(bool isCheckBoundaries = true)
        {
            if (isCheckBoundaries && IsOutsideBorder) throw new InvalidOperationException(ExceptionMessages.MustInsideTheBorder);

            var srcSize = (RoixIntSize)Size;
            var intSize = (RoixIntSize)Border;
            if (intSize.IsZero) throw new InvalidOperationException(ExceptionMessages.SizeIsZero);

            var width = Math.Clamp(srcSize.Width, 0, intSize.Width - 1);
            var height = Math.Clamp(srcSize.Height, 0, intSize.Height - 1);
            return new(width, height);
        }
        #endregion

    }
}
