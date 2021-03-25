using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.None)]
    public readonly partial struct RoixBorderIntRect
    {
        readonly struct SourceValues
        {
            public readonly RoixIntRect Roi;
            public readonly RoixIntSize Border;
            public SourceValues(in RoixIntRect roi, in RoixIntSize border) => (Roi, Border) = (roi, border);
        }

        private RoixIntRect Value => Roi;

        #region ctor
        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint, in RoixBorderIntSize borderSize)
            : this(new RoixIntRect(borderPoint.Point, borderSize.Size), borderPoint.Border)
        {
            if (borderPoint.Border != borderSize.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            if (borderPoint.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }

        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint1, in RoixBorderIntPoint borderPoint2)
            : this(new RoixIntRect(borderPoint1.Point, borderPoint2.Point), borderPoint1.Border)
        {
            if (borderPoint1.Border != borderPoint2.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            if (borderPoint1.Border.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
        }

        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint, in RoixBorderIntVector borderVector) : this(borderPoint, borderPoint + borderVector) { }
        #endregion

        public static implicit operator RoixBorderRect(in RoixBorderIntRect borderRect) => new(borderRect.Roi, borderRect.Border);

        #region Methods
        /// <summary>Border の内部に収めた Rect を返します</summary>
        public RoixBorderIntRect GetClippedBorderIntRect(bool isPointPriority = true)
        {
            var rect = Roi.GetClippedIntRect(Border, isPointPriority);
            return new(rect, Border);
        }

        /// <summary>Roiの最小サイズを指定値で制限します</summary>
        //public RoixBorderIntRect ClippedByMinimumSize(in RoixIntSize minSize) => new(Roi.ClippedByMinimumSize(minSize), Border);

        /// <summary>RoixBorderPoint(double) から RoixBorderIntRect を作成します</summary>
        public static RoixBorderIntRect Create(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2, in RoixIntSize intSize)
        {
            static RoixBorderIntPoint ConvertToRoixInt(in RoixBorderPoint srcBorderPoint, in RoixIntSize destIntSize, PointDirection roundingDirection)
            {
                var (roundingX, roundingY) = roundingDirection.GetRoundingMode();
                return srcBorderPoint.ConvertToRoixInt(destIntSize, roundingX, roundingY);
            }

            // point1 に対して point2 がどの方向にあるか判定(真横/真上は差し替える)
            var point2Direction = borderPoint2.Point.GetPointDirection(origin: borderPoint1.Point);
            if (point2Direction is PointDirection.Same) return RoixBorderIntRect.Zero;
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
            return new RoixBorderIntRect(intPoint1, intPoint2);
        }

        #endregion

    }
}
