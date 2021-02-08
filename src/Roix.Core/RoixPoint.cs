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
        private static readonly GenericOperation<T> _op = GenericOperation<T>.GetInstance();

        public void Deconstruct(out T x, out T y) => (x, y) = (X, Y);

        public static RoixSize<T> Add(RoixPoint<T> point, RoixVector<T> vector) => new(_op.Add(point.X, vector.X), _op.Add(point.Y, vector.Y));
        public static RoixVector<T> Add(RoixPoint<T> left, RoixPoint<T> right) => new(_op.Add(left.X, right.X), _op.Add(left.Y, right.Y));
        public static RoixPoint<T> Subtract(RoixPoint<T> point, RoixVector<T> vector) => new(_op.Subtract(point.X, vector.X), _op.Subtract(point.Y, vector.Y));
        public static RoixVector<T> Subtract(RoixPoint<T> left, RoixPoint<T> right) => new(_op.Subtract(left.X, right.X), _op.Subtract(left.Y, right.Y));

        //public static RoixPoint<T> operator +(RoixPoint<T> point, RoixVector<T> vector) => Add(point, vector);
        //public static RoixPoint<T> operator -(RoixPoint<T> point, RoixVector<T> vector) => Subtract(point, vector);
        //public static RoixVector<T> operator -(RoixPoint<T> left, RoixPoint<T> right) => Subtract(left, right);
    }

    static class RoixPointExtension
    {

    }

    public record RoixPointInt : RoixPoint<int>
    {
        public static RoixPointInt Zero { get; } = new(0, 0);

        public RoixPointInt(int x, int y) : base(x, y) { }

        public static explicit operator RoixPointDouble(RoixPointInt p) => new(p.X, p.Y);
        public static explicit operator RoixSizeInt(RoixPointInt p) => new(p.X, p.Y);

    }

    public record RoixPointDouble : RoixPoint<double>
    {
        public static RoixPointDouble Zero { get; } = new(0, 0);

        public RoixPointDouble(double x, double y) : base(x, y) { }
        public RoixPointDouble(RoixPoint<double> roix) : base(roix.X, roix.Y) { }

        public static explicit operator RoixPointInt(RoixPointDouble p) => new(MathExtension.RoundToInt(p.X), MathExtension.RoundToInt(p.Y));
        public static explicit operator RoixSizeDouble(RoixPointDouble p) => new(p.X, p.Y);

        public static RoixSizeDouble operator +(RoixPointDouble left, RoixVectorDouble right) => new(Add(left, right));
        public static RoixVectorDouble operator -(RoixPointDouble left, RoixPointDouble right) => new(Subtract(left, right));
    }

}
