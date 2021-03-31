using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RoixApp.Wpf
{
    public partial class DraggableRect : UserControl
    {
        public static readonly DependencyProperty BorderRectProperty =
            DependencyProperty.Register(nameof(BorderRect), typeof(RoixBorderRect), typeof(DraggableRect));
        public RoixBorderRect BorderRect
        {
            get => (RoixBorderRect)GetValue(BorderRectProperty);
            set => SetValue(BorderRectProperty, value);
        }

        public static readonly DependencyProperty ImageSourceSizeProperty =
            DependencyProperty.Register(nameof(ImageSourceSize), typeof(RoixIntSize), typeof(DraggableRect));
        public RoixIntSize ImageSourceSize
        {
            get => (RoixIntSize)GetValue(ImageSourceSizeProperty);
            set => SetValue(ImageSourceSizeProperty, value);
        }

        // ドラッグ開始時の Rect を記憶（ここを基準にドラッグを計算）
        private RoixBorderRect _dragStartRectBuf;

        public DraggableRect()
        {
            InitializeComponent();

            var cornerThumbs = new[] { cornerThumb0, cornerThumb1, cornerThumb2, cornerThumb3 };
            foreach (var thumb in cornerThumbs)
            {
                thumb.DragStarted += Thumb_DragStarted;
                thumb.DragDelta += CornerThumb_DragDelta;
            }
        }

        // ドラッグ開始時の Rect を記録する
        private void Thumb_DragStarted(object sender, DragStartedEventArgs e) => _dragStartRectBuf = BorderRect;

        // 枠自体のドラッグ移動
        private void RectThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var vector = e.ToRoixVector();
            var clippedRect = (_dragStartRectBuf + vector).ClipToBorder();
            var adjustedRect = clippedRect.ConvertToRoixInt(ImageSourceSize, RoundingMode.Round).ConvertToNewBorder(_dragStartRectBuf.Border);
            BorderRect = adjustedRect;
        }

        // コーナー操作時の枠サイズ変更
        private void CornerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is not Thumb cornerThumb) return;

            var position = AttachedCornerPosition.GetCornerPos(cornerThumb);
            if (position is AttachedCornerPosition.CornerPosition.None) return;

            var vector = e.ToRoixVector();
            var clippedRect = GetResizedBorderRect(_dragStartRectBuf, vector, position).ClipToBorder();
            var modelRect = clippedRect.ConvertToRoixInt(ImageSourceSize, RoundingMode.Round);
            //modelRect = modelRect.ClippedSizeByMinimum(new RoixIntSize(1, 1));    // 1x1サイズで制限
            var adjustedRect = modelRect.ConvertToNewBorder(_dragStartRectBuf.Border);
            BorderRect = adjustedRect;
        }

        // コーナー位置に応じて Vector を Rect に適用する
        private static RoixBorderRect GetResizedBorderRect(in RoixBorderRect borderRect, in RoixVector vector, AttachedCornerPosition.CornerPosition position)
        {
            var topLeftVector = position switch
            {
                AttachedCornerPosition.CornerPosition.TopLeft => vector,
                AttachedCornerPosition.CornerPosition.TopRight => new RoixVector(0, vector.Y),
                AttachedCornerPosition.CornerPosition.BottomRight => RoixVector.Zero,
                AttachedCornerPosition.CornerPosition.BottomLeft => new RoixVector(vector.X, 0),
                _ => throw new NotImplementedException()
            };
            var newTopLeft = borderRect.Roi.TopLeft + topLeftVector;

            var bottomRightVector = position switch
            {
                AttachedCornerPosition.CornerPosition.TopLeft => RoixVector.Zero,
                AttachedCornerPosition.CornerPosition.TopRight => new RoixVector(vector.X, 0),
                AttachedCornerPosition.CornerPosition.BottomRight => vector,
                AttachedCornerPosition.CornerPosition.BottomLeft => new RoixVector(0, vector.Y),
                _ => throw new NotImplementedException()
            };
            var newBottomRight = borderRect.Roi.BottomRight + bottomRightVector;

            var rect = new RoixRect(newTopLeft, newBottomRight);
            var clippedRect = rect.GetClippedRect(borderRect.Border);
            return clippedRect.ToRoixBorder(borderRect.Border);
        }

    }

    static class AttachedCornerPosition
    {
        public enum CornerPosition { None, TopLeft, TopRight, BottomRight, BottomLeft }

        public static readonly DependencyProperty CornerPosProperty =
            DependencyProperty.RegisterAttached("CornerPos", typeof(CornerPosition), typeof(AttachedCornerPosition));

        public static CornerPosition GetCornerPos(UIElement element) => (CornerPosition)element.GetValue(CornerPosProperty);
        public static void SetCornerPos(UIElement element, CornerPosition value) => element.SetValue(CornerPosProperty, value);
    }
}
