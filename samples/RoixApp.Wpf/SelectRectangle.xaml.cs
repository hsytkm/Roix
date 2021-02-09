using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Roix.Core;
using Roix.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Roix.Wpf
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
        public IReactiveProperty<RoixPointDouble> MouseDownPoint { get; }
        public IReactiveProperty<RoixPointDouble> MouseUpPoint { get; }
        public IReactiveProperty<RoixPointDouble> MouseMovePoint { get; }
        public IReactiveProperty<RoixSizeDouble> ViewBorderSize { get; }
        public IReactiveProperty<RoixRectDouble> SelectedRectangle { get; }

        public SelectRectangleViewModel()
        {
            MouseDownPoint = new ReactivePropertySlim<RoixPointDouble>(mode: ReactivePropertyMode.None);
            MouseUpPoint = new ReactivePropertySlim<RoixPointDouble>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixPointDouble>();
            ViewBorderSize = new ReactivePropertySlim<RoixSizeDouble>();
            SelectedRectangle = new ReactivePropertySlim<RoixRectDouble>(initialValue: RoixRectDouble.Zero, mode: ReactivePropertyMode.DistinctUntilChanged);

            var draggingVector = new ReactivePropertySlim<RoixVectorDouble>(mode: ReactivePropertyMode.None);

            // マウス操作開始時の初期化
            MouseDownPoint.Subscribe(_ => draggingVector.Value = RoixVectorDouble.Zero);

            // マウス操作中に移動量を流す + 操作完了時に枠位置を通知する
            MouseMovePoint
                .Pairwise()
                .Where(x => x.NewItem is not null && x.OldItem is not null)
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
        private static RoixRectDouble ClipRectangle(in RoixRectDouble roi, in RoixSizeDouble bounds)
            => new RoixBoundsRoiDouble(roi, bounds).GetClippedRoi();

    }

}
