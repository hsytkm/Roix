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
    public partial class ImageRatio : UserControl
    {
        public ImageRatio()
        {
            InitializeComponent();
        }
    }

    public class ImageRatioViewModel : BindableBase
    {
        private readonly ImageRatioModel _model = new();

        public BitmapSource MyImage { get; } = SelectRectangleViewModel.MyImage;
        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseLeftUpPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        #region InputTexts
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

        public IReadOnlyReactiveProperty<RoixRect> PreviewRectangle { get; }
        public IReadOnlyReactiveProperty<RoixRect> SelectedRectangle { get; }

        public ImageRatioViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);
            var imageSourceSize = MyImage.ToRoixIntSize();

            // 画像座標系の枠(これを基準に管理する)
            var selectedRectangleOnImage = new ReactivePropertySlim<RoixBorderIntRect>();

            #region InputTexts
            RectRatioX = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioX);
            RectRatioY = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioY);
            RectRatioWidth = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioWidth);
            RectRatioHeight = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioHeight);

            // Modelからの値をViewに反映
            _model.RectRatio.Subscribe(rect =>
                {
                    RectRatioX.Value = (rect.X * 100d).ToString("f2");
                    RectRatioY.Value = (rect.Y * 100d).ToString("f2");
                    RectRatioWidth.Value = (rect.Width * 100d).ToString("f2");
                    RectRatioHeight.Value = (rect.Height * 100d).ToString("f2");
                });

            // 入力値の検証
            var inputValuesHasNoError = new[] { RectRatioX.ObserveHasErrors, RectRatioY.ObserveHasErrors, RectRatioWidth.ObserveHasErrors, RectRatioHeight.ObserveHasErrors }
                .CombineLatestValuesAreAllFalse()
                .ToReadOnlyReactivePropertySlim(mode: ReactivePropertyMode.None);

            // 入力値から枠を作成（個々の入力値は検証してるけど、相関検証は未）
            var combinedRectRatio = inputValuesHasNoError
                .Where(noError => noError)
                .Subscribe(_ =>
                {
                    var x = double.Parse(RectRatioX.Value, CultureInfo.InvariantCulture) / 100d;
                    var y = double.Parse(RectRatioY.Value, CultureInfo.InvariantCulture) / 100d;
                    var width = double.Parse(RectRatioWidth.Value, CultureInfo.InvariantCulture) / 100d;
                    var height = double.Parse(RectRatioHeight.Value, CultureInfo.InvariantCulture) / 100d;
                    _model.RectRatio.Value = new RoixRatioXYWH(x, y, width, height);
                });
            #endregion

            // 画像座標系の選択枠(これを基準に管理する) マウス操作中に枠を更新 + 操作完了時に枠位置を通知する
            PreviewRectangle = MouseMovePoint
                .Select(latestPoint => (startPoint: MouseLeftDownPoint.Value, latestPoint))
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

            // View座標系の選択枠(length=0 だと View の Polygon が変に長くなるので minLength で Clip する)
            SelectedRectangle = _model.RectRatio
                .CombineLatest(ViewBorderSize, (rect, viewSize)
                    => (rect * viewSize).ClippedRoiSizeByMinimum(new RoixSize(1, 1)).Roi)
                .ToReadOnlyReactivePropertySlim();

        }
    }

    class ImageRatioModel : BindableBase
    {
        public IReactiveProperty<RoixRatioXYWH> RectRatio { get; } = new ReactiveProperty<RoixRatioXYWH>(initialValue: new(0.1, 0.2, 0.3, 0.4));

        public ImageRatioModel()
        {
            //RectRatio.Subscribe(x => Debug.WriteLine($"{x:f2}"));
        }
    }

}
