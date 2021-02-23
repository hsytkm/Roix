using Roix.Wpf;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RoixApp.Wpf.Extensions
{
    static class RoixSizeExtension
    {
        public static RoixSize ToRoixSize(this FrameworkElement fe) => new RoixSize(fe.ActualWidth, fe.ActualHeight);
        public static RoixSize ToRoixSize(this BitmapSource bitmap) => new RoixSize(bitmap.PixelWidth, bitmap.PixelHeight);

    }
}
