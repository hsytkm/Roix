using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixIntRect
    {
        readonly struct SourceValues
        {
            public readonly RoixIntPoint Location;
            public readonly RoixIntSize Size;
            public SourceValues(in RoixIntPoint point, in RoixIntSize size) => (Location, Size) = (point, size);
        }

        #region ctor
        public RoixIntRect(in RoixIntPoint point1, in RoixIntPoint point2)
        {
            var x = Math.Min(point1.X, point2.X);
            var y = Math.Min(point1.Y, point2.Y);
            var width = Math.Max(1, Math.Max(point1.X, point2.X) - x);
            var height = Math.Max(1, Math.Max(point1.Y, point2.Y) - y);
            _values = new SourceValues(new(x, y), new(width, height));    // exception will occur if the size is zero.
        }

        public RoixIntRect(int x, int y, int width, int height) => _values = new(new(x, y), new(width, height));
        public void Deconstruct(out int x, out int y, out int width, out int height) => (x, y, width, height) = (Location.X, Location.Y, Size.Width, Size.Height);
        #endregion

        #region implicit
        public static implicit operator RoixRect(in RoixIntRect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        public static implicit operator System.Windows.Rect(in RoixIntRect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);
        #endregion

        #region explicit
        public static explicit operator RoixIntRect(in RoixRect rect) => !rect.IsEmpty ? new((RoixIntPoint)rect.Location, (RoixIntSize)rect.Size) : throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        public static explicit operator RoixIntRect(System.Windows.Rect rect) => !rect.IsEmpty ? new((RoixIntPoint)rect.Location, (RoixIntSize)rect.Size) : throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        #endregion

        #region operator
        // RoixStructGeneratorOptions.ArithmeticOperator2
        public static RoixIntRect operator *(in RoixIntRect value, double scalar)
        {
            return new(value.Location * scalar, value.Size * scalar);
        }

        public static RoixIntRect operator /(in RoixIntRect value, double scalar)
        {
            if (scalar == 0) throw new DivideByZeroException();
            return new(value.Location / scalar, value.Size / scalar);
        }

        public static RoixIntRect operator *(in RoixIntRect value, in RoixRatioXY ratio)
        {
            return new(value.Location * ratio.X, value.Size * ratio.Y);
        }

        public static RoixIntRect operator /(in RoixIntRect value, in RoixRatioXY ratio)
        {
            if (ratio.IsIncludeZero) throw new DivideByZeroException();
            if (ratio.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
            return new(value.Location / ratio.X, value.Size / ratio.Y);
        }

        //public static RoixRatioXY operator /(in RoixIntRect value1, in RoixIntRect value2)
        //{
        //    if (value2.IsIncludeZero) throw new DivideByZeroException();
        //    return new(value1.Location / value2.Location, value1.Size / value2.Size);
        //}
        #endregion

        #region Properties
        #endregion

        #region Methods
        /// <summary>Roiの最小サイズを指定値で制限する</summary>
        public RoixIntRect ClippedByMinimumSize(in RoixIntSize minSize) => new(Location, Size.ClippedByMinimumSize(minSize));

        /// <summary>引数で指定した座標系(int)に変換する</summary>
        public static RoixIntRect Create(in RoixRect srcRect, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode mode)
        {
            var rect = srcRect * (destSize / srcSize);
            return new RoixIntRect(rect.X.ToInt(mode), rect.Y.ToInt(mode), rect.Width.ToInt(mode), rect.Height.ToInt(mode));
        }

        public RoixIntRect GetClippedIntRect(in RoixIntSize size)
        {
            // ◆ちゃんと実装してない
            var x = Math.Clamp(Location.X, 0, size.Width);
            var y = Math.Clamp(Location.Y, 0, size.Height);
            var width = Math.Clamp(Size.Width, 0, size.Width);
            var height = Math.Clamp(Size.Height, 0, size.Height);
            return new RoixIntRect(x, y, width, height);
        }

        #endregion

    }
}
