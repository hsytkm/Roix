using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{

    [RoixStructGenerator(RoixStructGeneratorOptions.ArithmeticOperator2)]
    public readonly partial struct RoixLine
    {
        readonly struct SourceValues
        {
            public readonly RoixPoint Point1;
            public readonly RoixPoint Point2;
            public SourceValues(in RoixPoint point1, in RoixPoint point2) => (Point1, Point2) = (point1, point2);
        }

        #region ctor
        public RoixLine(double x1, double y1, double x2, double y2) : this(new RoixPoint(x1, y1), new RoixPoint(x2, y2)) { }

        public RoixLine(in RoixPoint point, in RoixVector vector) : this(point, point + vector) { }
        #endregion

        #region operator
        //public static RoixRect operator *(in RoixRect rect, double scalar)
        //{
        //    if (rect.IsEmpty) return Empty;
        //    if (scalar < 0) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
        //    return new(rect.Location * scalar, rect.Size * scalar);
        //}

        //public static RoixRect operator /(in RoixRect rect, double scalar)
        //{
        //    if (rect.IsEmpty) return Empty;
        //    if (scalar == 0) throw new DivideByZeroException();
        //    if (scalar < 0) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
        //    return rect * (1d / scalar);
        //}

        //public static RoixRect operator *(in RoixRect rect, in RoixRatioXY ratio)
        //{
        //    if (rect.IsEmpty) return Empty;
        //    if (ratio.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
        //    return new(rect.Location * ratio, rect.Size * ratio);
        //}

        //public static RoixRect operator /(in RoixRect rect, in RoixRatioXY ratio)
        //{
        //    if (ratio.IsIncludeZero) throw new DivideByZeroException();
        //    return rect * (1d / ratio);
        //}
        #endregion

        #region Methods

        ///// <summary>指定 border の内部に収めた IntRect を返します</summary>
        //public RoixRect GetClippedRect(in RoixSize border, bool isPointPriority = true)
        //    => isPointPriority ? GetClippedRectByPointPriority(border) : GetClippedRectBySizePriority(border);

        ///// <summary>Rect の最小サイズを指定値で制限します</summary>
        //public RoixRect ClippedSizeByMinimum(in RoixSize minSize) => new(Location, Size.ClippedByMinimum(minSize));

        ///// <summary>Rect の最大サイズを指定値で制限します</summary>
        //public RoixRect ClippedSizeByMaximum(in RoixSize maxSize) => new(Location, Size.ClippedByMaximum(maxSize));

        /// <summary>引数で指定した RoundingMode で Int型に変換します</summary>
        public RoixIntLine ToInt(RoundingMode modeX, RoundingMode modeY) => new(Point1.ToInt(modeX, modeY), Point2.ToInt(modeX, modeY));

        #endregion

    }
}
