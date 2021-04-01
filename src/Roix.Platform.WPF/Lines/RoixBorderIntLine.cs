using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderIntLine
    {
        readonly struct SourceValues
        {
            public readonly RoixIntLine Line;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntLine line, in RoixIntSize border) => (Line, Border) = (line, border);
        }

        private RoixIntLine Value => Line;

        #region ctor
        public RoixBorderIntLine(in RoixBorderIntPoint borderPoint1, in RoixBorderIntPoint borderPoint2)
            : this(new RoixIntLine(borderPoint1.Point, borderPoint2.Point), borderPoint1.Border)
        {
            if (borderPoint1.Border != borderPoint2.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            if (borderPoint1.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }
        #endregion

        public static implicit operator RoixBorderLine(in RoixBorderIntLine borderLine) => new(borderLine.Line, borderLine.Border);

        #region operator
        public static RoixBorderIntLine operator +(in RoixBorderIntLine borderLine, in RoixIntVector vector) => new(borderLine.Line + vector, borderLine.Border);
        public static RoixBorderIntLine operator -(in RoixBorderIntLine borderLine, in RoixIntVector vector) => new(borderLine.Line + (-vector), borderLine.Border);
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

        ///// <summary>Point を Border の内部に収めて返します</summary>
        //public RoixBorderPoint GetClippedBorderPoint() => new(new(Math.Clamp(Point.X, 0, Border.Width), Math.Clamp(Point.Y, 0, Border.Height)), Border);

        //public RoixRatioXY ToRoixRatio() => Line / Border;


        /// <summary>RoixBorderLine(double) から RoixBorderIntLine を作成します</summary>
        public static RoixBorderIntLine Create(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2, in RoixIntSize intSize, RoundingMode rounding = RoundingMode.Floor)
        {
            // double座標系の Point を int座標系に丸める
            var intPoint1 = borderPoint1.ConvertToNewBorderInt(intSize, rounding);
            var intPoint2 = borderPoint2.ConvertToNewBorderInt(intSize, rounding);
            return new(intPoint1, intPoint2);
        }

        #endregion

    }
}
