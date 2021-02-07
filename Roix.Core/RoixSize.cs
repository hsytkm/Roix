using Roix.Core.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace Roix.Core
{
    //public interface IRoixSize<T> where T : struct, IComparable<T>
    //{
    //    public T Width { get; }
    //    public T Height { get; }
    //}

    public record RoixSize<T> /*: IRoixSize<T>*/ where T : struct, IComparable<T>
    {
        public T Width
        {
            get => _width;
            init
            {
                if (IsNegative(value)) throw new ArgumentException(Messages.Size_WidthAndHeightCannotBeNegative);
                _width = value;
            }
        }
        private T _width;

        public T Height
        {
            get => _height;
            init
            {
                if (IsNegative(value)) throw new ArgumentException(Messages.Size_WidthAndHeightCannotBeNegative);
                _height = value;
            }
        }
        private T _height;

        public RoixSize(T width, T height)
        {
            if (IsNegative(width) || IsNegative(height)) throw new ArgumentException(Messages.Size_WidthAndHeightCannotBeNegative);
            (Width, Height) = (width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNegative(T value) => value.CompareTo(default) < 0;

        public void Deconstruct(out T width, out T height) => (width, height) = (Width, Height);

        public virtual RoixSize<T> ReverseLength() => throw new NotImplementedException();
    }

    static class RoixSizeExtension
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static (T width, T height) GetReverseLength<T>(this RoixSize<T> size) where T : struct, IComparable<T>
        //    => (size.Height, size.Width);
    }

    public record RoixSizeInt : RoixSize<int>
    {
        //public static RoixSizeInt Zero { get; } = new(0, 0);

        public RoixSizeInt(int width, int height) : base(width, height) { }

        public static explicit operator RoixSizeDouble(RoixSizeInt s) => new(s.Width, s.Height);
        public static explicit operator RoixPointInt(RoixSizeInt s) => new(s.Width, s.Height);

        public override RoixSizeInt ReverseLength() => new(Height, Width);
    }

    public record RoixSizeDouble : RoixSize<double>
    {
        //public static RoixSizeDouble Zero { get; } = new(0, 0);

        public RoixSizeDouble(double width, double height) : base(width, height) { }

        public static explicit operator RoixSizeInt(RoixSizeDouble s) => new(MathExtension.RoundToInt(s.Width), MathExtension.RoundToInt(s.Height));
        public static explicit operator RoixPointDouble(RoixSizeDouble s) => new(s.Width, s.Height);

        public override RoixSizeDouble ReverseLength() => new(Height, Width);
    }

}
