using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Roix.Wpf.Extensions
{
    public static class RoixSizeExtension
    {
        public static RoixSize ToRoixSize(this FrameworkElement fe) => new(fe.ActualWidth, fe.ActualHeight);

        public static RoixIntSize ToRoixIntSize(this BitmapSource bitmap) => new(bitmap.PixelWidth, bitmap.PixelHeight);
        public static RoixSize ToRoixSize(this BitmapSource bitmap) => new(bitmap.PixelWidth, bitmap.PixelHeight);

    }
}
