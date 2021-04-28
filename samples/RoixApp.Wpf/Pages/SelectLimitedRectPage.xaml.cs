using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoixApp.Wpf
{
    public partial class SelectLimitedRectPage : UserControl
    {
        public SelectLimitedRectPage()
        {
            InitializeComponent();
        }
    }

    public class SelectLimitedRectViewModel : BindableBase
    {
        public static BitmapSource MyImage { get; } = App.Current.SourceImages.Image1;
        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseLeftUpPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        public IReadOnlyReactiveProperty<RoixRect> PreviewRectangle { get; }
        public IReadOnlyReactiveProperty<RoixRect> SelectedRectangle { get; }
        public IReactiveProperty<RoixIntRect> SelectedRectangleToModel { get; }

        public SelectLimitedRectViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);
            SelectedRectangleToModel = new ReactivePropertySlim<RoixIntRect>();

            var imageSourceSize = MyImage.ToRoixIntSize();

            // 画像座標系の選択枠(これを基準に管理する) マウス操作中に枠を更新 + 操作完了時に枠位置を通知する
            var selectedRectangleOnImage = MouseMovePoint
                .Select(latestPoint => (startPoint: MouseLeftDownPoint.Value, latestPoint))
                .Where(x => x.startPoint != x.latestPoint)
                .SkipUntil(MouseLeftDownPoint.ToUnit())
                .TakeUntil(MouseLeftUpPoint.ToUnit())
                .Finally(() =>
                {
                    var (startPoint, latestPoint) = (MouseLeftDownPoint.Value, MouseLeftUpPoint.Value);
                    if (startPoint == default || latestPoint == default || startPoint == latestPoint) return;

                    SelectedRectangleToModel.Value = CreateBorderIntRect(startPoint, latestPoint, imageSourceSize).Roi;
                })
                .Repeat()
                .Select(x => RoixBorderIntRect.Create(x.startPoint, x.latestPoint, imageSourceSize))
                .ToReadOnlyReactivePropertySlim();

            // View座標系の選択枠
            PreviewRectangle = selectedRectangleOnImage
                .CombineLatest(ViewBorderSize, (rect, border) => rect.ConvertToNewBorder(border).Roi)
                .ToReadOnlyReactivePropertySlim();

            // View座標系の選択枠
            SelectedRectangle = SelectedRectangleToModel
                .CombineLatest(ViewBorderSize, (rect, border) => new RoixBorderRect(rect, imageSourceSize).ConvertToNewBorder(border).Roi)
                .ToReadOnlyReactivePropertySlim();
        }

        // 2点から作成する四角形を 最大/最小サイズ で制限する
        private static RoixBorderIntRect CreateBorderIntRect(in RoixBorderPoint startBorderPoint, in RoixBorderPoint endBorderPoint, in RoixIntSize sourceImageSize)
        {
            (var widthMin, var heightMin) = (100, 50);
            (var widthMax, var heightMax) = (400, 200);

            var startBorderIntPoint = startBorderPoint.ConvertToNewBorderInt(sourceImageSize);
            var endPoint = endBorderPoint.ConvertToNewBorderInt(sourceImageSize).Point;

            // 終了点を開始点を原点に Vector 換算してから、水平/垂直の長さで制限する
            var clippedEndVector = (endPoint - startBorderIntPoint.Point).LimitNear(widthMin, heightMin).LimitFar(widthMax, heightMax);

            return new RoixBorderIntRect(startBorderIntPoint, clippedEndVector).ClipToBorder();
        }
    }
}
