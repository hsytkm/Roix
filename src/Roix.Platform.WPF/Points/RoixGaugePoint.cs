using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    public readonly struct RoixGaugePoint : IEquatable<RoixGaugePoint>
    {
        public readonly RoixPoint Point;
        public readonly RoixSize Canvas;

        #region ctor
        public RoixGaugePoint(in RoixPoint point, in RoixSize canvas)
        {
            if (canvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(canvas)}");
            (Point, Canvas) = (point, canvas);
        }

        public RoixGaugePoint(double x, double y, double width, double height) => (Point, Canvas) = (new RoixPoint(x, y), new RoixSize(width, height));

        public void Deconstruct(out RoixPoint point, out RoixSize canvas) => (point, canvas) = (Point, Canvas);
        #endregion

        #region Equals
        public bool Equals(RoixGaugePoint other) => (Point, Canvas) == (other.Point, other.Canvas);
        public override bool Equals(object? obj) => (obj is RoixGaugePoint other) && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Point, Canvas);
        public static bool operator ==(in RoixGaugePoint left, in RoixGaugePoint right) => left.Equals(right);
        public static bool operator !=(in RoixGaugePoint left, in RoixGaugePoint right) => !(left == right);
        #endregion

        public override string ToString() => $"{nameof(RoixGaugePoint)} {{ {nameof(Point)} = {Point}, {nameof(Canvas)} = {Canvas} }}";

        #region operator
        public static RoixGaugeRect Add(in RoixGaugePoint gaugePoint, in RoixVector vector) => new RoixGaugeRect(new RoixRect(gaugePoint.Point, vector), gaugePoint.Canvas);
        public static RoixGaugeRect operator +(in RoixGaugePoint gaugePoint, in RoixVector vector) => Add(gaugePoint, vector);
        public RoixGaugeRect Add(in RoixVector vector) => Add(this, vector);
        #endregion

        #region Properties
        public readonly RoixPoint ClippedRoi => new(Math.Clamp(Point.X, 0, Canvas.Width), Math.Clamp(Point.Y, 0, Canvas.Height));

        public readonly bool IsInside => Point.X.IsInside(0, Canvas.Width) && Point.Y.IsInside(0, Canvas.Height);
        public readonly bool IsOutside => !IsInside;
        #endregion

        #region Methods
        public readonly RoixGaugePoint ConvertToNewGauge(in RoixSize newSize)
        {
            if (Canvas.IsInvalid) throw new ArgumentException($"Invalid {nameof(Canvas)}");
            if (newSize.IsInvalid) throw new ArgumentException($"Invalid {nameof(newSize)}");

            var x = Point.X * newSize.Width / Canvas.Width;
            var y = Point.Y * newSize.Height / Canvas.Height;
            return new(new(x, y), newSize);
        }
        #endregion

    }
}
