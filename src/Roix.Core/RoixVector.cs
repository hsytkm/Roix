using System;

namespace Roix.Core
{
    public record RoixVector<T>(T X, T Y)where T : struct, IComparable<T>
    {
        private static readonly GenericOperation<T> _op = GenericOperation<T>.GetInstance();

        public void Deconstruct(out T x, out T y) => (x, y) = (X, Y);

        public static RoixVector<T> Add(RoixVector<T> left, RoixVector<T> right) => new(_op.Add(left.X, right.X), _op.Add(left.Y, right.Y));

        public static RoixVector<T> operator +(RoixVector<T> left, RoixVector<T> right) => Add(left, right);

    }

    public record RoixVectorInt : RoixVector<int>
    {
        public static RoixVectorInt Zero { get; } = new(0, 0);

        public RoixVectorInt(int x, int y) : base(x, y) { }

    }

    public record RoixVectorDouble : RoixVector<double>
    {
        public static RoixVectorDouble Zero { get; } = new(0, 0);

        public RoixVectorDouble(double x, double y) : base(x, y) { }
        public RoixVectorDouble(RoixVector<double> roix) : base(roix.X, roix.Y) { }

        public bool IsZero => this == Zero;

        public static RoixVectorDouble operator +(RoixVectorDouble left, RoixVectorDouble right) => new (Add(left, right));
    }

}
