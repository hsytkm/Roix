using Roix.Core;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Roix.Wpf.Extensions
{
    public static class WpfConvertPointExtension
    {
        public static RoixPointDouble ToRoixPointDouble(in this Point point) => new(point.X, point.Y);

        public static Point ToPoint(this RoixPointDouble point) => new(point.X, point.Y);
    }

    public static class WpfConvertSizeExtension
    {
        public static RoixSizeDouble ToRoixSizeDouble(in this Size size) => new(size.Width, size.Height);
        public static RoixSizeDouble ToRoixSizeDouble(this FrameworkElement fe) => new(fe.ActualWidth, fe.ActualHeight);

        public static Size ToSize(this RoixSizeDouble size) => new(size.Width, size.Height);
    }


    public static class WpfConvertVectorExtension
    {
        public static RoixVectorDouble ToRoixVectorDouble(in this Vector vector) => new(vector.X, vector.Y);

        public static Vector ToVector(this RoixVectorDouble vector) => new(vector.X, vector.Y);
    }

    public static class WpfConvertRectExtension
    {
        public static RoixRectDouble ToRoixRectDouble(in this Rect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        public static Rect ToRect(this RoixRectDouble rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        public static PointCollection ToPointCollection(this RoixRectDouble rect)
            => new PointCollection(new[] { rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft }.Select(x => x.ToPoint()));

    }
}
