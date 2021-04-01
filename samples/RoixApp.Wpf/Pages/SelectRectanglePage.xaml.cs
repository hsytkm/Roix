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
    public partial class SelectRectanglePage : UserControl
    {
        public SelectRectanglePage()
        {
            InitializeComponent();
        }
    }

    public class SelectRectangleViewModel : BindableBase
    {
        public static BitmapSource MyImage { get; } = App.Current.SourceImages.Image1;
        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseLeftUpPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        public IReadOnlyReactiveProperty<RoixBorderPoint> CursorBorderPoint { get; }
        public IReadOnlyReactiveProperty<RoixIntPoint> CursorPointToModel { get; }

        public IReadOnlyReactiveProperty<RoixPoint> SinglePoint { get; }
        public IReadOnlyReactiveProperty<RoixIntPoint> SinglePointToModel { get; }

        public IReadOnlyReactiveProperty<RoixRect> SelectedRectangle { get; }
        public IReactiveProperty<RoixIntRect> SelectedRectangleToModel { get; }

        public IReactiveProperty<RoixBorderPoint> MouseRightDownPoint { get; }
        public IReadOnlyReactiveProperty<RoixRect> ClickedFixedRectangle { get; }
        public IReadOnlyReactiveProperty<RoixIntRect> ClickedFixedRectangleToModel { get; }

        public SelectRectangleViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);
            MouseRightDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            SelectedRectangleToModel = new ReactivePropertySlim<RoixIntRect>();

            var imageSourceSize = MyImage.ToRoixIntSize();

            #region CursorPoint
            CursorBorderPoint = MouseMovePoint.ToReadOnlyReactivePropertySlim();

            CursorPointToModel = CursorBorderPoint
                .Where(borderPoint => borderPoint.IsNotZero)
                .Select(borderPoint => borderPoint.ConvertToNewBorderInt(imageSourceSize).Point)
                .ToReadOnlyReactivePropertySlim();
            #endregion

            #region DoubleClickPoint
            var eventAcceptedTime = DateTime.Now;
            var mouseDoubleClickPoint = MouseLeftDownPoint
                .TimeInterval()
                .Skip(1)
                .Where(ti =>
                {
                    // 前回の MouseDown から一定時間が経過していればダブクリと言わない
                    if (ti.Interval > TimeSpan.FromMilliseconds(500)) return false;

                    var now = DateTime.Now;

                    // 前回のダブクリ受付から一定時間が経過するまでは、次のダブクリを受け付けない
                    if (now - eventAcceptedTime < TimeSpan.FromMilliseconds(500)) return false;

                    eventAcceptedTime = now;    // ダブクリ受付時間の更新
                    return true;
                })
                .Select(x => x.Value)
                .ToReadOnlyReactivePropertySlim();

            // 画像座標系の点(これを基準に管理する)
            var selectedPointOnImage = mouseDoubleClickPoint
                .Select(borderPoint => borderPoint.ConvertToNewBorderInt(imageSourceSize))
                .ToReadOnlyReactivePropertySlim();

            SinglePoint = selectedPointOnImage
                .CombineLatest(ViewBorderSize, (intPoint, viewSize) =>
                {
                    var leftTop = intPoint.ConvertToNewBorder(viewSize).Point;
                    var halfPixelSize = new RoixIntSize(1).ToRoixBorder(imageSourceSize).ConvertToNewBorder(viewSize).Size / 2d;
                    return leftTop + (RoixVector)halfPixelSize;     // 画素の中央部に点を打つためシフト
                })
                .ToReadOnlyReactivePropertySlim();

            SinglePointToModel = selectedPointOnImage
                .Where(borderPoint => borderPoint.Border.IsNotZero)
                .Select(borderRectOnView => borderRectOnView.Point)
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
                    if (startPoint == default || latestPoint == default || startPoint == latestPoint) return;

                    SelectedRectangleToModel.Value = RoixBorderIntRect.Create(startPoint, latestPoint, imageSourceSize).Roi;
                })
                .Repeat()
                .Select(x => RoixBorderIntRect.Create(x.startPoint, x.latestPoint, imageSourceSize))
                .ToReadOnlyReactivePropertySlim();

            // View座標系の選択枠(length=0 だと View の Polygon が変に長くなるので minLength で Clip する)
            SelectedRectangle = selectedRectangleOnImage
                .CombineLatest(ViewBorderSize, (rect, border)
                    => rect.ConvertToNewBorder(border).ClipByMinimumSize(new RoixSize(1, 1)).Roi)
                .ToReadOnlyReactivePropertySlim();
            #endregion

            #region ClickedFixedRectangle
            // 画像座標系の固定サイズ枠(これを基準に管理する) 右クリックで固定サイズの枠を描画する
            var clickedFixedRectangleOnImage = MouseRightDownPoint
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
            #endregion

        }
    }

}
