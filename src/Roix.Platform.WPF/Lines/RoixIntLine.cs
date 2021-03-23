using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.ArithmeticOperator2)]
    public readonly partial struct RoixIntLine
    {
        readonly struct SourceValues
        {
            public readonly RoixIntPoint Point1;
            public readonly RoixIntPoint Point2;
            public SourceValues(in RoixIntPoint point1, in RoixIntPoint point2) => (Point1, Point2) = (point1, point2);
        }

        #region ctor
        public RoixIntLine(int x1, int y1, int x2, int y2) : this(new RoixIntPoint(x1, y1), new RoixIntPoint(x2, y2)) { }

        public RoixIntLine(in RoixIntPoint point, in RoixIntVector vector) : this(point, point + vector) { }
        #endregion

        #region implicit
        public static implicit operator RoixLine(in RoixIntLine line) => new(line.Point1, line.Point2);
        #endregion

        #region operator
        //public static RoixIntPoint operator +(in RoixIntPoint point, in RoixIntVector vector) => new(point.X + vector.X, point.Y + vector.Y);
        //public static RoixIntVector operator -(in RoixIntPoint point1, in RoixIntPoint point2) => new(point1.X - point2.X, point1.Y - point2.Y);
        //public static RoixIntPoint operator -(in RoixIntPoint point, in RoixIntVector vector) => new(point.X - vector.X, point.Y - vector.Y);
        #endregion

        #region Properties
        ///// <summary>引数で指定したInt型の座標系に変換します</summary>
        //public static RoixIntPoint Create(in RoixPoint srcPoint, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode modeX, RoundingMode modeY)
        //{
        //    if (srcSize.IsIncludeZero) throw new DivideByZeroException();

        //    var point = srcPoint * (destSize / srcSize);
        //    return new(point.X.ToInt(modeX), point.Y.ToInt(modeY));
        //}

        ///// <summary>引数で指定したInt型の座標系に変換します</summary>
        //public static RoixIntPoint Create(in RoixPoint srcPoint, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode mode)
        //    => Create(srcPoint, srcSize, destSize, mode, mode);

        ///// <summary>引数で指定した IntSize 内に収めた IntPoint を返します</summary>
        //public RoixIntPoint GetClippedIntPoint(in RoixIntSize size)
        //{
        //    if (size.IsIncludeZero) throw new ArgumentException(ExceptionMessages.SizeIsZero);

        //    return new(Math.Clamp(X, 0, size.Width - 1), Math.Clamp(Y, 0, size.Height - 1));
        //}
        #endregion

    }
}
