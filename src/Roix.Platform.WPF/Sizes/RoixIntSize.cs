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
        public static explicit operator RoixIntPoint(in RoixIntSize size) => new(size.Width, size.Height);
        public static explicit operator RoixIntVector(in RoixIntSize size) => new(size.Width, size.Height);
        #endregion

        public bool IsEmpty => false;   // Not implemented in RoixIntSize

        /// <summary>Size の最小サイズを指定値で制限します</summary>
        public RoixIntSize ClippedByMinimum(in RoixIntSize minSize)
        {
            if (this.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            if (minSize.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);

            return new(Math.Max(Width, minSize.Width), Math.Max(Height, minSize.Height));
        }

        /// <summary>Size の最大サイズを指定値で制限します</summary>
        public RoixIntSize ClippedByMaximum(in RoixIntSize maxSize)
        {
            if (this.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            if (maxSize.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);

            return new(Math.Min(Width, maxSize.Width), Math.Min(Height, maxSize.Height));
        }

    }
}
