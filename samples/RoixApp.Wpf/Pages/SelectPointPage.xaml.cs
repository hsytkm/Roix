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
    public partial class SelectPointPage : UserControl
    {
        public SelectPointPage()
        {
            InitializeComponent();
        }
    }

    public class SelectPointViewModel : BindableBase
    {
        public static BitmapSource MyImage { get; } = App.Current.SourceImages.Image1;
        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        public IReadOnlyReactiveProperty<RoixBorderPoint> CursorBorderPoint { get; }
        public IReadOnlyReactiveProperty<RoixIntPoint> CursorPointToModel { get; }

        public IReadOnlyReactiveProperty<RoixPoint> SinglePoint { get; }
        public IReadOnlyReactiveProperty<RoixIntPoint> SinglePointToModel { get; }

        public SelectPointViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);

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
                .ToReadOnlyReactivePropertySlim(mode: ReactivePropertyMode.None);

            // 画像座標系の点(これを基準に管理する)
            var selectedPointOnImage = mouseDoubleClickPoint
                .Select(borderPoint =>
                {
                    var borderInt = borderPoint.ConvertToNewBorderInt(imageSourceSize);
                    return borderInt.ClipToSize(borderInt.Border - 1);
                })
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

        }
    }

}
