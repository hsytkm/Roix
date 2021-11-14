using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderRect
    {
        readonly struct SourceValues
        {
            public readonly RoixRect Roi;
            public readonly RoixSize Border;
            public SourceValues(in RoixRect roi, in RoixSize border) => (Roi, Border) = (roi, border);
        }

        internal RoixRect Value => Roi;

        #region ctor
        public RoixBorderRect(in RoixBorderPoint borderPoint, in RoixBorderSize borderSize)
            : this(new RoixRect(borderPoint.Point, borderSize.Size), borderPoint.Border)
        {
            if (borderPoint.Border != borderSize.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            if (borderPoint.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }

        public RoixBorderRect(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2)
            : this(new RoixRect(borderPoint1.Point, borderPoint2.Point), borderPoint1.Border)
        {
            if (borderPoint1.Border != borderPoint2.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            if (borderPoint1.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }

        public RoixBorderRect(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector)
            : this(borderPoint, borderPoint + borderVector) { }

        public RoixBorderRect(in RoixBorderPoint borderPoint, in RoixSize size)
            : this(new RoixRect(borderPoint.Point, size), borderPoint.Border) { }

        public RoixBorderRect(in RoixBorderPoint borderPoint, in RoixPoint point)
            : this(new RoixRect(borderPoint.Point, point), borderPoint.Border) { }

        public RoixBorderRect(in RoixBorderPoint borderPoint, in RoixVector vector)
            : this(new RoixRect(borderPoint.Point, borderPoint.Point + vector), borderPoint.Border) { }
        #endregion

        #region operator
        // ◆Generatorに押し込みたい
        public static RoixBorderRect operator +(in RoixBorderRect borderRect, in RoixVector vector)
        {
            if (borderRect.Roi.IsEmpty) throw new ArgumentException(ExceptionMessages.RectIsEmpty);
            return new(borderRect.Roi + vector, borderRect.Border);
        }
        #endregion

        #region Methods
        public RoixRatioXYWH ToRoixRatio() => new(Roi.Location / (RoixPoint)Border, Roi.Size / Border);

        // ◆これいるか？
        //public RoixIntRect ToRoixIntRect(bool isCheckBorder = true)
        //{
        //    if (isCheckBorder && IsOutsideBorder) throw new InvalidOperationException(ExceptionMessages.MustInsideTheBorder);

        //    var srcRect = (RoixIntRect)Roi;
        //    var intSize = (RoixIntSize)Border;
        //    if (intSize.IsZero) throw new InvalidOperationException(ExceptionMessages.SizeIsZero);

        //    var x = Math.Clamp(srcRect.X, 0, intSize.Width - 1);
        //    var y = Math.Clamp(srcRect.Y, 0, intSize.Height - 1);
        //    var width = Math.Clamp(srcRect.Width, 0, intSize.Width - x);
        //    var height = Math.Clamp(srcRect.Height, 0, intSize.Height - y);
        //    return new(x, y, width, height);
        //}

        /// <summary>Rect を Border の内部に収めて返します</summary>
        public RoixBorderRect GetClippedBorderRect(bool isPointPriority = true)
        {
            var rect = Roi.GetClippedRect(Border, isPointPriority);
            return new(rect, Border);
        }

        /// <summary>引数で指定した座標系(int)に変換します</summary>
        public RoixBorderIntRect ConvertToNewBorderInt(in RoixIntSize destIntSize, RoundingMode mode = RoundingMode.Floor)
            => ConvertToNewBorderInt(destIntSize, mode, mode);

        /// <summary>引数で指定した座標系(int)に変換します</summary>
        public RoixBorderIntRect ConvertToNewBorderInt(in RoixIntSize destIntSize, RoundingMode roundingX, RoundingMode roundingY)
        {
            if (this.Border.IsEmpty || this.Border.IsZero)
            {
                if (roundingX != roundingY) throw new NotImplementedException();
                return new(Roi.ToRoixInt(roundingX), RoixIntSize.Zero);
            }

            var rect1 = RoixIntRect.Create(this.Roi, this.Border, destIntSize, roundingX, roundingY);
            var rect2 = rect1.GetClippedIntRect(destIntSize);
            return new(rect2, destIntSize);
        }

        /// <summary>引数で指定した座標系(int)の分解能に調整する</summary>
        //public RoixBorderRect AdjustRoixWithResolutionOfImage(in RoixIntSize destIntSize, RoundingMode mode = RoundingMode.Floor)
        //{
        //    if (this.Border.IsEmpty || this.Border.IsZero) return this;
        //    var rect = this.Roi.AdjustRoixWithResolutionOfImage(this.Border, destIntSize, mode);
        //    return new(rect, this.Border);
        //}

        /// <summary>Rect の最小サイズを指定値で制限します</summary>
        public RoixBorderRect ClipByMinimumSize(in RoixSize minSize) => new(Roi.ClipByMinimumSize(minSize), Border);

        /// <summary>Rect の最大サイズを指定値で制限します</summary>
        public RoixBorderRect ClipByMaximumSize(in RoixSize maxSize) => new(Roi.ClipByMaximumSize(maxSize), Border);

        #endregion

    }
}
