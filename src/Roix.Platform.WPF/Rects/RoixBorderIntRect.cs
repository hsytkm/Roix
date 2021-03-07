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

        private RoixIntRect Value => _values.Roi;

        #region ctor
        public RoixBorderIntRect(in RoixBorderIntPoint borderPoint1, in RoixBorderIntPoint borderPoint2)
        {
            if (borderPoint1.Border != borderPoint2.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            _values = new(new RoixIntRect(borderPoint1.Point, borderPoint2.Point), borderPoint1.Border);
        }

        #endregion

        #region implicit
        public static implicit operator RoixBorderRect(in RoixBorderIntRect borderRect) => new(borderRect.Roi, borderRect.Border);
        #endregion

        #region explicit
        #endregion

        #region operator
        #endregion

        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// Roi の左上点を優先して Rect を Border サイズ内に納めます。
        /// Roi の左上点が Border の境界上に乗っている場合は、戻り値の Size が Zero になります。
        /// </summary>
        private RoixBorderIntRect GetClippedBorderIntRectByPointPriority()
        {
            if (IsInsideBorder) return this;

            var left = Math.Clamp(Roi.Left, 0, Border.Width);
            var top = Math.Clamp(Roi.Top, 0, Border.Height);

            // 最小側にめり込んで制限された場合は正数になる（その値だけ長さを伸縮する）
            var (deltaLeft, deltaTop) = (left - Roi.Left, top - Roi.Top);

            var width = Math.Clamp(Roi.Width - deltaLeft, 0, Border.Width - left);
            var height = Math.Clamp(Roi.Height - deltaTop, 0, Border.Height - top);
            var rect = new RoixIntRect(new RoixIntPoint(left, top), new RoixIntSize(width, height));
            return new(rect, Border);
        }

        /// <summary>
        /// Roi のサイズを優先して Rect を Border サイズに納めます。
        /// </summary>
        private RoixBorderIntRect GetClippedBorderIntRectBySizePriority()
        {
            if (IsInsideBorder) return this;

            var width = Math.Clamp(Roi.Width, 0, Border.Width);
            var height = Math.Clamp(Roi.Height, 0, Border.Height);
            var left = Math.Clamp(Roi.Left - GetJutLength(Roi.Left, Roi.Right, 0, Border.Width), 0, Border.Width - width);
            var top = Math.Clamp(Roi.Top - GetJutLength(Roi.Top, Roi.Bottom, 0, Border.Height), 0, Border.Height - height);
            var rect = new RoixIntRect(new RoixIntPoint(left, top), new RoixIntSize(width, height));
            return new(rect, Border);

            static double GetJutLength(double left, double right, double min, double max)
            {
                // 水平方向の場合、左に食み出てたら負数、右に食み出てたら正数を返す
                if (left < min) return min - left;
                if (max < right) return right - max;
                return 0;
            }
        }

        public RoixBorderIntRect GetClippedBorderIntRect(bool isPointPriority = true)
            => isPointPriority ? GetClippedBorderIntRectByPointPriority() : GetClippedBorderIntRectBySizePriority();

        /// <summary>Roiの最小サイズを指定値で制限する</summary>
        //public RoixBorderIntRect ClippedByMinimumSize(in RoixIntSize minSize) => new(Roi.ClippedByMinimumSize(minSize), Border);

        #region PointPosition
        [Flags]
        private enum PointPosition
        {
            Same = 0x0000,
            Right = 0x0001,
            Left = 0x0002,
            Bottom = 0x0004,
            Top = 0x0008,
            TopLeft = Top | Left,
            TopRight = Top | Right,
            BottomRight = Bottom | Right,
            BottomLeft = Bottom | Left,
        };

        private static RoixBorderIntPoint ConvertToRoixInt(in RoixBorderPoint srcBorderPoint, in RoixIntSize destIntSize, PointPosition roundingDirection)
        {
            static RoundingMode GetRoundingModeX(PointPosition position)
            {
                if (position.HasFlag(PointPosition.Left)) return RoundingMode.Floor;
                if (position.HasFlag(PointPosition.Right)) return RoundingMode.Ceiling;
                throw new NotImplementedException();
            }
            static RoundingMode GetRoundingModeY(PointPosition position)
            {
                if (position.HasFlag(PointPosition.Top)) return RoundingMode.Floor;
                if (position.HasFlag(PointPosition.Bottom)) return RoundingMode.Ceiling;
                throw new NotImplementedException();
            }

            if (srcBorderPoint.Border.IsEmpty || srcBorderPoint.Border.IsZero) return (RoixBorderIntPoint)srcBorderPoint;

            var roundingX = GetRoundingModeX(roundingDirection);
            var roundingY = GetRoundingModeY(roundingDirection);

            var intPoint = srcBorderPoint.Point * (destIntSize / srcBorderPoint.Border);
            var point1 = new RoixIntPoint(intPoint.X.ToInt(roundingX), intPoint.Y.ToInt(roundingY));
            var point2 = point1.GetClippedIntPoint(destIntSize);
            return new(point2, destIntSize);
        }
        #endregion

        /// <summary>RoixBorderPoint(double) から RoixBorderIntRect を作成します</summary>
        public static RoixBorderIntRect Create(in RoixBorderPoint borderPoint1, in RoixBorderPoint borderPoint2, in RoixIntSize intSize)
        {
            static PointPosition GetOppositePosition(PointPosition pos) => pos switch
            {
                PointPosition.Same => PointPosition.Same,
                PointPosition.TopLeft => PointPosition.BottomRight,
                PointPosition.TopRight => PointPosition.BottomLeft,
                PointPosition.BottomRight => PointPosition.TopLeft,
                PointPosition.BottomLeft => PointPosition.TopRight,
                _ => throw new NotImplementedException(),
            };

            var (point1, point2) = (borderPoint1.Point, borderPoint2.Point);

            // point1 に対して point2 がどの位置にあるか判定
            var point2Position = (point2.X - point1.X, point2.Y - point1.Y) switch
            {
                (0, 0) => PointPosition.Same,
                ( > 0, < 0) => PointPosition.TopRight,
                ( <= 0, <= 0) => PointPosition.TopLeft,     // Top, Left
                ( < 0, > 0) => PointPosition.BottomLeft,
                ( >= 0, >= 0) => PointPosition.BottomRight, // Bottom, Right
                _ => throw new NotSupportedException(),
            };
            if (point2Position is PointPosition.Same) return RoixBorderIntRect.Zero;

            var point1Position = GetOppositePosition(point2Position);
            var intPoint1 = ConvertToRoixInt(borderPoint1, intSize, point1Position);
            var intPoint2 = ConvertToRoixInt(borderPoint2, intSize, point2Position);
            return new RoixBorderIntRect(intPoint1, intPoint2);
        }

        #endregion

    }
}
