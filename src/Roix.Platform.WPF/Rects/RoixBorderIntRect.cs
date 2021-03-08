using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixBorderIntRect
    {
        readonly struct SourceValues
        {
            public readonly RoixIntRect Roi;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntRect roi, in RoixIntSize border) => (Roi, Border) = (roi, border);
        }

        private RoixIntRect Value => _values.Roi;

        #region ctor
        // ◆基本のctorにthisしたい
        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint1, in RoixBorderIntPoint borderPoint2)
        {
            if (borderPoint1.Border != borderPoint2.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            _values = new(new RoixIntRect(borderPoint1.Point, borderPoint2.Point), borderPoint1.Border);
            Validate(this);
        }

        private partial void Validate(in RoixBorderIntRect value)
        {
            if (value.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }
        #endregion

        #region implicit
        public static implicit operator RoixBorderRect(in RoixBorderIntRect borderRect) => new(borderRect.Roi, borderRect.Border);
        #endregion

        #region explicit
        public static explicit operator RoixBorderIntRect(in RoixBorderRect borderRect) => new((RoixIntRect)borderRect.Roi, (RoixIntSize)borderRect.Border);
        #endregion

        #region Methods
        /// <summary>Border の内部に収めた Rect を返します</summary>
        public RoixBorderIntRect GetClippedBorderIntRect(bool isPointPriority = true)
        {
            var rect = Roi.GetClippedBorderIntRect(Border, isPointPriority);
            return new(rect, Border);
        }

        /// <summary>Roiの最小サイズを指定値で制限します</summary>
        //public RoixBorderIntRect ClippedByMinimumSize(in RoixIntSize minSize) => new(Roi.ClippedByMinimumSize(minSize), Border);

        /// <summary>RoixBorderPoint(double) から RoixBorderIntRect を作成します</summary>
        public static RoixBorderIntRect Create(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2, in RoixIntSize intSize)
        {
            static RoixBorderIntPoint ConvertToRoixInt(in RoixBorderPoint srcBorderPoint, in RoixIntSize destIntSize, PointPosition roundingDirection)
            {
                if (srcBorderPoint.Border.IsEmpty || srcBorderPoint.Border.IsZero) return (RoixBorderIntPoint)srcBorderPoint;

                var rounding = roundingDirection.GetRoundingMode();
                var point1 = RoixIntPoint.Create(srcBorderPoint.Point, srcBorderPoint.Border, destIntSize, rounding.X, rounding.Y);
                var point2 = point1.GetClippedIntPoint(destIntSize);
                return new(point2, destIntSize);
            }

            // point1 に対して point2 がどの位置にあるか判定
            var point2Position = (borderPoint2.Point - borderPoint1.Point) switch
            {
                (0, 0) => PointPosition.Same,
                ( > 0, < 0) => PointPosition.TopRight,
                ( <= 0, <= 0) => PointPosition.TopLeft,     // Top, Left
                ( < 0, > 0) => PointPosition.BottomLeft,
                ( >= 0, >= 0) => PointPosition.BottomRight, // Bottom, Right
                _ => throw new NotSupportedException(),
            };
            if (point2Position is PointPosition.Same) return RoixBorderIntRect.Zero;
            var point1Position = point2Position.GetOppositePosition();

            // double座標系の Point を int座標系に丸める
            var intPoint1 = ConvertToRoixInt(borderPoint1, intSize, point1Position);
            var intPoint2 = ConvertToRoixInt(borderPoint2, intSize, point2Position);
            return new RoixBorderIntRect(intPoint1, intPoint2);
        }

        #endregion

    }
}
