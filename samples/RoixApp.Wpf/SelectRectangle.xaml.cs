﻿using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Roix.Wpf;
using RoixApp.Wpf.Extensions;
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
        public IReadOnlyReactiveProperty<RoixGaugeRect> SelectedRectangle { get; }
        public IReactiveProperty<RoixIntRect> SelectedRectangleToModel { get; }

        public IReactiveProperty<RoixGaugePoint> MouseRightDownPoint { get; }
        public IReadOnlyReactiveProperty<RoixGaugeRect> ClickedFixedGaugeRectangle { get; }
        public IReadOnlyReactiveProperty<RoixIntRect> ClickedFixedRectangleToModel { get; }

        public SelectRectangleViewModel()
        {
            var imageSourceSize = MyImage.ToRoixSize();
            MouseLeftDownPoint = new ReactivePropertySlim<RoixGaugePoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixGaugePoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixGaugePoint>();
            MouseRightDownPoint = new ReactivePropertySlim<RoixGaugePoint>(mode: ReactivePropertyMode.None);
            SelectedRectangleToModel = new ReactivePropertySlim<RoixIntRect>();

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
                    if (draggingVector.Value.IsZero) return;

                    // ドラッグ枠の確定
                    var gaugeRectOnView = MouseLeftDownPoint.Value.Add(draggingVector.Value);
                    var gaugeRectOnModel = gaugeRectOnView.ConvertToNewGauge(imageSourceSize);
                    SelectedRectangleToModel.Value = (RoixIntRect)gaugeRectOnModel.GetClippedGaugeRect().Roi;
                })
                .Repeat()
                .Subscribe(v => draggingVector.Value += v);

            // 選択枠のプレビュー
            SelectedRectangle = draggingVector
                .Where(vec => !vec.IsZero)
                .Select(vec => MouseLeftDownPoint.Value.Add(vec).GetClippedGaugeRect())
                .ToReadOnlyReactivePropertySlim();

            // 右クリックで固定サイズの枠を描画
            ClickedFixedGaugeRectangle = MouseRightDownPoint
                .Where(x => !x.Canvas.IsZero)
                .Select(gaugePoint =>
                {
                    var gaugeSizeOnModel = new RoixGaugeSize(new RoixSize(100, 100), imageSourceSize);
                    var canvasOnView = gaugePoint.Canvas;
                    var gaugeSizeOnView = gaugeSizeOnModel.ConvertToNewGauge(canvasOnView);
                    var newPointOnView = gaugePoint.Point - (RoixVector)(gaugeSizeOnView.Size / 2);
                    var gaugeRectOnView = new RoixGaugeRect(new RoixRect(newPointOnView, gaugeSizeOnView.Size), canvasOnView);
                    return gaugeRectOnView.GetClippedGaugeRect(isPointPriority: false);
                })
                .ToReadOnlyReactivePropertySlim();

            // Model通知用にView座標系から元画像の座標系に正規化
            ClickedFixedRectangleToModel = ClickedFixedGaugeRectangle
                .Where(gaugeRectOnView => !gaugeRectOnView.Canvas.IsZero)
                .Select(gaugeRectOnView => (RoixIntRect)gaugeRectOnView.ConvertToNewGauge(imageSourceSize).Roi)
                .ToReadOnlyReactivePropertySlim();

        }
    }

}
