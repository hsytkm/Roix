using Roix.Core.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace Roix.Core
{
    public record RoixRect<T>(RoixPoint<T> Point, RoixSize<T> Size) where T : struct, IComparable<T>
    {
        private static readonly GenericOperation<T> _op = GenericOperation<T>.GetInstance();

        public RoixRect(T x, T y, T width, T height) : this(new RoixPoint<T>(x, y), new RoixSize<T>(width, height)) { }

        public T X => Point.X;
        public T Y => Point.Y;
        public T Width => Size.Width;
        public T Height => Size.Height;

        public T Left => X;
        public T Right => _op.Add(X, Width);
        public T Top => Y;
        public T Bottom => _op.Add(Y, Height);

        public virtual RoixPoint<T> TopLeft => new(Left, Top);
        public virtual RoixPoint<T> TopRight => new(Right, Top);
        public virtual RoixPoint<T> BottomRight => new(Right, Bottom);
        public virtual RoixPoint<T> BottomLeft => new(Left, Bottom);

        public void Deconstruct(out T x, out T y, out T width, out T height) => (x, y, width, height) = (X, Y, Width, Height);
    }

    static class RoixRectExtension
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    }

    public record RoixRectInt : RoixRect<int>
    {
        public RoixRectInt(RoixSizeInt size) : base(RoixPointInt.Zero, size) { }
        public RoixRectInt(int x, int y, int width, int height) : base(x, y, width, height) { }

        public override string ToString() => $"{nameof(RoixRectInt)} {{ ({nameof(X)}, {nameof(Y)}, {nameof(Width)}, {nameof(Height)}) = ({Point.X}, {Point.Y}, {Size.Width}, {Size.Height}) }}";
    }

    public record RoixRectDouble : RoixRect<double>
    {
        public static RoixRectDouble Zero { get; } = new(0, 0, 0, 0);

        public RoixRectDouble(RoixRect<double> roix) : base(roix.X, roix.Y, roix.Width, roix.Height) { }
        public RoixRectDouble(RoixSizeDouble size) : base(RoixPointDouble.Zero, size) { }
        public RoixRectDouble(RoixPointDouble point1, RoixPointDouble point2) : base(RoixPointDouble.Zero, RoixSizeDouble.Zero)
        {
            var x = Math.Min(point1.X, point2.X);
            var y = Math.Min(point1.Y, point2.Y);
            Point = new(x, y);

            //  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
            var width = Math.Max(Math.Max(point1.X, point2.X) - x, 0);
            var height = Math.Max(Math.Max(point1.Y, point2.Y) - y, 0);
            Size = new(width, height);
        }
        public RoixRectDouble(RoixPointDouble point, RoixVectorDouble vector) : this(point, (RoixPointDouble)(point + vector)) { }
        public RoixRectDouble(double x, double y, double width, double height) : base(x, y, width, height) { }

        public override RoixPointDouble TopLeft => new(base.TopLeft);
        public override RoixPointDouble TopRight => new(base.TopRight);
        public override RoixPointDouble BottomRight => new(base.BottomRight);
        public override RoixPointDouble BottomLeft => new(base.BottomLeft);

        public override string ToString() => $"{nameof(RoixRectDouble)} {{ ({nameof(X)}, {nameof(Y)}, {nameof(Width)}, {nameof(Height)}) = ({Point.X}, {Point.Y}, {Size.Width}, {Size.Height}) }}";
    }

}
