using System;

namespace Roix.Wpf.Utils
{
    public static class RoixUtil
    {
#if false
        private static double RoundDownToMultipleOfX(double value, double div) => Math.Floor(value / div) * div;
        private static double RoundUpToMultipleOfX(double value, double div) => Math.Ceiling(value / div) * div;

        private static RoixPoint AdjustWithResolutionByFloor(in RoixPoint point, in RoixSize resolutionSize)
            => new(RoundDownToMultipleOfX(point.X, resolutionSize.Width), RoundDownToMultipleOfX(point.Y, resolutionSize.Height));

        private static RoixVector AdjustWithResolutionByCeiling(in RoixVector vector, in RoixSize resolutionSize)
            => new(RoundUpToMultipleOfX(vector.X, resolutionSize.Width), RoundUpToMultipleOfX(vector.Y, resolutionSize.Height));


        /// <summary>Point と Vector から Rect を作成します。作成される Rect は resolutionSize の倍数となり、最小サイズは resolutionSize になります。</summary>
        public static RoixRect CreateResolutionAdjustedRoixRect(in RoixPoint point, in RoixVector vector, in RoixSize resolutionSize)
        {
            var adjustedPoint = AdjustWithResolutionByFloor(point, resolutionSize);
            var adjustedVector = AdjustWithResolutionByCeiling(vector, resolutionSize);
            var adjustedRect = new RoixRect(adjustedPoint, adjustedVector);
            return adjustedRect.GetClippedRectByMinimumRoiSize(resolutionSize);
        }


        /// <summary>Point と Vector から Rect を作成します。作成される Rect は resolutionSize の倍数となり、最小サイズは resolutionSize になります。</summary>
        //public static RoixBorderRect CreateResolutionAdjustedRoixBorderRect(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector, in RoixSize resolutionSize)
        //{
        //    if (borderPoint.Border != borderVector.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
        //    var rect = CreateResolutionAdjustedRoixRect(borderPoint.Point, borderVector.Vector, resolutionSize);
        //    return new(rect, borderPoint.Border);
        //}

        /// <summary>Point と Vector から Rect を作成します。作成される Rect は resolutionSize の倍数となり、最小サイズは resolutionSize になります。</summary>
        public static RoixBorderRect CreateResolutionAdjustedRoixBorderRect(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector, in RoixIntSize imageSize)
        {
            if (borderPoint.Border != borderVector.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            if (imageSize.IsZero) throw new ArgumentException(ExceptionMessages.SizeIsZero);

            var resolutionSize = borderPoint.Border / imageSize;
            var rect = CreateResolutionAdjustedRoixRect(borderPoint.Point, borderVector.Vector, resolutionSize);
            return new(rect, borderPoint.Border);
        }

        /// <summary>Point を resolutionSize の倍数にします。</summary>
        public static RoixPoint CreateResolutionAdjustedRoixPoint(in RoixPoint point, in RoixSize resolutionSize)
        {
            return AdjustWithResolutionByFloor(point, resolutionSize);
        }

        /// <summary>Point を resolutionSize の倍数にします。</summary>
        //public static RoixBorderPoint CreateResolutionAdjustedRoixBorderPoint(in RoixBorderPoint borderPoint, in RoixSize resolutionSize)
        //{
        //    var adjustedPoint = CreateResolutionAdjustedRoixPoint(borderPoint.Point, resolutionSize);
        //    return new(adjustedPoint, borderPoint.Border);
        //}

        /// <summary>Point を imageSize の解像度(倍数)にします。</summary>
        public static RoixBorderPoint CreateResolutionAdjustedRoixBorderPoint(in RoixBorderPoint borderPoint, in RoixIntSize imageSize)
        {
            if (imageSize.IsZero) throw new ArgumentException(ExceptionMessages.SizeIsZero);

            var resolutionSize = borderPoint.Border / imageSize;
            var adjustedPoint = CreateResolutionAdjustedRoixPoint(borderPoint.Point, resolutionSize);
            return new(adjustedPoint, borderPoint.Border);
        }

        /// <summary>BorderPoint を RoixIntSize のサイズに変換します。</summary>
        //public static RoixIntPoint CreateResolutionAdjustedRoixIntRect(in RoixBorderPoint borderPoint, in RoixIntSize newIntSize)
        //{
        //    var newBorderPoint = borderPoint.ConvertToNewBorder(newIntSize);
        //    var point = newBorderPoint.ClippedRoixPoint;
        //    return point.ToRoixIntPoint(Rounding.Floor);
        //}
#endif
    }
}
