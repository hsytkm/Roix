using System;
using System.Windows;

namespace Roix.Wpf.Extensions
{
    public static class RoixRectExtension
    {
        public static System.Windows.Media.PointCollection ToPointCollection(in this RoixRect rect)
        {
            if (rect.IsEmpty) throw new ArgumentException(ExceptionMessages.RectIsEmpty);

            if (rect.Size.IsIncludeZero)
            {
                return ToPointCollectionInternal(rect.ClipByMinimumSize(new Size(1, 1)));
            }
            return ToPointCollectionInternal(rect);

            static System.Windows.Media.PointCollection ToPointCollectionInternal(in RoixRect rect)
                => new(new Point[] { rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft });
        }

        public static Int32Rect ToInt32Rect(in this RoixIntRect rect)
            => new(rect.X, rect.Y, rect.Width, rect.Height);

    }
}
