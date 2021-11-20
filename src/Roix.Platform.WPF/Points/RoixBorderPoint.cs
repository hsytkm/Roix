using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderPoint
    {
        readonly struct SourceValues
        {
            public readonly RoixPoint Point;
            public readonly RoixSize Border;
            public SourceValues(in RoixPoint point, in RoixSize border) => (Point, Border) = (point, border);
        }

        internal RoixPoint Value => Point;

        #region implicit
        public static implicit operator RoixBorderIntPoint(in RoixBorderPoint borderPoint) => new(borderPoint.Point.ToRoixInt(), borderPoint.Border.ToRoixInt());
        #endregion

        #region operator
        public static RoixBorderPoint operator +(in RoixBorderPoint borderPoint, in RoixVector vector) => new(borderPoint.Point + vector, borderPoint.Border);
        public static RoixBorderPoint operator -(in RoixBorderPoint borderPoint, in RoixVector vector) => new(borderPoint.Point - vector, borderPoint.Border);
        public static RoixBorderVector operator -(in RoixBorderPoint borderPoint, in RoixPoint point) => new(borderPoint.Point - point, borderPoint.Border);

        public static RoixBorderPoint operator +(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector) => (borderPoint.Border == borderVector.Border) ? borderPoint + borderVector.Vector : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        public static RoixBorderPoint operator -(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector) => (borderPoint.Border == borderVector.Border) ? borderPoint - borderVector.Vector : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        public static RoixBorderVector operator -(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2) => (borderPoint1.Border == borderPoint2.Border) ? borderPoint1 - borderPoint2.Point : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        #endregion

        #region Methods
        // ◆これいるか？ ConvertToRoixInt の方が良くない？
        //public RoixIntPoint ToRoixIntPoint(bool isCheckBorder = true)
        //{
        //    if (isCheckBorder && IsOutsideBorder) throw new InvalidOperationException(ExceptionMessages.MustInsideTheBorder);

        //    var srcPoint = (RoixIntPoint)Point;
        //    var intSize = (RoixIntSize)Border;
        //    if (intSize.IsZero) throw new InvalidOperationException(ExceptionMessages.SizeIsZero);

        //    var x = Math.Clamp(srcPoint.X, 0, intSize.Width - 1);
        //    var y = Math.Clamp(srcPoint.Y, 0, intSize.Height - 1);
        //    return new(x, y);
        //}

        // ◆これいるか？
        //public RoixBorderRect CreateRoixBorderRect(in RoixVector vector) => new(new RoixRect(Point, vector), Border);

        // ◆これいるか？
        //public RoixBorderRect CreateRoixBorderRect(in RoixBorderVector borderVector)
        //{
        //    if (Border != borderVector.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
        //    return CreateRoixBorderRect(borderVector.Vector);
        //}

        /// <summary>引数で指定した座標系(int)に変換します</summary>
        public RoixBorderIntPoint ConvertToNewBorderInt(in RoixIntSize destIntSize, RoundingMode mode = RoundingMode.Floor)
            => ConvertToNewBorderInt(destIntSize, mode, mode);

        /// <summary>引数で指定した座標系(int)に変換します</summary>
        public RoixBorderIntPoint ConvertToNewBorderInt(in RoixIntSize destIntSize, RoundingMode roundingX, RoundingMode roundingY)
        {
            if (this.Border.IsEmpty || this.Border.IsZero)
            {
                if (roundingX != roundingY) throw new NotImplementedException();
                return new(Point.ToRoixInt(roundingX), RoixIntSize.Zero);
            }

            var point1 = RoixIntPoint.Create(this.Point, this.Border, destIntSize, roundingX, roundingY);
            var point2 = point1.ClipToSize(destIntSize);
            return new(point2, destIntSize);
        }

        /// <summary>引数で指定した座標系(int)の分解能に調整する</summary>
        //public RoixBorderPoint AdjustRoixWithResolutionOfImage(in RoixIntSize destIntSize, RoundingMode mode = RoundingMode.Floor)
        //{
        //    if (this.Border.IsEmpty || this.Border.IsZero) return this;
        //    var point = this.Point.AdjustRoixWithResolutionOfImage(this.Border, destIntSize, mode);
        //    return new(point, this.Border);
        //}

        public RoixRatioXY ToRoixRatio() => Point / (RoixPoint)Border;

        #endregion

    }
}
