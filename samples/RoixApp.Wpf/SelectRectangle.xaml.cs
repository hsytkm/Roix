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

            var imageSourceSize = MyImage.PixelSizeToRoixIntSize();

            #region CursorPoint
            CursorBorderPoint = MouseMovePoint.ToReadOnlyReactivePropertySlim();

            CursorPointToModel = CursorBorderPoint
                .Where(borderPoint => borderPoint.IsNotZero)
                .Select(borderPoint => borderPoint.ConvertToRoixInt(imageSourceSize).Point)
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

                    // ドラッグ枠確定後
                    var intStartPoint = startPoint.ConvertToRoixInt(imageSourceSize);
                    var intLatestPoint = latestPoint.ConvertToRoixInt(imageSourceSize);
                    var intRect = new RoixBorderIntRect(intStartPoint, intLatestPoint);
                    SelectedRectangleToModel.Value = intRect.Roi;
                })
                .Repeat()
                .Subscribe(x =>
                {
                    var intStartPoint = x.startPoint.ConvertToRoixInt(imageSourceSize);
                    var intLatestPoint = x.latestPoint.ConvertToRoixInt(imageSourceSize);
                    var intRect = new RoixBorderIntRect(intStartPoint, intLatestPoint);
                    var viewRect = intRect.ConvertToNewBorder(x.startPoint.Border);
                    SelectedBorderRectangle.Value = viewRect;
                });
            #endregion

            #region ClickedFixedRectangle
            // 右クリックで固定サイズの枠を描画
            MouseRightDownPoint
                .Where(border => border.Border.IsNotZero)
                .Subscribe(borderPointOnView =>
                {
                    var length = 100;
                    var borderOnView = borderPointOnView.Border;

                    var borderSizeOnModel = new RoixBorderIntSize(new RoixIntSize(length), imageSourceSize);
                    var borderHalfSizeOnModel = new RoixBorderIntSize(borderSizeOnModel.Size / 2, borderSizeOnModel.Border);
                    var borderSizeOnView = borderSizeOnModel.ConvertToNewBorder(borderOnView);
                    var borderHalfSizeOnView = borderHalfSizeOnModel.ConvertToNewBorder(borderOnView);

                    var newPointOnView = borderPointOnView.ConvertToRoixInt(imageSourceSize);
                    var newPointOnModel = newPointOnView.ConvertToNewBorder(borderOnView);
                    var newLeftTopPointOnModel = (RoixPoint)(newPointOnModel.Point - (RoixPoint)borderHalfSizeOnView.Size);

                    var borderRectOnView = new RoixBorderRect(new(newLeftTopPointOnModel, borderSizeOnView.Size), borderSizeOnView.Border);
                    ClickedFixedBorderRectangle.Value = borderRectOnView.GetClippedBorderRect(isPointPriority: false);
                });

            // Model通知用にView座標系から元画像の座標系に正規化
            ClickedFixedRectangleToModel = ClickedFixedBorderRectangle
                .Where(border => border.Border.IsNotZero)
                .Select(borderRectOnView => borderRectOnView.ConvertToRoixInt(imageSourceSize, mode: RoundingMode.Round).Roi)
                .ToReadOnlyReactivePropertySlim();
            #endregion

            // View画像サイズの変更に応じて枠を伸縮
            ViewBorderSize
                .Where(size => size.IsNotZero)
                .Subscribe(newBorder =>
                {
                    static RoixBorderRect Convert(in RoixBorderRect srcBorderRect, in RoixIntSize intSize, in RoixSize newSize)
                    {
                        var intRect = srcBorderRect.ConvertToRoixInt(intSize, mode: RoundingMode.Round);
                        var viewRect = intRect.ConvertToNewBorder(newSize);
                        return viewRect;
                    }

                    SelectedBorderRectangle.Value = Convert(SelectedBorderRectangle.Value, imageSourceSize, newBorder);
                    ClickedFixedBorderRectangle.Value = Convert(ClickedFixedBorderRectangle.Value, imageSourceSize, newBorder);
                });

        }
    }

}
