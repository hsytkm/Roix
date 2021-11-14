using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderLine
    {
        readonly struct SourceValues
        {
            public readonly RoixLine Line;
            public readonly RoixSize Border;
            public SourceValues(in RoixLine line, in RoixSize border) => (Line, Border) = (line, border);
        }

        internal RoixLine Value => Line;

        #region ctor
        public RoixBorderLine(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2)
            : this(new RoixLine(borderPoint1.Point, borderPoint2.Point), borderPoint1.Border)
        {
            if (borderPoint1.Border != borderPoint2.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            if (borderPoint1.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }

        public RoixBorderLine(in RoixBorderPoint borderPoint, in RoixPoint point)
            : this(new RoixLine(borderPoint.Point, point), borderPoint.Border) { }
        #endregion

        #region operator
        public static RoixBorderLine operator +(in RoixBorderLine borderLine, in RoixVector vector) => new(borderLine.Line + vector, borderLine.Border);
        public static RoixBorderLine operator -(in RoixBorderLine borderLine, in RoixVector vector) => new(borderLine.Line + (-vector), borderLine.Border);
        #endregion

        #region Methods
        ///// <summary>引数で指定した座標系(int)に変換します</summary>
        //public RoixBorderIntPoint ConvertToRoixInt(in RoixIntSize destIntSize, RoundingMode mode = RoundingMode.Floor)
        //    => ConvertToRoixInt(destIntSize, mode, mode);

        ///// <summary>引数で指定した座標系(int)に変換します</summary>
        //public RoixBorderIntPoint ConvertToRoixInt(in RoixIntSize destIntSize, RoundingMode roundingX, RoundingMode roundingY)
        //{
        //    if (this.Border.IsEmpty || this.Border.IsZero)
        //    {
        //        if (roundingX != roundingY) throw new NotImplementedException();
        //        return new(Point.ToRoixInt(roundingX), RoixIntSize.Zero);
        //    }

        //    var point1 = RoixIntPoint.Create(this.Point, this.Border, destIntSize, roundingX, roundingY);
        //    var point2 = point1.GetClippedIntPoint(destIntSize);
        //    return new(point2, destIntSize);
        //}

        //public RoixRatioXY ToRoixRatio() => Line / Border;

        #endregion

    }
}
