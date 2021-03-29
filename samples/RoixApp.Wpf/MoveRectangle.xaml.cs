using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RoixApp.Wpf
{
    public partial class MoveRectangle : UserControl
    {
        public MoveRectangle()
        {
            InitializeComponent();
        }
    }

    public class MoveRectangleViewModel : BindableBase
    {
        private readonly MoveRectangleModel _model = new();
        public BitmapSource MyImage => _model.MyImage;
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }
        public IReactiveProperty<RoixPoint> DragStartedPoint { get; }
        public IReactiveProperty<RoixVector> DragCompletedVector { get; }
        public IReactiveProperty<RoixVector> DraggingVector { get; }
        public IReactiveProperty<RoixBorderRect> SelectedRectangle { get; }

        public MoveRectangleViewModel()
        {
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);
            DragStartedPoint = new ReactivePropertySlim<RoixPoint>(mode: ReactivePropertyMode.None);
            DragCompletedVector = new ReactivePropertySlim<RoixVector>(mode: ReactivePropertyMode.None);
            DraggingVector = new ReactivePropertySlim<RoixVector>(mode: ReactivePropertyMode.None);
            SelectedRectangle = new ReactivePropertySlim<RoixBorderRect>();
            var imageSourceSize = MyImage.ToRoixIntSize();

            // Drag開始時の枠位置を記憶
            var dragStartedRect = DragStartedPoint
                .Select(_ => SelectedRectangle.Value)
                .ToReadOnlyReactivePropertySlim(mode: ReactivePropertyMode.None);

            // Drag中のView更新
            DraggingVector
                .SkipUntil(dragStartedRect.ToUnit())
                .TakeUntil(DragCompletedVector.ToUnit())
                .Repeat()
                .Where(vector => vector.IsNotZero)
                .Subscribe(vector =>
                {
                    // Modelの座標系からViewの座標系に戻す
                    SelectedRectangle.Value = CreateRectOnModel(dragStartedRect.Value, vector, imageSourceSize)
                        .ConvertToNewBorder(dragStartedRect.Value.Border);
                });

            // Drag終了時のModel通知
            DragCompletedVector
                .Where(vector => vector.IsNotZero)
                .Subscribe(vector => _model.Rect.Value = CreateRectOnModel(dragStartedRect.Value, vector, imageSourceSize));

            // Viewサイズ変化に追従
            _model.Rect
                .CombineLatest(ViewBorderSize.Where(x => x.IsNotZero), (rect, border) => rect.ConvertToNewBorder(border))
                .Subscribe(borderRect => SelectedRectangle.Value = borderRect);


            // Model座標系のRectを求める
            static RoixBorderIntRect CreateRectOnModel(in RoixBorderRect baseRect, in RoixVector vector, in RoixIntSize imageSize)
            {
                // Viewの座標系でDragを適用
                var draggedViewRect = (baseRect + vector).ClipToBorder();

                // Modelの座標系に変換
                return draggedViewRect.ConvertToRoixInt(imageSize, RoundingMode.Round);
            }
        }
    }

    class MoveRectangleModel : BindableBase
    {
        public BitmapSource MyImage { get; } = App.Current.SourceImages.Image16x16;
        public IReactiveProperty<RoixBorderIntRect> Rect { get; }

        public MoveRectangleModel()
        {
            var imageSourceSize = MyImage.ToRoixIntSize();
            var initialRect = new RoixIntRect(1, 2, 2, 2).ToRoixBorder(imageSourceSize);

            Rect = new ReactivePropertySlim<RoixBorderIntRect>(initialValue: initialRect);
            //Rect.Subscribe(x => Debug.WriteLine($"Model Rect: {x}"));
        }
    }

}
