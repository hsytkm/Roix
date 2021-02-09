using Microsoft.Xaml.Behaviors;
using Roix.Core;
using Roix.Wpf.Extensions;
using System;
using System.Windows;

namespace Roix.Wpf.Actions
{
    class EventToSizeAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty ControlSizeProperty
            = DependencyProperty.Register(nameof(ControlSize), typeof(RoixSizeDouble), typeof(EventToSizeAction));
        public RoixSizeDouble ControlSize
        {
            get => (RoixSizeDouble)GetValue(ControlSizeProperty);
            set => SetValue(ControlSizeProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (parameter is SizeChangedEventArgs e)
            {
                ControlSize = e.NewSize.ToRoixSizeDouble();
            }
            else if (AssociatedObject is FrameworkElement fe)
            {
                ControlSize = fe.ToRoixSizeDouble();
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
