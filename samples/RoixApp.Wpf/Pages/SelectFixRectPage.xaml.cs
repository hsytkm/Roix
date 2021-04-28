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
    public partial class SelectFixRectPage : UserControl
    {
        public SelectFixRectPage()
        {
            InitializeComponent();
        }
    }

    public class SelectFixRectViewModel : BindableBase
    {
        public static BitmapSource MyImage { get; } = App.Current.SourceImages.Image1;
        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        public IReadOnlyReactiveProperty<RoixRect> ClickedFixedRectangle { get; }
        public IReadOnlyReactiveProperty<RoixIntRect> ClickedFixedRectangleToModel { get; }

        public SelectFixRectViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);

            var imageSourceSize = MyImage.ToRoixIntSize();

            // 画像座標系の固定サイズ枠(これを基準に管理する) 右クリックで固定サイズの枠を描画する
            var clickedFixedRectangleOnImage = MouseLeftDownPoint
                .Where(border => border.Border.IsNotZero)
                .Select(borderPointOnView =>
                {
                    var length = 100;
                    var rectBorderSize = new RoixIntSize(length).ToRoixBorder(imageSourceSize);
                    var rectHalfSize = rectBorderSize.Size / 2d;

                    var newCenterPoint = borderPointOnView.ConvertToNewBorderInt(imageSourceSize);
                    var newRect = new RoixBorderIntRect(newCenterPoint - (RoixIntVector)rectHalfSize, rectBorderSize);
                    return newRect.GetClippedBorderIntRect(isPointPriority: false);
                })
                .ToReadOnlyReactivePropertySlim();

            // Model通知
            ClickedFixedRectangleToModel = clickedFixedRectangleOnImage
                .Where(border => border.Border.IsNotZero)
                .Select(borderRectOnView => borderRectOnView.Roi)
                .ToReadOnlyReactivePropertySlim();

            // View座標系の選択枠
            ClickedFixedRectangle = clickedFixedRectangleOnImage
                .CombineLatest(ViewBorderSize, (rect, border) => rect.ConvertToNewBorder(border).Roi)
                .ToReadOnlyReactivePropertySlim();

        }
    }

}
