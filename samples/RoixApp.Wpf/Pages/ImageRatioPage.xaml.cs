using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoixApp.Wpf
{
    public partial class ImageRatioPage : UserControl
    {
        public ImageRatioPage()
        {
            InitializeComponent();
        }
    }

    public class ImageRatioViewModel : BindableBase
    {
        private readonly ImageRatioModel _model = new();

        public BitmapSource MyImage { get; } = App.Current.SourceImages.Image1;
        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseLeftUpPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseRightDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        #region InputTexts (Rect)
        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+(\\.[0-9]{1,2})?$", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0.00, 100.0)]
        public ReactiveProperty<string> RectRatioX { get; }

        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+(\\.[0-9]{1,2})?$", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0.00, 100.0)]
        public ReactiveProperty<string> RectRatioY { get; }

        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+(\\.[0-9]{1,2})?$", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0.00, 100.0)]
        public ReactiveProperty<string> RectRatioWidth { get; }

        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+(\\.[0-9]{1,2})?$", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0.00, 100.0)]
        public ReactiveProperty<string> RectRatioHeight { get; }
        #endregion

        #region Rect
        public IReadOnlyReactiveProperty<RoixRect> PreviewRectangle { get; }
        public IReadOnlyReactiveProperty<RoixRect> SelectedRectangle { get; }
        #endregion

        #region Point
        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+(\\.[0-9]{1,2})?$", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0.00, 100.0)]
        public ReactiveProperty<string> PointRatioX { get; }

        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+(\\.[0-9]{1,2})?$", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0.00, 100.0)]
        public ReactiveProperty<string> PointRatioY { get; }

        public IReadOnlyReactiveProperty<RoixPoint> SelectedPoint { get; }
        #endregion

        public ImageRatioViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseRightDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);
            var imageSourceSize = MyImage.ToRoixIntSize();

            #region InputTexts (Rect)
            RectRatioX = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioX);
            RectRatioY = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioY);
            RectRatioWidth = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioWidth);
            RectRatioHeight = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioHeight);

            // Modelからの値をViewに反映
            _model.RectRatio
                .Subscribe(ratio =>
                {
                    RectRatioX.Value = (ratio.X * 100d).ToString("f2");
                    RectRatioY.Value = (ratio.Y * 100d).ToString("f2");
                    RectRatioWidth.Value = (ratio.Width * 100d).ToString("f2");
                    RectRatioHeight.Value = (ratio.Height * 100d).ToString("f2");
                });

            // 入力値から枠を作成（個々の入力値は検証してるけど、相関検証は未）
            new[] { RectRatioX, RectRatioY, RectRatioWidth, RectRatioHeight }
                .Select(rp => rp.ObserveHasErrors)
                .CombineLatestValuesAreAllFalse()
                .Where(noError => noError)
                .Throttle(TimeSpan.FromMilliseconds(10))    // マウス操作時に複数回変更が発生するので落ち着いたら流す
                .Subscribe(_ =>
                {
                    var x = double.Parse(RectRatioX.Value, CultureInfo.InvariantCulture) / 100d;
                    var y = double.Parse(RectRatioY.Value, CultureInfo.InvariantCulture) / 100d;
                    var width = double.Parse(RectRatioWidth.Value, CultureInfo.InvariantCulture) / 100d;
                    var height = double.Parse(RectRatioHeight.Value, CultureInfo.InvariantCulture) / 100d;
                    _model.RectRatio.Value = new RoixRatioXYWH(x, y, width, height);
                });
            #endregion

            #region Rect
            // 画像座標系の選択枠(これを基準に管理する) マウス操作中に枠を更新 + 操作完了時に枠位置を通知する
            PreviewRectangle = MouseMovePoint
                .Select(latestPoint => (startPoint: MouseLeftDownPoint.Value, latestPoint))
                .Where(x => x.startPoint != x.latestPoint)
                .SkipUntil(MouseLeftDownPoint.ToUnit())
                .TakeUntil(MouseLeftUpPoint.ToUnit())
                .Finally(() =>
                {
                    var (startPoint, latestPoint) = (MouseLeftDownPoint.Value, MouseLeftUpPoint.Value);
                    if (startPoint == default || latestPoint == default || startPoint == latestPoint) return;

                    _model.RectRatio.Value = new RoixBorderRect(startPoint, latestPoint).GetClippedBorderRect().ToRoixRatio();
                })
                .Repeat()
                .Select(x => RoixBorderIntRect.Create(x.startPoint, x.latestPoint, imageSourceSize).ConvertToNewBorder(x.startPoint.Border).Roi)
                .ToReadOnlyReactivePropertySlim();

            // View座標系の選択枠
            SelectedRectangle = _model.RectRatio
                .CombineLatest(ViewBorderSize, (rect, viewSize) => (rect * viewSize).Roi)
                .ToReadOnlyReactivePropertySlim();
            #endregion

            #region InputTexts (Rect)
            PointRatioX = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => PointRatioX);
            PointRatioY = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => PointRatioY);

            // Modelからの値をViewに反映
            _model.PointRatio
                .Subscribe(rect =>
                {
                    PointRatioX.Value = (rect.X * 100d).ToString("f2");
                    PointRatioY.Value = (rect.Y * 100d).ToString("f2");
                });

            // 入力値から枠を作成（個々の入力値は検証してるけど、相関検証は未）
            new[] { PointRatioX, PointRatioY }
                .Select(rp => rp.ObserveHasErrors)
                .CombineLatestValuesAreAllFalse()
                .Where(noError => noError)
                .Throttle(TimeSpan.FromMilliseconds(10))    // マウス操作時に複数回変更が発生するので落ち着いたら流す
                .Subscribe(_ =>
                {
                    var x = double.Parse(PointRatioX.Value, CultureInfo.InvariantCulture) / 100d;
                    var y = double.Parse(PointRatioY.Value, CultureInfo.InvariantCulture) / 100d;
                    _model.PointRatio.Value = new RoixRatioXY(x, y);
                });
            #endregion

            #region Point
            MouseRightDownPoint
                .Where(point => point.IsNotZero)
                .Subscribe(borderPoint => _model.PointRatio.Value = borderPoint.ClipToBorder().ToRoixRatio());

            SelectedPoint = _model.PointRatio
                .CombineLatest(ViewBorderSize, (ratio, viewSize) => ratio * (RoixPoint)viewSize)
                .ToReadOnlyReactivePropertySlim();
            #endregion

        }
    }

    class ImageRatioModel : BindableBase
    {
        public IReactiveProperty<RoixRatioXYWH> RectRatio { get; } = new ReactivePropertySlim<RoixRatioXYWH>(initialValue: new(0.1, 0.2, 0.3, 0.4));
        public IReactiveProperty<RoixRatioXY> PointRatio { get; } = new ReactivePropertySlim<RoixRatioXY>(initialValue: new(0.5, 0.5));

        public ImageRatioModel()
        {
            //RectRatio.Subscribe(x => Debug.WriteLine($"{x:f2}"));
        }
    }

}
