using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate | RoixStructGeneratorOptions.ArithmeticOperator2)]
    public readonly partial struct RoixIntSize
    {
        readonly struct SourceValues
        {
            public readonly int Width;
            public readonly int Height;
            public SourceValues(int width, int height) => (Width, Height) = (width, height);
        }

        private double X => Width;
        private double Y => Height;

        #region ctor
        public RoixIntSize(int length) : this(length, length) { }
        public RoixIntSize(double x, double y) : this(x.FloorToInt(), y.FloorToInt()) { }

        private partial void Validate(in RoixIntSize value)
        {
            if (value.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
        }
        #endregion

        #region implicit
        public static implicit operator RoixSize(in RoixIntSize size) => new(size.Width, size.Height);
        #endregion

        #region explicit
        public static explicit operator RoixIntSize(in RoixSize size) => !size.IsEmpty ? new(size.Width.FloorToInt(), size.Height.FloorToInt()) : throw new ArgumentException(ExceptionMessages.SizeIsEmpty);

        public static implicit operator RoixIntPoint(in RoixIntSize size) => new(size.Width, size.Height);
        public static implicit operator RoixIntVector(in RoixIntSize size) => new(size.Width, size.Height);
        #endregion

        public bool IsEmpty => false;   // Not implement in RoixIntSize

        ///// <summary>指定値でサイズを制限する</summary>
        //public RoixIntSize ClippedByMinimumSize(in RoixIntSize minSize)
        //{
        //    if (this.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        //    if (minSize.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);

        //    var width = Math.Max(Width, minSize.Width);
        //    var height = Math.Max(Height, minSize.Height);
        //    return new(width, height);
        //}

    }
}
