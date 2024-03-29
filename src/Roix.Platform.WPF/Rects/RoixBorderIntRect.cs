﻿using Roix.SourceGenerator;
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

        internal RoixIntRect Value => Roi;

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

        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint, in RoixBorderIntVector borderVector)
            : this(borderPoint, borderPoint + borderVector) { }

        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint, in RoixIntSize size)
            : this(new RoixIntRect(borderPoint.Point, size), borderPoint.Border) { }

        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint, in RoixIntPoint point)
            : this(new RoixIntRect(borderPoint.Point, point), borderPoint.Border) { }

        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint, in RoixIntVector vector)
            : this(new RoixIntRect(borderPoint.Point, borderPoint.Point + vector), borderPoint.Border) { }
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
            static RoixBorderIntPoint ConvertToNewBorderInt(in RoixBorderPoint srcBorderPoint, in RoixIntSize destIntSize, PointDirection roundingDirection)
            {
                var (roundingX, roundingY) = roundingDirection.GetRoundingMode();
                return srcBorderPoint.ConvertToNewBorderInt(destIntSize, roundingX, roundingY);
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
            var intPoint1 = ConvertToNewBorderInt(borderPoint1, intSize, point1Direction);
            var intPoint2 = ConvertToNewBorderInt(borderPoint2, intSize, point2Direction);
            return new RoixBorderIntRect(intPoint1, intPoint2);
        }

        /// <summary>RoixBorderPoint(double) から RoixBorderIntRect を作成し、指定サイズで制限します</summary>
        public static RoixBorderIntRect Create(
            in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2, in RoixIntSize destBorderSize,
            in RoixIntSize sizeMin = default, in RoixIntSize sizeMax = default)
        {
            if (sizeMin.IsZero && sizeMax.IsZero) return Create(borderPoint1, borderPoint2, destBorderSize);

            // Point1 を原点に Vector化して、長さで制限する
            var clippedPoint2Vector = (borderPoint2.Point - borderPoint1.Point);

            if (sizeMin.Width > 0 && sizeMin.Height > 0)
            {
                // サイズはデクリする（start/end で同じ画素が選択された場合、startは左上点 で endは右下点 として扱って1画素を選択する制御のため）
                var srcSizeMin = new RoixBorderSize(sizeMin - new RoixIntSize(1, 1), destBorderSize).ConvertToNewBorder(borderPoint1.Border).Size;
                clippedPoint2Vector = clippedPoint2Vector.LimitNear(srcSizeMin.Width, srcSizeMin.Height);
            }

            if (sizeMax.Width > 0 && sizeMax.Height > 0)
            {
                // サイズはデクリする（start/end で同じ画素が選択された場合、startは左上点 で endは右下点 として扱って1画素を選択する制御のため）
                var srcSizeMax = new RoixBorderSize(sizeMax - new RoixIntSize(1, 1), destBorderSize).ConvertToNewBorder(borderPoint1.Border).Size;
                clippedPoint2Vector = clippedPoint2Vector.LimitFar(srcSizeMax.Width, srcSizeMax.Height);
            }

            // Point2 を元の座標系に戻す（Point1 の原点を止める）
            var clippedPoint2Point = borderPoint1 + clippedPoint2Vector;
            return Create(borderPoint1, clippedPoint2Point, destBorderSize).ClipToBorder();
        }

        /// <summary>Rect の最小サイズを指定値で制限します</summary>
        public RoixBorderIntRect ClipByMinimumSize(in RoixIntSize minSize) => new(Roi.ClipByMinimumSize(minSize), Border);

        /// <summary>Rect の最大サイズを指定値で制限します</summary>
        public RoixBorderIntRect ClipByMaximumSize(in RoixIntSize maxSize) => new(Roi.ClipByMaximumSize(maxSize), Border);

        #endregion

    }
}
