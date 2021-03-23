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

        public static implicit operator RoixBorderLine(in RoixBorderIntLine borderLine) => new(borderLine.Line, borderLine.Border);

        #region operator
        //public static RoixBorderPoint operator +(in RoixBorderPoint borderPoint, in RoixVector vector) => new(borderPoint.Point + vector, borderPoint.Border);
        //public static RoixBorderPoint operator -(in RoixBorderPoint borderPoint, in RoixVector vector) => new(borderPoint.Point - vector, borderPoint.Border);
        //public static RoixBorderVector operator -(in RoixBorderPoint borderPoint, in RoixPoint point) => new(borderPoint.Point - point, borderPoint.Border);

        //public static RoixBorderPoint operator +(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector) => (borderPoint.Border == borderVector.Border) ? borderPoint + borderVector.Vector : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        //public static RoixBorderPoint operator -(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector) => (borderPoint.Border == borderVector.Border) ? borderPoint - borderVector.Vector : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        //public static RoixBorderVector operator -(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2) => (borderPoint1.Border == borderPoint2.Border) ? borderPoint1 - borderPoint2.Point : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
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

        #endregion

    }
}
