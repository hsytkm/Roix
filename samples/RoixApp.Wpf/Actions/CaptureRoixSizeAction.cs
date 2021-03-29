using Microsoft.Xaml.Behaviors;
using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Windows;

namespace RoixApp.Wpf.Actions
{
    class CaptureRoixSizeAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(RoixSize), typeof(CaptureRoixSizeAction));
        public RoixSize Size
        {
            get => (RoixSize)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        protected override void Invoke(object parameter) => Size = parameter switch
        {
            SizeChangedEventArgs e => e.NewSize,
            _ => AssociatedObject.ToRoixSize(),
        };
    }
}
