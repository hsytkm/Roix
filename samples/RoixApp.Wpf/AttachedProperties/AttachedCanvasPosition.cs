using Roix.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RoixApp.Wpf.AttachedProperties
{
    static class AttachedCanvasPosition
    {
        public static readonly DependencyProperty LeftTopProperty =
            DependencyProperty.RegisterAttached("LeftTop", typeof(RoixPoint), typeof(AttachedCanvasPosition),
                new FrameworkPropertyMetadata(new RoixPoint(), (sender, e) => OnLeftTopPropertyChanged(sender, e.OldValue, e.NewValue)),
                new ValidateValueCallback(IsValidPoint));

        public static RoixPoint GetLeftTop(UIElement element) => (RoixPoint)element.GetValue(LeftTopProperty);
        public static void SetLeftTop(UIElement element, RoixPoint value) => element.SetValue(LeftTopProperty, value);

        // 検証コールバックによって false が返されると、値が設定されない
        private static bool IsValidPoint(object value) => value is RoixPoint p && !double.IsNaN(p.X) && !double.IsNaN(p.Y);

        private static void OnLeftTopPropertyChanged(DependencyObject sender, object oldValue, object newValue)
        {
            if (sender is not UIElement element) return;
            if (newValue is not RoixPoint leftTop) return;

            Canvas.SetLeft(element, leftTop.X);
            Canvas.SetTop(element, leftTop.Y);
        }
    }
}
