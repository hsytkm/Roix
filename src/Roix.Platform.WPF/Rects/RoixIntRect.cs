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
        public RoixIntRect(int x, int y, int width, int height) : this(new RoixIntPoint(x, y), new RoixIntSize(width, height)) { }

        // ◆基本のctorにthisしたい
        public RoixIntRect(in RoixIntPoint point1, in RoixIntPoint point2)
        {
            var x = Math.Min(point1.X, point2.X);
            var y = Math.Min(point1.Y, point2.Y);
            var width = Math.Max(1, Math.Max(point1.X, point2.X) - x);
            var height = Math.Max(1, Math.Max(point1.Y, point2.Y) - y);
            _values = new SourceValues(new(x, y), new(width, height));    // exception will occur if the size is zero.
        }
        #endregion

        #region implicit
        public static implicit operator RoixRect(in RoixIntRect rect) => new(rect.Location, rect.Size);
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

        #region Methods
        /// <summary>Roiの最小サイズを指定値で制限する</summary>
        //public RoixIntRect ClippedByMinimumSize(in RoixIntSize minSize) => new(Location, Size.ClippedByMinimumSize(minSize));

        /// <summary>引数で指定した座標系(int)に変換する</summary>
        public static RoixIntRect Create(in RoixRect srcRect, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode modeX, RoundingMode modeY)
        {
            if (srcSize.IsIncludeZero) throw new DivideByZeroException();

            var rect = srcRect * (destSize / srcSize);
            return new RoixIntRect(new RoixIntPoint(rect.X.ToInt(modeX), rect.Y.ToInt(modeY)), new RoixIntSize(rect.Width.ToInt(modeX), rect.Height.ToInt(modeY)));
        }

        /// <summary>引数で指定した座標系(int)に変換する</summary>
        public static RoixIntRect Create(in RoixRect srcRect, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode mode)
            => Create(srcRect, srcSize, destSize, mode, mode);

        /// <summary>指定 border の内部に収めた IntRect を返す</summary>
        public RoixIntRect GetClippedIntRect(in RoixIntSize border, bool isPointPriority = true)
            => isPointPriority ? GetClippedRectByPointPriority(border) : GetClippedRectBySizePriority(border);

        #endregion

    }
}
