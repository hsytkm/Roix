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
        public IReactiveProperty<RoixPoint> MouseDownPoint { get; }
        public IReactiveProperty<RoixPoint> MouseUpPoint { get; }
        public IReactiveProperty<RoixPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }
        public IReactiveProperty<RoixRect> SelectedRectangle { get; }

        public SelectRectangleViewModel()
        {
            MouseDownPoint = new ReactivePropertySlim<RoixPoint>(mode: ReactivePropertyMode.None);
            MouseUpPoint = new ReactivePropertySlim<RoixPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>();
            SelectedRectangle = new ReactivePropertySlim<RoixRect>(initialValue: RoixRect.Zero, mode: ReactivePropertyMode.DistinctUntilChanged);

            var draggingVector = new ReactivePropertySlim<RoixVector>(mode: ReactivePropertyMode.None);

            // マウス操作開始時の初期化
            MouseDownPoint.Subscribe(_ => draggingVector.Value = RoixVector.Zero);

            // マウス操作中に移動量を流す + 操作完了時に枠位置を通知する
            MouseMovePoint
                .Pairwise()
                .Select(x => x.NewItem - x.OldItem)
                .SkipUntil(MouseDownPoint.ToUnit())
                .TakeUntil(MouseUpPoint.ToUnit())
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
            draggingVector
                .Where(v => !v.IsZero)
                .Select(v => ClipRectangle(new(MouseDownPoint.Value, v), ViewBorderSize.Value))
                .Subscribe(r => SelectedRectangle.Value = r);

        }

        /// <summary>引数の四角形を指定範囲に制限する</summary>
        private static RoixRect ClipRectangle(in RoixRect roi, in RoixSize bounds)
            => new RoixGaugeRect(roi, bounds).GetClippedRoi();

    }

}
