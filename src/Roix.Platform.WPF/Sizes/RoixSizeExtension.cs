using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Roix.Wpf.Extensions
{
    public static class RoixSizeExtension
    {
        public static RoixSize ActualSizeToRoixSize(this FrameworkElement fe) => new(fe.ActualWidth, fe.ActualHeight);

        public static RoixIntSize PixelSizeToRoixIntSize(this BitmapSource bitmap) => new(bitmap.PixelWidth, bitmap.PixelHeight);
        public static RoixSize PixelSizeToRoixSize(this BitmapSource bitmap) => new(bitmap.PixelWidth, bitmap.PixelHeight);

    }
}
