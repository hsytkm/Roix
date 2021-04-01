using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoixApp.Wpf
{
    public partial class ResizeRectanglePage : UserControl
    {
        public ResizeRectanglePage()
        {
            InitializeComponent();
        }
    }

    public class ResizeRectangleViewModel : BindableBase
    {
        private readonly ResizeRectangleModel _model = new();
        public BitmapSource MyImage => _model.MyImage;
        public IReactiveProperty<RoixIntSize> ImageSourceSize { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }
        public IReactiveProperty<RoixBorderRect> SelectedRectangle { get; }

        public IReadOnlyReactiveProperty<string> RectOnImage { get; }

        public ResizeRectangleViewModel()
        {
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);
            SelectedRectangle = new ReactivePropertySlim<RoixBorderRect>(mode: ReactivePropertyMode.None);
            ImageSourceSize = new ReactivePropertySlim<RoixIntSize>(initialValue: MyImage.ToRoixIntSize(), mode: ReactivePropertyMode.DistinctUntilChanged);

            // Model通知
            SelectedRectangle
                .Subscribe(rect => _model.Rect.Value = rect.ConvertToNewBorderInt(ImageSourceSize.Value, RoundingMode.Round));

            // Viewサイズ変化に追従
            _model.Rect
                .CombineLatest(ViewBorderSize.Where(x => x.IsNotZero), (rect, border) => rect.ConvertToNewBorder(border))
                .Subscribe(borderRect => SelectedRectangle.Value = borderRect);

            RectOnImage = _model.Rect
                .Select(rect => $"X={rect.Roi.X}, Y={rect.Roi.Y}, Width={rect.Roi.Width}, Height={rect.Roi.Height}")
                .ToReadOnlyReactivePropertySlim<string>();
        }
    }

    class ResizeRectangleModel : BindableBase
    {
        public BitmapSource MyImage { get; } = App.Current.SourceImages.Image16x16;
        public IReactiveProperty<RoixBorderIntRect> Rect { get; }

        public ResizeRectangleModel()
        {
            var imageSourceSize = MyImage.ToRoixIntSize();
            var initialRect = new RoixIntRect(1, 2, 2, 2).ToRoixBorder(imageSourceSize);

            Rect = new ReactivePropertySlim<RoixBorderIntRect>(initialValue: initialRect);
            //Rect.Subscribe(x => Debug.WriteLine($"Model Rect: {x}"));
        }
    }

}
