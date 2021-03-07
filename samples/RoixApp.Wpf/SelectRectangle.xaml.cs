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
    public partial class SelectRectangle : UserControl
    {
        public SelectRectangle()
        {
            InitializeComponent();
        }
    }

    public class SelectRectangleViewModel : BindableBase
    {
        public BitmapSource MyImage { get; } = BitmapFrame.Create(new Uri("pack://application:,,,/RoixApp.Wpf;component/Assets/Image1.jpg"));
        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseLeftUpPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        public IReadOnlyReactiveProperty<RoixBorderPoint> CursorBorderPoint { get; }
        public IReadOnlyReactiveProperty<RoixIntPoint> CursorPointToModel { get; }

        public IReadOnlyReactiveProperty<RoixBorderRect> SelectedBorderRectangle { get; }
        public IReactiveProperty<RoixIntRect> SelectedRectangleToModel { get; }

        public IReactiveProperty<RoixBorderPoint> MouseRightDownPoint { get; }
        public IReadOnlyReactiveProperty<RoixBorderRect> ClickedFixedBorderRectangle { get; }
        public IReadOnlyReactiveProperty<RoixIntRect> ClickedFixedRectangleToModel { get; }

        public SelectRectangleViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>();
            MouseRightDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            SelectedRectangleToModel = new ReactivePropertySlim<RoixIntRect>();

            var imageSourceSize = MyImage.PixelSizeToRoixIntSize();

            #region CursorPoint
            CursorBorderPoint = MouseMovePoint.ToReadOnlyReactivePropertySlim();

            CursorPointToModel = CursorBorderPoint
                .Where(borderPoint => borderPoint.IsNotZero)
                .Select(borderPoint => borderPoint.ConvertToRoixInt(imageSourceSize).Point)
                .ToReadOnlyReactivePropertySlim();
            #endregion

            #region SelectedRectangle
            // 画像座標系の選択枠(これを基準に管理する) マウス操作中に枠を更新 + 操作完了時に枠位置を通知する
            var selectedRectangleOnImage = MouseMovePoint
                .Select(latestPoint => (startPoint: MouseLeftDownPoint.Value, latestPoint))
                .SkipUntil(MouseLeftDownPoint.ToUnit())
                .TakeUntil(MouseLeftUpPoint.ToUnit())
                .Finally(() =>
                {
                    var (startPoint, latestPoint) = (MouseLeftDownPoint.Value, MouseLeftUpPoint.Value);
                    SelectedRectangleToModel.Value = RoixBorderIntRect.Create(startPoint, latestPoint, imageSourceSize).Roi;
                })
                .Repeat()
                .Select(x => RoixBorderIntRect.Create(x.startPoint, x.latestPoint, imageSourceSize))
                .ToReadOnlyReactivePropertySlim();

            // View座標系の選択枠
            SelectedBorderRectangle = selectedRectangleOnImage
                .CombineLatest(ViewBorderSize, (rect, border) => rect.ConvertToNewBorder(border))
                .ToReadOnlyReactivePropertySlim();
            #endregion

            #region ClickedFixedRectangle
            // 画像座標系の固定サイズ枠(これを基準に管理する) 右クリックで固定サイズの枠を描画する
            var clickedFixedRectangleOnImage = MouseRightDownPoint
                .Where(border => border.Border.IsNotZero)
                .Select(borderPointOnView =>
                {
                    var length = 3;
                    var borderSize = new RoixBorderIntSize(new RoixIntSize(length), imageSourceSize);
                    var borderShiftVector = (RoixIntVector)(borderSize.Size / 2);

                    var newCenterPoint = borderPointOnView.ConvertToRoixInt(imageSourceSize);
                    var newLeftTopPoint = newCenterPoint.Point - borderShiftVector;
                    var newBorderRect = new RoixBorderIntRect(new(newLeftTopPoint, borderSize.Size), imageSourceSize);
                    return newBorderRect.GetClippedBorderIntRect(isPointPriority: false);
                })
                .ToReadOnlyReactivePropertySlim();

            // Model通知
            ClickedFixedRectangleToModel = clickedFixedRectangleOnImage
                .Where(border => border.Border.IsNotZero)
                .Select(borderRectOnView => borderRectOnView.Roi)
                .ToReadOnlyReactivePropertySlim();

            // View座標系の選択枠
            ClickedFixedBorderRectangle = clickedFixedRectangleOnImage
                .CombineLatest(ViewBorderSize, (rect, border) => rect.ConvertToNewBorder(border))
                .ToReadOnlyReactivePropertySlim();
            #endregion

        }
    }

}
