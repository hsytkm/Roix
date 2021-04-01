using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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

            rectThumb.DragStarted += RectThumb_DragStarted;
            rectThumb.DragDelta += RectThumb_DragDelta;
            rectThumb.DragCompleted += Thumb_DragCompleted;

            var cornerThumbs = new[] { cornerThumb0, cornerThumb1, cornerThumb2, cornerThumb3 };
            foreach (var thumb in cornerThumbs)
            {
                thumb.DragStarted += CornerThumb_DragStarted;
                thumb.DragDelta += CornerThumb_DragDelta;
                thumb.DragCompleted += Thumb_DragCompleted;
            }
        }

        #region RectThumb
        // ドラッグ開始時の Rect を記録する
        private void RectThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (sender is not Thumb thumb) return;
            thumb.Cursor = Cursors.ScrollAll;
            _dragStartRectBuf = BorderRect;
        }

        // 枠自体のドラッグ移動
        private void RectThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var vector = e.ToRoixVector();
            var clippedRect = (_dragStartRectBuf + vector).ClipToBorder();
            var adjustedRect = clippedRect.ConvertToNewBorderInt(ImageSourceSize, RoundingMode.Round).ConvertToNewBorder(_dragStartRectBuf.Border);
            BorderRect = adjustedRect;
        }
        #endregion

        #region CornerThumbs
        // ドラッグ開始時の Rect を記録する
        private void CornerThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (sender is not Thumb cornerThumb) return;
            cornerThumb.Cursor = AttachedCornerPosition.GetCursor(cornerThumb);
            _dragStartRectBuf = BorderRect;
        }

        // コーナー操作時の枠サイズ変更
        private void CornerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is not Thumb cornerThumb) return;

            var position = AttachedCornerPosition.GetCornerPos(cornerThumb);
            if (position is AttachedCornerPosition.CornerPosition.None) return;

            var vector = e.ToRoixVector();
            var clippedModelRect = GetResizedBorderIntRect(_dragStartRectBuf, vector, ImageSourceSize, position).ClipToBorder();
            var adjustedRect = clippedModelRect.ConvertToNewBorder(_dragStartRectBuf.Border);
            BorderRect = adjustedRect;

            // コーナー位置に応じて Vector を Rect に適用する（画像の座標系で管理するのがポイント）
            static RoixBorderIntRect GetResizedBorderIntRect(in RoixBorderRect borderRect, in RoixVector vector, in RoixIntSize imageSize, AttachedCornerPosition.CornerPosition position)
            {
                var viewBorder = borderRect.Border;

                var topLeftVector = position switch
                {
                    AttachedCornerPosition.CornerPosition.TopLeft => vector,
                    AttachedCornerPosition.CornerPosition.TopRight => new RoixVector(0, vector.Y),
                    AttachedCornerPosition.CornerPosition.BottomRight => RoixVector.Zero,
                    AttachedCornerPosition.CornerPosition.BottomLeft => new RoixVector(vector.X, 0),
                    _ => throw new NotImplementedException()
                };
                var newBorderTopLeft = (borderRect.Roi.TopLeft + topLeftVector).ToRoixBorder(viewBorder);

                var bottomRightVector = position switch
                {
                    AttachedCornerPosition.CornerPosition.TopLeft => RoixVector.Zero,
                    AttachedCornerPosition.CornerPosition.TopRight => new RoixVector(vector.X, 0),
                    AttachedCornerPosition.CornerPosition.BottomRight => vector,
                    AttachedCornerPosition.CornerPosition.BottomLeft => new RoixVector(0, vector.Y),
                    _ => throw new NotImplementedException()
                };
                var newBorderBottomRight = (borderRect.Roi.BottomRight + bottomRightVector).ToRoixBorder(viewBorder);

                var newBorderIntTopLeft = newBorderTopLeft.ConvertToNewBorderInt(imageSize, RoundingMode.Round);
                var newBorderIntBottomRight = newBorderBottomRight.ConvertToNewBorderInt(imageSize, RoundingMode.Round);
                var modelRect = new RoixBorderIntRect(newBorderIntTopLeft, newBorderIntBottomRight);
                return modelRect.ClipToBorder();
            }
        }
        #endregion

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (sender is not Thumb thumb) return;
            thumb.Cursor = Cursors.Arrow;
        }
    }

    static class AttachedCornerPosition
    {
        public enum CornerPosition { None, TopLeft, TopRight, BottomRight, BottomLeft }

        public static readonly DependencyProperty CornerPosProperty =
            DependencyProperty.RegisterAttached("CornerPos", typeof(CornerPosition), typeof(AttachedCornerPosition));

        public static CornerPosition GetCornerPos(UIElement element) => (CornerPosition)element.GetValue(CornerPosProperty);
        public static void SetCornerPos(UIElement element, CornerPosition value) => element.SetValue(CornerPosProperty, value);

        private static Cursor GetCursor(in CornerPosition position) => position switch
        {
            CornerPosition.TopLeft => Cursors.SizeNWSE,
            CornerPosition.TopRight => Cursors.SizeNESW,
            CornerPosition.BottomRight => Cursors.SizeNWSE,
            CornerPosition.BottomLeft => Cursors.SizeNESW,
            _ => throw new NotImplementedException()
        };

        public static Cursor GetCursor(UIElement element) => GetCursor(GetCornerPos(element));
    }
}
