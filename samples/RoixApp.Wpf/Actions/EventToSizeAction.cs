using Microsoft.Xaml.Behaviors;
using Roix.Wpf;
using System;
using System.Windows;

namespace RoixApp.Wpf.Actions
{
    class EventToSizeAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty ControlSizeProperty
            = DependencyProperty.Register(nameof(ControlSize), typeof(RoixSize), typeof(EventToSizeAction));
        public RoixSize ControlSize
        {
            get => (RoixSize)GetValue(ControlSizeProperty);
            set => SetValue(ControlSizeProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (parameter is SizeChangedEventArgs e)
            {
                ControlSize = e.NewSize;
            }
            else if (AssociatedObject is FrameworkElement fe)
            {
                ControlSize = new(fe.ActualWidth, fe.ActualHeight);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
