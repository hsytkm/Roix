using System;
using System.Windows.Media.Imaging;

namespace RoixApp.Wpf.Model
{
    class SourceImages
    {
        public BitmapSource Image1 { get; } = CreateBitmapFrame("pack://application:,,,/RoixApp.Wpf;component/Assets/Image1.jpg");
        public BitmapSource Image16x16 { get; } = CreateBitmapFrame("pack://application:,,,/RoixApp.Wpf;component/Assets/Image16x16.jpg");

        private static BitmapFrame CreateBitmapFrame(string resourceUriString) => BitmapFrame.Create(new Uri(resourceUriString));
    }
}
