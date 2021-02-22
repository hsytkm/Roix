using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Roix.Wpf;
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
        public IReadOnlyReactiveProperty<RoixRect> SelectedRectangle { get; }

        public IReactiveProperty<RoixGaugePoint> MouseRightDownPoint { get; }
        public IReadOnlyReactiveProperty<RoixRect> ClickedFixedRectangle { get; }

        public SelectRectangleViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixGaugePoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixGaugePoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixGaugePoint>();
            MouseRightDownPoint = new ReactivePropertySlim<RoixGaugePoint>(mode: ReactivePropertyMode.None);

            var draggingVector = new ReactivePropertySlim<RoixVector>(mode: ReactivePropertyMode.None);

            // マウス操作開始時の初期化
            MouseLeftDownPoint.Subscribe(_ => draggingVector.Value = RoixVector.Zero);

            // マウス操作中に移動量を流す + 操作完了時に枠位置を通知する
            MouseMovePoint
                .Pairwise()
                .Select(x => x.NewItem.Point - x.OldItem.Point)
                .SkipUntil(MouseLeftDownPoint.ToUnit())
                .TakeUntil(MouseLeftUpPoint.ToUnit())
                .Finally(() =>
                {
                    //Debug.WriteLine($"LeftTop: {MouseDownPoint.Value:f2}, Vector: {draggingVector:f2}");
                    if (draggingVector.Value.IsZero) return;

                    // 枠の確定
                    Debug.WriteLine("end");
                })
                .Repeat()
                .Subscribe(v => draggingVector.Value += v);

            // 選択枠のプレビュー
            SelectedRectangle = draggingVector
                .Where(vec => !vec.IsZero)
                .Select(vec => MouseLeftDownPoint.Value.Add(vec).GetClippedRoi(isPointPriority: true))
                .ToReadOnlyReactivePropertySlim();

            // 右クリックで固定サイズの枠を描画
            ClickedFixedRectangle = MouseRightDownPoint
                .Where(x => !x.Canvas.IsZero)
                .Select(rgp =>
                {
                    var size = new RoixSize(100, 100);
                    var point = rgp.Point - (RoixVector)(size / 2);
                    var rect = new RoixGaugeRect(new RoixRect(point, size), rgp.Canvas);
                    return rect.GetClippedRoi(isPointPriority: false);
                })
                .ToReadOnlyReactivePropertySlim();

        }

    }

}
