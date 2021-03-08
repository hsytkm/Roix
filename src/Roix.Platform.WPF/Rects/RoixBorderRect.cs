using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixBorderRect
    {
        readonly struct SourceValues
        {
            public readonly RoixRect Roi;
            public readonly RoixSize Border;
            public SourceValues(in RoixRect roi, in RoixSize border) => (Roi, Border) = (roi, border);
        }

        private RoixRect Value => _values.Roi;

        #region ctor
        // ◆基本のctorにthisしたい
        public RoixBorderRect(in RoixBorderPoint borderPoint, in RoixBorderSize borderSize)
        {
            if (borderPoint.Border != borderSize.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            _values = new(new RoixRect(borderPoint.Point, borderSize.Size), borderPoint.Border);
            Validate(this);
        }

        // ◆基本のctorにthisしたい
        public RoixBorderRect(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2)
        {
            if (borderPoint1.Border != borderPoint2.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            _values = new(new RoixRect(borderPoint1.Point, borderPoint2.Point), borderPoint1.Border);
            Validate(this);
        }

        public RoixBorderRect(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector) : this(borderPoint, borderPoint + borderVector) { }

        private partial void Validate(in RoixBorderRect value)
        {
            if (value.Border.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
        }
        #endregion

        #region implicit
        #endregion

        #region Methods
        public RoixIntRect ToRoixIntRect(bool isCheckBorder = true)
        {
            if (isCheckBorder && IsOutsideBorder) throw new InvalidOperationException(ExceptionMessages.MustInsideTheBorder);

            var srcRect = (RoixIntRect)Roi;
            var intSize = (RoixIntSize)Border;
            if (intSize.IsZero) throw new InvalidOperationException(ExceptionMessages.SizeIsZero);

            var x = Math.Clamp(srcRect.X, 0, intSize.Width - 1);
            var y = Math.Clamp(srcRect.Y, 0, intSize.Height - 1);
            var width = Math.Clamp(srcRect.Width, 0, intSize.Width - x);
            var height = Math.Clamp(srcRect.Height, 0, intSize.Height - y);
            return new(x, y, width, height);
        }

        /// <summary>Border の内部に収めた Rect を返す</summary>
        public RoixBorderRect GetClippedBorderRect(bool isPointPriority = true)
        {
            var rect = Roi.GetClippedBorderRect(Border, isPointPriority);
            return new(rect, Border);
        }

        /// <summary>引数で指定した座標系(int)に変換する</summary>
        //public RoixBorderIntRect ConvertToRoixInt(in RoixIntSize destIntSize, RoundingMode mode = RoundingMode.Floor)
        //{
        //    if (this.Border.IsEmpty || this.Border.IsZero) return (RoixBorderIntRect)this;

        //    var rect1 = RoixIntRect.Create(this.Roi, this.Border, destIntSize, mode);
        //    var rect2 = rect1.GetClippedIntRect(destIntSize);
        //    return new(rect2, destIntSize);
        //}

        /// <summary>引数で指定した座標系(int)の分解能に調整する</summary>
        //public RoixBorderRect AdjustRoixWithResolutionOfImage(in RoixIntSize destIntSize, RoundingMode mode = RoundingMode.Floor)
        //{
        //    if (this.Border.IsEmpty || this.Border.IsZero) return this;
        //    var rect = this.Roi.AdjustRoixWithResolutionOfImage(this.Border, destIntSize, mode);
        //    return new(rect, this.Border);
        //}

        #endregion

    }
}
