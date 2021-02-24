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
        public IReactiveProperty<RoixGaugePoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixGaugePoint> MouseLeftUpPoint { get; }
        public IReactiveProperty<RoixGaugePoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        public IReadOnlyReactiveProperty<RoixGaugePoint> CursorGaugePoint { get; }
        public IReadOnlyReactiveProperty<RoixIntPoint> CursorPointToModel { get; }

        public IReactiveProperty<RoixGaugeRect> SelectedGaugeRectangle { get; }
        public IReactiveProperty<RoixIntRect> SelectedRectangleToModel { get; }

        public IReactiveProperty<RoixGaugePoint> MouseRightDownPoint { get; }
        public IReactiveProperty<RoixGaugeRect> ClickedFixedGaugeRectangle { get; }
        public IReadOnlyReactiveProperty<RoixIntRect> ClickedFixedRectangleToModel { get; }

        public SelectRectangleViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixGaugePoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixGaugePoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixGaugePoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>();
            MouseRightDownPoint = new ReactivePropertySlim<RoixGaugePoint>(mode: ReactivePropertyMode.None);
            SelectedGaugeRectangle = new ReactivePropertySlim<RoixGaugeRect>();
            SelectedRectangleToModel = new ReactivePropertySlim<RoixIntRect>();
            ClickedFixedGaugeRectangle = new ReactivePropertySlim<RoixGaugeRect>();

            var imageSourceSize = MyImage.PixelSizeToRoixSize();

            #region CursorPoint
            CursorGaugePoint = MouseMovePoint.ToReadOnlyReactivePropertySlim();

            CursorPointToModel = CursorGaugePoint
                .Where(gp => !gp.IsZero)
                .Select(gp => gp.ConvertToNewGauge(imageSourceSize).ToRoixIntPoint(isCheckBoundaries: false))
                .ToReadOnlyReactivePropertySlim();
            #endregion

            #region SelectedRectangle
            // マウス操作中に枠を更新 + 操作完了時に枠位置を通知する
            MouseMovePoint
                .SkipUntil(MouseLeftDownPoint.ToUnit())
                .TakeUntil(MouseLeftUpPoint.ToUnit())
                .Finally(() =>
                {
                    if (MouseLeftDownPoint.Value == MouseLeftUpPoint.Value) return;

                    // ドラッグ枠の確定
                    var gaugeRectOnView = new RoixGaugeRect(MouseLeftDownPoint.Value, MouseLeftUpPoint.Value);
                    var gaugeRectOnModel = gaugeRectOnView.ConvertToNewGauge(imageSourceSize);
                    SelectedRectangleToModel.Value = gaugeRectOnModel.GetClippedGaugeRect().ToRoixIntRect(isCheckBoundaries: true);
                })
                .Repeat()
                .Select(lastestPoint => lastestPoint - MouseLeftDownPoint.Value)
                .Where(gaugeVector => !gaugeVector.IsZero)
                .Subscribe(gaugeVector => SelectedGaugeRectangle.Value = MouseLeftDownPoint.Value.CreateRoixGaugeRect(gaugeVector.Vector).GetClippedGaugeRect());
            #endregion

            #region ClickedFixedRectangle
            // 右クリックで固定サイズの枠を描画
            MouseRightDownPoint
                .Where(gauge => !gauge.Border.IsZero)
                .Subscribe(gaugePointOnView =>
                {
                    var gaugeSizeOnModel = new RoixGaugeSize(new RoixSize(100, 100), imageSourceSize);
                    var borderOnView = gaugePointOnView.Border;
                    var gaugeSizeOnView = gaugeSizeOnModel.ConvertToNewGauge(borderOnView);
                    var newPointOnView = gaugePointOnView.Point - (RoixVector)(gaugeSizeOnView.Size / 2);
                    var gaugeRectOnView = new RoixGaugeRect(new RoixRect(newPointOnView, gaugeSizeOnView.Size), borderOnView);
                    ClickedFixedGaugeRectangle.Value = gaugeRectOnView.GetClippedGaugeRect(isPointPriority: false);
                });

            // Model通知用にView座標系から元画像の座標系に正規化
            ClickedFixedRectangleToModel = ClickedFixedGaugeRectangle
                .Where(gauge => !gauge.Border.IsZero)
                .Select(gaugeRectOnView => gaugeRectOnView.ConvertToNewGauge(imageSourceSize).ToRoixIntRect(isCheckBoundaries: false))
                .ToReadOnlyReactivePropertySlim();
            #endregion

            // View画像サイズの変更に応じて枠を伸縮
            ViewBorderSize
                .Where(size => !size.IsZero)
                .Subscribe(newBorder =>
                {
                    SelectedGaugeRectangle.Value = SelectedGaugeRectangle.Value.ConvertToNewGauge(newBorder);
                    ClickedFixedGaugeRectangle.Value = ClickedFixedGaugeRectangle.Value.ConvertToNewGauge(newBorder);
                });

        }
    }

}
