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


        /// <summary>RoixBorderPoint(double) から RoixBorderIntRect を作成します</summary>
        public static RoixBorderIntLine Create(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2, in RoixIntSize intSize)
        {
            static RoixBorderIntPoint ConvertToRoixInt(in RoixBorderPoint srcBorderPoint, in RoixIntSize destIntSize, PointDirection roundingDirection)
            {
                var rounding = roundingDirection.GetRoundingMode();
                return srcBorderPoint.ConvertToRoixInt(destIntSize, rounding.X, rounding.Y);
            }

            // point1 に対して point2 がどの方向にあるか判定(真横/真上は差し替える)
            var point2Direction = borderPoint2.Point.GetPointDirection(origin: borderPoint1.Point);
            if (point2Direction is PointDirection.Same) return RoixBorderIntLine.Zero;
            point2Direction = point2Direction switch
            {
                PointDirection.Top or PointDirection.Left => PointDirection.TopLeft,
                PointDirection.Bottom or PointDirection.Right => PointDirection.BottomRight,
                _ => point2Direction,
            };
            var point1Direction = point2Direction.GetOppositeDirection();

            // double座標系の Point を int座標系に丸める
            var intPoint1 = ConvertToRoixInt(borderPoint1, intSize, point1Direction);
            var intPoint2 = ConvertToRoixInt(borderPoint2, intSize, point2Direction);
            return new RoixBorderIntLine(intPoint1, intPoint2);
        }

        #endregion

    }
}
