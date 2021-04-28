using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoixApp.Wpf
{
    public partial class SelectLinePage : UserControl
    {
        public SelectLinePage()
        {
            InitializeComponent();
        }
    }

    public class SelectLineViewModel : BindableBase
    {
        private readonly SelectLineModel _model = new();
        public BitmapSource MyImage => _model.MyImage;

        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseLeftUpPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        #region InputTexts
        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0, 100)]
        public ReactiveProperty<string> RectRatioX1 { get; }

        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0, 100)]
        public ReactiveProperty<string> RectRatioY1 { get; }

        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0, 100)]
        public ReactiveProperty<string> RectRatioX2 { get; }

        [Required(ErrorMessage = "Number is required")]
        [RegularExpression("^[0-9]+", ErrorMessage = "有効な数値を入力して下さい")]
        [Range(0, 100)]
        public ReactiveProperty<string> RectRatioY2 { get; }
        #endregion

        public IReactiveProperty<RoixBorderLine> SelectedLine { get; }

        public IReadOnlyReactiveProperty<string> PointsOnLine { get; }

        public SelectLineViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);
            SelectedLine = new ReactivePropertySlim<RoixBorderLine>(mode: ReactivePropertyMode.DistinctUntilChanged);
            var imageSourceSize = MyImage.ToRoixIntSize();

            #region InputTexts
            RectRatioX1 = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioX1);
            RectRatioY1 = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioY1);
            RectRatioX2 = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioX2);
            RectRatioY2 = new ReactiveProperty<string>(mode: ReactivePropertyMode.DistinctUntilChanged).SetValidateAttribute(() => RectRatioY2);

            // Modelからの値をViewに反映
            _model.Line
                .Subscribe(borderLine =>
                {
                    RectRatioX1.Value = borderLine.Line.X1.ToString();
                    RectRatioY1.Value = borderLine.Line.Y1.ToString();
                    RectRatioX2.Value = borderLine.Line.X2.ToString();
                    RectRatioY2.Value = borderLine.Line.Y2.ToString();
                });

            // 入力値から枠を作成（個々の入力値は検証してるけど、相関検証は未）
            new[] { RectRatioX1, RectRatioY1, RectRatioX2, RectRatioY2 }
                .Select(rp => rp.ObserveHasErrors)
                .CombineLatestValuesAreAllFalse()
                .Where(noError => noError)
                .Throttle(TimeSpan.FromMilliseconds(10))    // マウス操作時に複数回変更が発生するので落ち着いたら流す
                .Subscribe(_ =>
                {
                    var x1 = int.Parse(RectRatioX1.Value, CultureInfo.InvariantCulture);
                    var y1 = int.Parse(RectRatioY1.Value, CultureInfo.InvariantCulture);
                    var x2 = int.Parse(RectRatioX2.Value, CultureInfo.InvariantCulture);
                    var y2 = int.Parse(RectRatioY2.Value, CultureInfo.InvariantCulture);
                    _model.Line.Value = new RoixBorderIntLine(new RoixIntLine(x1, y1, x2, y2), imageSourceSize);
                });
            #endregion

            // 画像座標系の選択枠(これを基準に管理する) マウス操作中に枠を更新 + 操作完了時に枠位置を通知する
            MouseMovePoint
                .Select(latestPoint => (startPoint: MouseLeftDownPoint.Value, latestPoint))
                .Where(x => x.startPoint != x.latestPoint)
                .SkipUntil(MouseLeftDownPoint.ToUnit())
                .TakeUntil(MouseLeftUpPoint.ToUnit())
                .Finally(() =>
                {
                    var (startPoint, latestPoint) = (MouseLeftDownPoint.Value, MouseLeftUpPoint.Value);
                    if (startPoint == default || latestPoint == default || startPoint == latestPoint) return;

                    var borderLine = RoixBorderIntLine.Create(startPoint, latestPoint, imageSourceSize);
                    borderLine = borderLine.ClipToSize(borderLine.Border - 1);
                    if (borderLine.Line.IsSamePoints) return;

                    _model.Line.Value = borderLine;
                })
                .Repeat()
                .Subscribe(x =>
                {
                    var borderSize = x.startPoint.Border;
                    var halfPixelVector = GetViewHalfPixelVector(imageSourceSize, borderSize);
                    var imageLine = RoixBorderIntLine.Create(x.startPoint, x.latestPoint, imageSourceSize);
                    imageLine = imageLine.ClipToSize(imageLine.Border - 1);
                    var viewLine = imageLine.ConvertToNewBorder(borderSize);
                    SelectedLine.Value = new(viewLine.Line + halfPixelVector, viewLine.Border);
                });

            // Modelの値に応じて描画 + Viewサイズ変更に応じてコントロールを伸縮
            _model.Line
                .CombineLatest(ViewBorderSize.Where(x => x.IsNotZero), (borderLine, newBorder)
                    => borderLine.ConvertToNewBorder(newBorder))
                .Subscribe(viewBorderLine =>
                {
                    var halfPixelVector = GetViewHalfPixelVector(imageSourceSize, viewBorderLine.Border);
                    SelectedLine.Value = new(viewBorderLine.Line + halfPixelVector, viewBorderLine.Border);
                });

            // Line上のPointsを全表示
            PointsOnLine = _model.PointsOnLine
                .Where(x => x is not null)
                .Select(points => string.Join(Environment.NewLine, points.Select((p, i) => $"{i + 1}: ({p.X}, {p.Y})")) ?? "")
                .ToReadOnlyReactivePropertySlim<string>();

        }

        // View上の1画素の半分の移動量（左上角から中心までの移動量）
        private static RoixVector GetViewHalfPixelVector(in RoixIntSize imageSize, in RoixSize viewSize)
        {
            var onePixelOnImage = new RoixBorderIntSize(new RoixIntSize(1, 1), imageSize);
            var onePixelOnView = onePixelOnImage.ConvertToNewBorder(viewSize).Size;
            return (RoixVector)(onePixelOnView / 2d);
        }
    }

    class SelectLineModel : BindableBase
    {
        public BitmapSource MyImage { get; } = App.Current.SourceImages.Image16x16;
        public IReactiveProperty<RoixBorderIntLine> Line { get; }
        public IReadOnlyReactiveProperty<IReadOnlyList<RoixIntPoint>> PointsOnLine { get; }

        public SelectLineModel()
        {
            var imageSourceSize = MyImage.ToRoixIntSize();

            Line = new ReactivePropertySlim<RoixBorderIntLine>(initialValue: RoixIntLine.Zero.ToRoixBorder(imageSourceSize));

            PointsOnLine = Line
                .Where(borderLine => borderLine.Border == imageSourceSize)
                .Select(borderLine => borderLine.ClipToBorder().Line.GetIntPointsOnLine().ToArray())
                .ToReadOnlyReactiveProperty<IReadOnlyList<RoixIntPoint>>();
        }
    }
}
