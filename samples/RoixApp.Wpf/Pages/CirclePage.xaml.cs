using Microsoft.Xaml.Behaviors;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Roix.Wpf;
using Roix.Wpf.Extensions;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RoixApp.Wpf
{
    public partial class CirclePage : UserControl
    {
        public CirclePage()
        {
            InitializeComponent();
        }
    }

    public class CircleViewModel : BindableBase
    {
        CircleModel _model = new();

        public BitmapSource MyImage { get; } = App.Current.SourceImages.Image1;

        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseLeftUpPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        public IReadOnlyReactiveProperty<RoixCircle> PreviewCircle { get; }
        public IReadOnlyReactiveProperty<RoixCircle> SelectedCircle { get; }

        public CircleViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);
            var imageSourceSize = MyImage.ToRoixIntSize();

            // 画像座標系の選択枠(これを基準に管理する) マウス操作中に枠を更新 + 操作完了時に枠位置を通知する
            PreviewCircle = MouseMovePoint
                .Select(latestPoint => (startPoint: MouseLeftDownPoint.Value, latestPoint))
                .Where(x => x.startPoint != x.latestPoint)
                .SkipUntil(MouseLeftDownPoint.ToUnit())
                .TakeUntil(MouseLeftUpPoint.ToUnit())
                .Finally(() =>
                {
                    var (startPoint, latestPoint) = (MouseLeftDownPoint.Value, MouseLeftUpPoint.Value);
                    if (startPoint == default || latestPoint == default || startPoint == latestPoint) return;

                    var center = startPoint.Point;
                    var border = startPoint.Border;
                    var radius = center.GetDistance(latestPoint.Point);
                    var circle = new RoixCircle(center, radius);
                    _model.CircleRatio.Value = circle / border.Width;
                })
                .Repeat()
                .Select(x =>
                {
                    var center = x.startPoint.ConvertToNewBorderInt(imageSourceSize).ConvertToNewBorder(x.startPoint.Border);
                    var radius = x.startPoint.Point.GetDistance(x.latestPoint.Point);
                    return new RoixCircle(center.Point, radius);
                })
                .ToReadOnlyReactivePropertySlim();

            // View座標系の選択枠
            SelectedCircle = _model.CircleRatio
                .CombineLatest(ViewBorderSize, (circle, viewSize) => circle * viewSize.Width)
                .ToReadOnlyReactivePropertySlim();
        }
    }

    class CircleModel : BindableBase
    {
        public IReactiveProperty<RoixCircle> CircleRatio { get; } = new ReactivePropertySlim<RoixCircle>(RoixCircle.Zero);
    }

    public readonly struct RoixCircle
    {
        public static readonly RoixCircle Zero = new(RoixPoint.Zero, 0);

        public RoixPoint Point { get; init; }
        public double Radius { get; init; }
        public double X => Point.X;
        public double Y => Point.Y;

        public RoixCircle(RoixPoint point, double radius)
        {
            Point = point;
            Radius = radius;
        }

        public RoixCircle(double x, double y, double radius)
            : this(new(x, y), radius)
        { }

        public static RoixCircle operator *(in RoixCircle circle, double ratio)
        {
            var x = circle.X * ratio;
            var y = circle.Y * ratio;
            var radius = circle.Radius * ratio;
            return new(x, y, radius);
        }

        public static RoixCircle operator /(in RoixCircle circle, double ratio)
        {
            var x = circle.X / ratio;
            var y = circle.Y / ratio;
            var radius = circle.Radius / ratio;
            return new(x, y, radius);
        }
        public override string ToString() => $"{nameof(RoixCircle)} : ({Point.X}, {Point.Y}), {Radius}";
    }

    public sealed class RoixCircleToEllipseAction : TriggerAction<Ellipse>
    {
        protected override void Invoke(object parameter)
        {
            if (parameter is not DependencyPropertyChangedEventArgs args)
                return;

            if (args.NewValue is not RoixCircle circle)
                return;

            if (AssociatedObject is not Ellipse ellipse)
                return;

            var x = circle.X - circle.Radius;
            var y = circle.Y - circle.Radius;
            ellipse.Margin = new Thickness(x, y, 0, 0);
            ellipse.Width = ellipse.Height = circle.Radius * 2;     //直径

            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Top;
        }
    }
}
