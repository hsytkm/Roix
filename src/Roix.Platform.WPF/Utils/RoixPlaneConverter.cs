using System;

namespace Roix.Wpf.Utils
{
    public class RoixPlaneConverter
    {
#if false
        public RoixSize ViewSize { get; }
        public RoixIntSize ImageSize { get; }

        public RoixPlaneConverter(in RoixSize viewSize, in RoixIntSize imageSize) => (ViewSize, ImageSize) = (viewSize, imageSize);


        /// <summary>最小サイズOnView</summary>
        public RoixSize GetMinimunSizeOnView()
        {
            throw new NotImplementedException();
        }

        private void ThrowArgumentExceptionIfViewSizeIsDifferent(in RoixSize size)
        {
            if (size != ViewSize) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
        }
        private void ThrowArgumentExceptionIfViewSizeIsDifferent(in RoixBorderPoint border) => ThrowArgumentExceptionIfViewSizeIsDifferent(border.Border);
        private void ThrowArgumentExceptionIfViewSizeIsDifferent(in RoixBorderSize border) => ThrowArgumentExceptionIfViewSizeIsDifferent(border.Border);
        private void ThrowArgumentExceptionIfViewSizeIsDifferent(in RoixBorderVector border) => ThrowArgumentExceptionIfViewSizeIsDifferent(border.Border);
        private void ThrowArgumentExceptionIfViewSizeIsDifferent(in RoixBorderRect border) => ThrowArgumentExceptionIfViewSizeIsDifferent(border.Border);

        public RoixPoint AdjustRoixWithResolutionOfImage(in RoixPoint point)
            => RoixPlaneConverterExtension.AdjustRoixWithResolutionOfImage(point, ViewSize, ImageSize);

        public RoixPoint AdjustRoixWithResolutionOfImage(in RoixBorderPoint border)
        {
            ThrowArgumentExceptionIfViewSizeIsDifferent(border);
            return RoixPlaneConverterExtension.AdjustRoixWithResolutionOfImage(border.Point, ViewSize, ImageSize);
        }

        public RoixSize AdjustRoixWithResolutionOfImage(in RoixSize size)
            => RoixPlaneConverterExtension.AdjustRoixWithResolutionOfImage(size, ViewSize, ImageSize);

        public RoixSize AdjustRoixWithResolutionOfImage(in RoixBorderSize border)
        {
            ThrowArgumentExceptionIfViewSizeIsDifferent(border);
            return RoixPlaneConverterExtension.AdjustRoixWithResolutionOfImage(border.Size, ViewSize, ImageSize);
        }

        public RoixVector AdjustRoixWithResolutionOfImage(in RoixVector vector) => RoixPlaneConverterExtension.AdjustRoixWithResolutionOfImage(vector, ViewSize, ImageSize);

        public RoixVector AdjustRoixWithResolutionOfImage(in RoixBorderVector border)
        {
            ThrowArgumentExceptionIfViewSizeIsDifferent(border);
            return RoixPlaneConverterExtension.AdjustRoixWithResolutionOfImage(border.Vector, ViewSize, ImageSize);
        }

        public RoixRect AdjustRoixWithResolutionOfImage(in RoixRect rect) => RoixPlaneConverterExtension.AdjustRoixWithResolutionOfImage(rect, ViewSize, ImageSize);

        public RoixRect AdjustRoixWithResolutionOfImage(in RoixBorderRect border)
        {
            ThrowArgumentExceptionIfViewSizeIsDifferent(border);
            return RoixPlaneConverterExtension.AdjustRoixWithResolutionOfImage(border.Roi, ViewSize, ImageSize);
        }
#endif

#if false   // Rect複合系
        //public RoixRect AdjustRoixWithResolutionOfImage(in RoixPoint point, in RoixSize size) => throw new NotImplementedException();
        //public RoixRect AdjustRoixWithResolutionOfImage(in RoixPoint point1, in RoixPoint point2) => throw new NotImplementedException();

        // 2-1
        public RoixRect AdjustRoixWithResolutionOfImage(in RoixPoint point, in RoixVector vector)
        {
            throw new NotImplementedException();
        }

        public RoixRect AdjustRoixWithResolutionOfImage(in RoixBorderPoint borderPoint, in RoixBorderVector borderVector)
        {
            ThrowArgumentExceptionIfViewSizeIsDifferent(borderPoint);
            ThrowArgumentExceptionIfViewSizeIsDifferent(borderVector);

            return AdjustRoixWithResolutionOfImage(borderPoint.Point, borderVector.Vector);
        }
#endif

#if false
        // 1
        public RoixIntPoint ConvertCoordinateToImage(in RoixPoint point) => RoixPlaneConverterExtension.ConvertCoordinateToImage(point, ViewSize, ImageSize);

        public RoixIntPoint ConvertCoordinateToImage(in RoixBorderPoint border)
        {
            ThrowArgumentExceptionIfViewSizeIsDifferent(border);
            return RoixPlaneConverterExtension.ConvertCoordinateToImage(border.Point, ViewSize, ImageSize);
        }

        public RoixIntSize ConvertCoordinateToImage(in RoixSize size) => RoixPlaneConverterExtension.ConvertCoordinateToImage(size, ViewSize, ImageSize);

        public RoixIntSize ConvertCoordinateToImage(in RoixBorderSize border)
        {
            ThrowArgumentExceptionIfViewSizeIsDifferent(border);
            return RoixPlaneConverterExtension.ConvertCoordinateToImage(border.Size, ViewSize, ImageSize);
        }

        //public RoixIntVector ConvertCoordinateToImage(in RoixVector vector) => throw new NotImplementedException();

        // 3-2
        public RoixIntRect ConvertCoordinateToImage(in RoixRect rect) => RoixPlaneConverterExtension.ConvertCoordinateToImage(rect, ViewSize, ImageSize);

        public RoixIntRect ConvertCoordinateToImage(in RoixBorderRect border)
        {
            ThrowArgumentExceptionIfViewSizeIsDifferent(border);
            return RoixPlaneConverterExtension.ConvertCoordinateToImage(border.Roi, ViewSize, ImageSize);
        }
#endif

    }
}
