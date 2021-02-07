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

        public RoixPoint<T> LeffTop => new(Left, Top);
        public RoixPoint<T> RightTop => new(Right, Top);
        public RoixPoint<T> RightBottom => new(Right, Bottom);
        public RoixPoint<T> LeftBottom => new(Left, Bottom);

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
    }

    public record RoixRectDouble : RoixRect<double>
    {
        public RoixRectDouble(RoixSizeDouble size) : base(RoixPointDouble.Zero, size) { }
        public RoixRectDouble(double x, double y, double width, double height) : base(x, y, width, height) { }
    }

}
