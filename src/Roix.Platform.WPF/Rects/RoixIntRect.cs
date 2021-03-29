using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.ArithmeticOperator2)]
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
        public RoixIntRect(double x, double y, double width, double height) : this(new RoixIntPoint(x, y), new RoixIntSize(width, height)) { }

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
        // ◆Generatorに押し込みたい
        public static RoixIntRect operator +(in RoixIntRect rect, in RoixIntVector vector) => new(rect.Location + vector, rect.Size);
        public static RoixIntRect operator -(in RoixIntRect rect, in RoixIntVector vector) => rect + (-vector);
        #endregion

        #region Methods
        /// <summary>Roiの最小サイズを指定値で制限します</summary>
        //public RoixIntRect ClippedByMinimumSize(in RoixIntSize minSize) => new(Location, Size.ClippedByMinimumSize(minSize));

        /// <summary>引数で指定した座標系(int)に変換します</summary>
        public static RoixIntRect Create(in RoixRect srcRect, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode modeX, RoundingMode modeY)
        {
            if (srcSize.IsIncludeZero) throw new DivideByZeroException();

            var rect = srcRect * (destSize / srcSize);
            return rect.ToRoixInt(modeX, modeY);
        }

        /// <summary>引数で指定した座標系(int)に変換します</summary>
        public static RoixIntRect Create(in RoixRect srcRect, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode mode)
            => Create(srcRect, srcSize, destSize, mode, mode);

        /// <summary>指定 border の内部に収めた IntRect を返します</summary>
        public RoixIntRect GetClippedIntRect(in RoixIntSize border, bool isPointPriority = true)
            => isPointPriority ? GetClippedRectByPointPriority(border) : GetClippedRectBySizePriority(border);

        #endregion

    }
}
