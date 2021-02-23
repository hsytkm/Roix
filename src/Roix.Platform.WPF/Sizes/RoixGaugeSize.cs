using System;
using System.Text;

namespace Roix.Wpf
{
    public readonly struct RoixGaugeSize : IEquatable<RoixGaugeSize>, IFormattable
    {
        public readonly RoixSize Size { get; }
        public readonly RoixSize Canvas { get; }

        #region ctor
        public RoixGaugeSize(in RoixSize size, in RoixSize canvas) => (Size, Canvas) = (size, canvas);

        public void Deconstruct(out RoixSize size, out RoixSize canvas) => (size, canvas) = (Size, Canvas);
        #endregion

        #region Equals
        public bool Equals(RoixGaugeSize other) => (Size, Canvas) == (other.Size, other.Canvas);
        public override bool Equals(object? obj) => (obj is RoixGaugeSize other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Size, Canvas);
        public static bool operator ==(in RoixGaugeSize left, in RoixGaugeSize right) => left.Equals(right);
        public static bool operator !=(in RoixGaugeSize left, in RoixGaugeSize right) => !(left == right);
        #endregion

        #region ToString
        public override string ToString() => $"{nameof(RoixGaugeSize)} {{ {nameof(Size)} = {Size}, {nameof(Canvas)} = {Canvas} }}";
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append($"{nameof(RoixGaugeSize)} {{ ");
            sb.Append($"{nameof(Size)} = {Size.ToString(format, formatProvider)}, ");
            sb.Append($"{nameof(Canvas)} = {Canvas.ToString(format, formatProvider)} }}");
            return sb.ToString();
        }
        #endregion

        #region Properties
        public readonly bool IsInsideInCanvas => Size.IsInside(Canvas);
        public readonly bool IsOutsideInCanvas => !IsInsideInCanvas;
        #endregion

        #region Methods
        public readonly RoixGaugeSize ConvertToNewGauge(in RoixSize newCanvas)
        {
            if (Canvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(Canvas)}");
            if (newCanvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(newCanvas)}");

            var newSize = new RoixSize(Size.Width * newCanvas.Width / Canvas.Width, Size.Height * newCanvas.Height / Canvas.Height);
            return new(newSize, newCanvas);
        }
        #endregion

    }
}
