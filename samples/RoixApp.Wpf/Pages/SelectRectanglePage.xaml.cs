﻿using Prism.Mvvm;
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
    public partial class SelectRectanglePage : UserControl
    {
        public SelectRectanglePage()
        {
            InitializeComponent();
        }
    }

    public class SelectRectangleViewModel : BindableBase
    {
        public static BitmapSource MyImage { get; } = App.Current.SourceImages.Image1;
        public IReactiveProperty<RoixBorderPoint> MouseLeftDownPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseLeftUpPoint { get; }
        public IReactiveProperty<RoixBorderPoint> MouseMovePoint { get; }
        public IReactiveProperty<RoixSize> ViewBorderSize { get; }

        public IReadOnlyReactiveProperty<RoixRect> SelectedRectangle { get; }
        public IReactiveProperty<RoixIntRect> SelectedRectangleToModel { get; }

        public SelectRectangleViewModel()
        {
            MouseLeftDownPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseLeftUpPoint = new ReactivePropertySlim<RoixBorderPoint>(mode: ReactivePropertyMode.None);
            MouseMovePoint = new ReactivePropertySlim<RoixBorderPoint>();
            ViewBorderSize = new ReactivePropertySlim<RoixSize>(mode: ReactivePropertyMode.DistinctUntilChanged);
            SelectedRectangleToModel = new ReactivePropertySlim<RoixIntRect>();

            var imageSourceSize = MyImage.ToRoixIntSize();

            // 画像座標系の選択枠(これを基準に管理する) マウス操作中に枠を更新 + 操作完了時に枠位置を通知する
            var selectedRectangleOnImage = MouseMovePoint
                .Select(latestPoint => (startPoint: MouseLeftDownPoint.Value, latestPoint))
                .Where(x => x.startPoint != x.latestPoint)
                .SkipUntil(MouseLeftDownPoint.ToUnit())
                .TakeUntil(MouseLeftUpPoint.ToUnit())
                .Finally(() =>
                {
                    var (startPoint, latestPoint) = (MouseLeftDownPoint.Value, MouseLeftUpPoint.Value);
                    if (startPoint == default || latestPoint == default || startPoint == latestPoint) return;

                    SelectedRectangleToModel.Value = RoixBorderIntRect.Create(startPoint, latestPoint, imageSourceSize).Roi;
                })
                .Repeat()
                .Select(x => RoixBorderIntRect.Create(x.startPoint, x.latestPoint, imageSourceSize))
                .ToReadOnlyReactivePropertySlim();

            // View座標系の選択枠
            SelectedRectangle = selectedRectangleOnImage
                .CombineLatest(ViewBorderSize, (rect, border) => rect.ConvertToNewBorder(border).Roi)
                .ToReadOnlyReactivePropertySlim();

        }
    }

}
