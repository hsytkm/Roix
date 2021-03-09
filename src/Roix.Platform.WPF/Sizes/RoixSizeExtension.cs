using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Roix.Wpf.Extensions
{
    public static class RoixSizeExtension
    {
        public static RoixSize ActualSizeToRoix(this FrameworkElement fe) => new(fe.ActualWidth, fe.ActualHeight);

        public static RoixIntSize PixelSizeToRoixInt(this BitmapSource bitmap) => new(bitmap.PixelWidth, bitmap.PixelHeight);
        public static RoixSize PixelSizeToRoix(this BitmapSource bitmap) => new(bitmap.PixelWidth, bitmap.PixelHeight);

    }
}
