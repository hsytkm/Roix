using Roix.Core.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace Roix.Core
{
    //public interface IRoixPoint<T> where T : struct, IComparable<T>
    //{
    //    public T X { get; }
    //    public T Y { get; }
    //}

    public record RoixPoint<T>(T X, T Y)　/*: IRoixPoint<T>*/ where T : struct, IComparable<T>
    {
        public void Deconstruct(out T x, out T y) => (x, y) = (X, Y);
    }

    static class RoixPointExtension
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static (T x, T y) GetReverse<T>(this RoixPoint<T> size) where T : struct, IComparable<T>
        //    => (size.Y, size.X);    // ◆この実装で良いのか？
    }

    public record RoixPointInt : RoixPoint<int>
    {
        public static RoixPointInt Zero { get; } = new(0, 0);

        public RoixPointInt(int x, int y) : base(x, y) { }

        public static explicit operator RoixPointDouble(RoixPointInt p) => new(p.X, p.Y);
        public static explicit operator RoixSizeInt(RoixPointInt p) => new(p.X, p.Y);

        public RoixPointInt GetReverse() => new(Y, X);
    }

    public record RoixPointDouble : RoixPoint<double>
    {
        public static RoixPointDouble Zero { get; } = new(0, 0);

        public RoixPointDouble(double x, double y) : base(x, y) { }

        public static explicit operator RoixPointInt(RoixPointDouble p) => new(MathExtension.RoundToInt(p.X), MathExtension.RoundToInt(p.Y));
        public static explicit operator RoixSizeDouble(RoixPointDouble p) => new(p.X, p.Y);

        public RoixPointDouble GetReverse() => new(Y, X);
    }

}
