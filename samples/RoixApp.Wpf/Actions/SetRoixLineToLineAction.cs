using Microsoft.Xaml.Behaviors;
using Roix.Wpf;
using System;
using System.Windows;
using System.Windows.Shapes;

namespace RoixApp.Wpf.Actions
{
    class SetRoixLineToLineAction : TargetedTriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            if (this.Target is not Line line) return;
            if (parameter is not DependencyPropertyChangedEventArgs e) return;

            RoixLine value = e.NewValue switch
            {
                RoixLine l => l,
                RoixIntLine il => il,
                RoixBorderLine bl => bl.Line,
                RoixBorderIntLine bil => bil.Line,
                _ => throw new NotSupportedException()
            };

            line.X1 = value.X1;
            line.X2 = value.X2;
            line.Y1 = value.Y1;
            line.Y2 = value.Y2;
        }
    }
}
