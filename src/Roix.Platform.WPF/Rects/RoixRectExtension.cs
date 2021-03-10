using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Roix.Wpf.Extensions
{
    public static class RoixRectExtension
    {
        public static System.Windows.Media.PointCollection ToPointCollection(in this RoixRect rect)
        {
            if (rect.IsEmpty) throw new ArgumentException(ExceptionMessages.RectIsEmpty);
            return new(new Point[] { rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft });
        }

    }
}
