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

        public IReactiveProperty<RoixBorderRect> SelectedBorderRectangle { get; }
        public IReactiveProperty<RoixIntRect> SelectedRectangleToModel { get; }

        public IReactiveProperty<RoixBorderPoint> MouseRightDownPoint { get; }
        public IReactiveProperty<RoixBorderRect> ClickedFixedBorderRectangle { get; }
        public IReadOnlyReactiveProperty<RoixIntRect> ClickedFixedRectangleToModel { get; }

        public SelectRectangleViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>();
            MouseRightDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            SelectedBorderRectangle = new ReactivePropertySlim<RoixBorderRect>();
            SelectedRectangleToModel = new ReactivePropertySlim<RoixIntRect>();
            ClickedFixedBorderRectangle = new ReactivePropertySlim<RoixBorderRect>();

            var imageSourceSize = MyImage.PixelSizeToRoixSize();

            #region CursorPoint
            CursorBorderPoint = MouseMovePoint.ToReadOnlyReactivePropertySlim();

            CursorPointToModel = CursorBorderPoint
                .Where(bp => bp.IsNotZero)
                .Select(bp => bp.ConvertToNewBorder(imageSourceSize).ToRoixIntPoint(isCheckBorder: false))
                .ToReadOnlyReactivePropertySlim();
            #endregion

            #region SelectedRectangle
            // マウス操作中に枠を更新 + 操作完了時に枠位置を通知する
            MouseMovePoint
                .Select(latestPoint => (startPoint: MouseLeftDownPoint.Value, latestPoint))
                .SkipUntil(MouseLeftDownPoint.ToUnit())
                .TakeUntil(MouseLeftUpPoint.ToUnit())
                .Finally(() =>
                {
                    var (startPoint, latestPoint) = (MouseLeftDownPoint.Value, MouseLeftUpPoint.Value);
                    if (startPoint == latestPoint) return;

                    // ドラッグ枠の確定
                    var borderRectOnView = new RoixBorderRect(startPoint, latestPoint);
                    var borderRectOnModel = borderRectOnView.ConvertToNewBorder(imageSourceSize);
                    SelectedRectangleToModel.Value = borderRectOnModel.GetClippedBorderRect().ToRoixIntRect(isCheckBorder: true);
                })
                .Repeat()
                .Subscribe(x => SelectedBorderRectangle.Value = new RoixBorderRect(x.latestPoint, x.startPoint).GetClippedBorderRect());
            #endregion

            #region ClickedFixedRectangle
            // 右クリックで固定サイズの枠を描画
            MouseRightDownPoint
                .Where(border => border.Border.IsNotZero)
                .Subscribe(borderPointOnView =>
                {
                    var borderSizeOnModel = new RoixBorderSize(new RoixSize(100, 100), imageSourceSize);
                    var borderOnView = borderPointOnView.Border;
                    var borderSizeOnView = borderSizeOnModel.ConvertToNewBorder(borderOnView);
                    var newPointOnView = borderPointOnView.Point - (RoixVector)(borderSizeOnView.Size / 2);
                    var borderRectOnView = new RoixBorderRect(new RoixRect(newPointOnView, borderSizeOnView.Size), borderOnView);
                    ClickedFixedBorderRectangle.Value = borderRectOnView.GetClippedBorderRect(isPointPriority: false);
                });

            // Model通知用にView座標系から元画像の座標系に正規化
            ClickedFixedRectangleToModel = ClickedFixedBorderRectangle
                .Where(border => border.Border.IsNotZero)
                .Select(borderRectOnView => borderRectOnView.ConvertToNewBorder(imageSourceSize).ToRoixIntRect(isCheckBorder: false))
                .ToReadOnlyReactivePropertySlim();
            #endregion

            // View画像サイズの変更に応じて枠を伸縮
            ViewBorderSize
                .Where(size => size.IsNotZero)
                .Subscribe(newBorder =>
                {
                    SelectedBorderRectangle.Value = SelectedBorderRectangle.Value.ConvertToNewBorder(newBorder);
                    ClickedFixedBorderRectangle.Value = ClickedFixedBorderRectangle.Value.ConvertToNewBorder(newBorder);
                });

        }
    }

}
