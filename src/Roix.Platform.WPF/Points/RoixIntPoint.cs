using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.ArithmeticOperator2)]
    public readonly partial struct RoixIntPoint
    {
        readonly struct SourceValues
        {
            public readonly int X;
            public readonly int Y;
            public SourceValues(int x, int y) => (X, Y) = (x, y);
        }

        #region ctor
        public RoixIntPoint(double x, double y) : this(x.FloorToInt(), y.FloorToInt()) { }
        #endregion

        #region implicit
        public static implicit operator RoixPoint(in RoixIntPoint point) => new(point.X, point.Y);

        #endregion

        #region explicit
        public static explicit operator RoixIntSize(in RoixIntPoint point) => new(point.X, point.Y);
        public static explicit operator RoixIntVector(in RoixIntPoint point) => new(point.X, point.Y);
        #endregion

        #region operator
        public static RoixIntPoint operator +(in RoixIntPoint point, in RoixIntVector vector) => new(point.X + vector.X, point.Y + vector.Y);
        public static RoixIntVector operator -(in RoixIntPoint point1, in RoixIntPoint point2) => new(point1.X - point2.X, point1.Y - point2.Y);
        public static RoixIntPoint operator -(in RoixIntPoint point, in RoixIntVector vector) => new(point.X - vector.X, point.Y - vector.Y);
        #endregion

        #region Properties
        /// <summary>引数で指定したInt型の座標系に変換します</summary>
        public static RoixIntPoint Create(in RoixPoint srcPoint, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode modeX, RoundingMode modeY)
        {
            if (srcSize.IsIncludeZero) throw new DivideByZeroException();

            var point = srcPoint * (destSize / srcSize);
            return new(point.X.ToInt(modeX), point.Y.ToInt(modeY));
        }

        /// <summary>引数で指定したInt型の座標系に変換します</summary>
        public static RoixIntPoint Create(in RoixPoint srcPoint, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode mode)
            => Create(srcPoint, srcSize, destSize, mode, mode);

        /// <summary>引数で指定した IntSize 内に収めた IntPoint を返します</summary>
        public RoixIntPoint GetClippedIntPoint(in RoixIntSize size)
        {
            if (size.IsIncludeZero) throw new ArgumentException(ExceptionMessages.SizeIsZero);

            return new(Math.Clamp(X, 0, size.Width - 1), Math.Clamp(Y, 0, size.Height - 1));
        }
        #endregion

    }
}
