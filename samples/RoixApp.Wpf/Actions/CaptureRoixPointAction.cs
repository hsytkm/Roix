using Microsoft.Xaml.Behaviors;
using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace RoixApp.Wpf.Actions
{
    class CaptureRoixPointAction : TriggerAction<Thumb>
    {
        public static readonly DependencyProperty PointProperty =
            DependencyProperty.Register(nameof(Point), typeof(RoixPoint), typeof(CaptureRoixPointAction));
        public RoixPoint Point
        {
            get => (RoixPoint)GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }

        protected override void Invoke(object parameter) => Point = parameter switch
        {
            DragStartedEventArgs e => e.ToRoixPoint(),
            _ => RoixPoint.Zero,
        };
    }
}
