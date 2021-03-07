using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf.Utils
{
    public static partial class RoixPlaneConverterExtension
    {
        //public static RoixBorderIntPoint CreateRoixBorderInt(in this RoixPoint point, in RoixSize viewSize, in RoixIntSize imageSize)
        //{
        //    throw new NotImplementedException();
        //}
        //public static RoixBorderIntPoint AdjustRoixWithResolutionOfImage(in this RoixBorderPoint border, in RoixIntSize imageSize)
        //    => CreateRoixBorderInt(border.Point, border.Border, imageSize);
    }

    public static partial class RoixPlaneConverterExtension
    {
#if false
        // AdjustRoixWithResolutionOfImage
        public static RoixPoint AdjustRoixWithResolutionOfImage(in this RoixPoint viewPoint, in RoixSize viewSize, in RoixIntSize imageSize)
        {
            // まず Image座標系 の Point を求める
            var imagePoint = RoixIntPoint.CreateRoixIntPoint(viewPoint, viewSize, imageSize);

            // 次に View座標系 に戻す
            static RoixPoint ToRoixPoint(in RoixIntPoint imagePoint, in RoixIntSize imageSize, in RoixSize viewSize)
            {
                return imagePoint * (viewSize / imageSize);
            }
            var newPoint = ToRoixPoint(imagePoint, imageSize, viewSize);

            return newPoint;
        }

        public static RoixPoint AdjustRoixWithResolutionOfImage(in this RoixBorderPoint border, in RoixIntSize imageSize)
            => AdjustRoixWithResolutionOfImage(border.Point, border.Border, imageSize);

        public static RoixSize AdjustRoixWithResolutionOfImage(in this RoixSize size, in RoixSize viewSize, in RoixIntSize imageSize)
        {
            throw new NotImplementedException();
        }

        public static RoixSize AdjustRoixWithResolutionOfImage(in this RoixBorderSize border, in RoixIntSize imageSize)
            => AdjustRoixWithResolutionOfImage(border.Size, border.Border, imageSize);

        public static RoixVector AdjustRoixWithResolutionOfImage(in this RoixVector vector, in RoixSize viewSize, in RoixIntSize imageSize)
        {
            throw new NotImplementedException();
        }
        public static RoixVector AdjustRoixWithResolutionOfImage(in this RoixBorderVector border, in RoixIntSize imageSize)
            => AdjustRoixWithResolutionOfImage(border.Vector, border.Border, imageSize);

        public static RoixRect AdjustRoixWithResolutionOfImage(in this RoixRect rect, in RoixSize viewSize, in RoixIntSize imageSize)
        {
            throw new NotImplementedException();
        }
        public static RoixRect AdjustRoixWithResolutionOfImage(in this RoixBorderRect border, in RoixIntSize imageSize)
            => AdjustRoixWithResolutionOfImage(border.Roi, border.Border, imageSize);
#endif
    }

    public static partial class RoixPlaneConverterExtension
    {
#if false
        public static RoixIntPoint ConvertCoordinateToImage(in this RoixPoint point, in RoixSize viewSize, in RoixIntSize imageSize)
        {
            throw new NotImplementedException();
        }
        public static RoixIntPoint ConvertCoordinateToImage(in this RoixBorderPoint border, in RoixIntSize imageSize)
            => ConvertCoordinateToImage(border.Point, border.Border, imageSize);

        public static RoixIntSize ConvertCoordinateToImage(in this RoixSize size, in RoixSize viewSize, in RoixIntSize imageSize)
        {
            throw new NotImplementedException();
        }
        public static RoixIntSize ConvertCoordinateToImage(in this RoixBorderSize border, in RoixIntSize imageSize)
            => ConvertCoordinateToImage(border.Size, border.Border, imageSize);

        //public static RoixIntVector ConvertCoordinateToImage(in this RoixVector vector, in RoixSize viewSize, in RoixIntSize imageSize) => throw new NotImplementedException();

        public static RoixIntRect ConvertCoordinateToImage(in this RoixRect rect, in RoixSize viewSize, in RoixIntSize imageSize)
        {
            throw new NotImplementedException();
        }
        public static RoixIntRect ConvertCoordinateToImage(in this RoixBorderRect border, in RoixIntSize imageSize)
            => ConvertCoordinateToImage(border.Roi, border.Border, imageSize);
#endif
    }
}
