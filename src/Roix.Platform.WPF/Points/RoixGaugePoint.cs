using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    public readonly struct RoixGaugePoint : IEquatable<RoixGaugePoint>
    {
        public readonly RoixPoint Roi;
        public readonly RoixSize Canvas;

        public RoixGaugePoint(in RoixPoint roi, in RoixSize canvas)
        {
            if (canvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(canvas)}");
            (Roi, Canvas) = (roi, canvas);
        }

        public RoixGaugePoint(double x, double y, double width, double height) => (Roi, Canvas) = (new RoixPoint(x, y), new RoixSize(width, height));

        public void Deconstruct(out RoixPoint roi, out RoixSize canvas) => (roi, canvas) = (Roi, Canvas);

        public bool Equals(RoixGaugePoint other) => (Roi, Canvas) == (other.Roi, other.Canvas);
        public override bool Equals(object? obj) => (obj is RoixGaugePoint other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Roi, Canvas);
        public static bool operator ==(in RoixGaugePoint left, in RoixGaugePoint right) => left.Equals(right);
        public static bool operator !=(in RoixGaugePoint left, in RoixGaugePoint right) => !(left == right);
        public override string ToString() => $"{nameof(RoixGaugePoint)} {{ {nameof(Roi)} = {Roi}, {nameof(Canvas)} = {Canvas} }}";

        public RoixGaugePoint ConvertToNewGauge(in RoixSize newSize)
        {
            if (Canvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(Canvas)}");
            if (newSize.IsInvalid) throw new ArgumentException($"Invalid {nameof(newSize)}");

            var x = Roi.X * newSize.Width / Canvas.Width;
            var y = Roi.Y * newSize.Height / Canvas.Height;
            return new(new RoixPoint(x, y), newSize);
        }

        public RoixPoint GetClippedRoi() => new(Roi.X.Clamp(0, Canvas.Width), Roi.Y.Clamp(0, Canvas.Height));

    }
}
