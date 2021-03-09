using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xunit;

namespace Roix.WPF.Tests
{
    public class RoixSizeExtensionTest
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, 10)]
        public async void ActualSizeToRoixSize(int width, int height)
        {
            FrameworkElement GetFrameworkElement()
            {
                var fe = new Ellipse
                {
                    Width = width,
                    Height = height,
                };
                fe.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                fe.Arrange(new Rect(fe.DesiredSize));
                return fe;
            };
            var fe = await STATask.Run(GetFrameworkElement);

            var size1 = fe.ActualSizeToRoix();
            size1.Width.Is(width);
            size1.Height.Is(height);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, 10)]
        public void PixelSizeToRoixIntSize(int width, int height)
        {
            var bitmap = CreateBitmapSource(width, height);
            var size1 = bitmap.PixelSizeToRoixInt();
            var size2 = bitmap.PixelSizeToRoix();
            size1.Width.Is(width);
            size1.Height.Is(height);
            size2.Is((RoixSize)size1);

            static BitmapSource CreateBitmapSource(int width, int height)
            {
                var pf = PixelFormats.Bgr24;
                var rawStride = (width * pf.BitsPerPixel + 7) / 8;
                var rawImage = new byte[rawStride * height];
                return BitmapSource.Create(width, height, 96, 96, pf, null, rawImage, rawStride);
            }
        }

    }

    // STAÇ≈é¿çsÇ≥ÇÍÇÈTaskÇÃê∂ê¨  https://qiita.com/tricogimmick/items/f3afc94e7133e9c641a7
    class STATask
    {
        public static Task<T> Run<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            var thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        public static Task Run(Action act)
        {
            return Run(() =>
            {
                act();
                return true;
            });
        }
    }
}
