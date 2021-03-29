using Microsoft.Xaml.Behaviors;
using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace RoixApp.Wpf.Actions
{
    class CaptureRoixVectorAction : TriggerAction<Thumb>
    {
        public static readonly DependencyProperty VectorProperty =
            DependencyProperty.Register(nameof(Vector), typeof(RoixVector), typeof(CaptureRoixVectorAction));
        public RoixVector Vector
        {
            get => (RoixVector)GetValue(VectorProperty);
            set => SetValue(VectorProperty, value);
        }

        protected override void Invoke(object parameter) => Vector = parameter switch
        {
            DragDeltaEventArgs e => e.ToRoixVector(),
            DragCompletedEventArgs e => e.ToRoixVector(),
            _ => RoixVector.Zero,
        };
    }
}
