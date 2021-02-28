﻿using System;

namespace Roix.Wpf
{
    [SourceGenerator.RoixStructGenerator]
    public readonly partial struct RoixGaugeRect
    {
        readonly struct SourceValues
        {
            public readonly RoixRect Roi;
            public readonly RoixSize Border;
            public SourceValues(in RoixRect roi, in RoixSize border) => (Roi, Border) = (roi, border);
        }

        #region ctor
        public RoixGaugeRect(in RoixRect roi, in RoixSize border)
        {
            if (border.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            _values = new(roi, border);
        }

        public RoixGaugeRect(in RoixGaugePoint gaugePoint, in RoixGaugeSize gaugeSize)
        {
            if (gaugePoint.Border != gaugeSize.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            _values = new(new RoixRect(gaugePoint.Point, gaugeSize.Size), gaugePoint.Border);
        }

        public RoixGaugeRect(in RoixGaugePoint gaugePoint1, in RoixGaugePoint gaugePoint2)
        {
            if (gaugePoint1.Border != gaugePoint2.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            _values = new(new RoixRect(gaugePoint1.Point, gaugePoint2.Point), gaugePoint1.Border);
        }

        public RoixGaugeRect(in RoixGaugePoint gaugePoint, in RoixGaugeVector gaugeVector) : this(gaugePoint, gaugePoint + gaugeVector) { }

        #endregion

        #region implicit
        #endregion

        #region explicit
        #endregion

        #region operator
        #endregion

        #region Properties
        public bool IsInsideBorder => Roi.IsInside(Border);
        public bool IsOutsideBorder => !IsInsideBorder;
        #endregion

        #region Methods
        public RoixGaugeRect ConvertToNewGauge(in RoixSize newBorder)
        {
            if (Border.IsInvalid) return this;
            if (newBorder.IsInvalid) throw new ArgumentException(ExceptionMessages.SizeIsInvalid);

            var newPoint = new RoixPoint(Roi.X * newBorder.Width / Border.Width, Roi.Y * newBorder.Height / Border.Height);
            var newSize = new RoixSize(Roi.Width * newBorder.Width / Border.Width, Roi.Height * newBorder.Height / Border.Height);
            return new(new(newPoint, newSize), newBorder);
        }

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

        /// <summary>
        /// Roi の左上点を優先して Rect を Border サイズ内に納めます。
        /// Roi の左上点が Border の境界上に乗っている場合は、戻り値の Size が Zero になります。
        /// </summary>
        private RoixGaugeRect GetClippedGaugeRectByPointPriority()
        {
            if (IsInsideBorder) return this;

            var left = Math.Clamp(Roi.Left, 0, Border.Width);
            var top = Math.Clamp(Roi.Top, 0, Border.Height);

            // 最小側にめり込んで制限された場合は正数になる（その値だけ長さを伸縮する）
            var (deltaLeft, deltaTop) = (left - Roi.Left, top - Roi.Top);

            var width = Math.Clamp(Roi.Width - deltaLeft, 0, Border.Width - left);
            var height = Math.Clamp(Roi.Height - deltaTop, 0, Border.Height - top);
            var rect = new RoixRect(new RoixPoint(left, top), new RoixSize(width, height));
            return new(rect, Border);
        }

        /// <summary>
        /// Roi のサイズを優先して Rect を Border サイズに納めます。
        /// </summary>
        private RoixGaugeRect GetClippedGaugeRectBySizePriority()
        {
            if (IsInsideBorder) return this;

            var width = Math.Clamp(Roi.Width, 0, Border.Width);
            var height = Math.Clamp(Roi.Height, 0, Border.Height);
            var left = Math.Clamp(Roi.Left - GetJutLength(Roi.Left, Roi.Right, 0, Border.Width), 0, Border.Width - width);
            var top = Math.Clamp(Roi.Top - GetJutLength(Roi.Top, Roi.Bottom, 0, Border.Height), 0, Border.Height - height);
            var rect = new RoixRect(new RoixPoint(left, top), new RoixSize(width, height));
            return new(rect, Border);

            static double GetJutLength(double left, double right, double min, double max)
            {
                // 水平方向の場合、左に食み出てたら負数、右に食み出てたら正数を返す
                if (left < min) return min - left;
                if (max < right) return right - max;
                return 0;
            }
        }

        public RoixGaugeRect GetClippedGaugeRect(bool isPointPriority = true) => isPointPriority ? GetClippedGaugeRectByPointPriority() : GetClippedGaugeRectBySizePriority();
        #endregion

    }
}
