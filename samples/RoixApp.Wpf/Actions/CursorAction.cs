using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;

namespace RoixApp.Wpf.Actions
{
    class CursorArrowAction : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is not FrameworkElement fe) return;
            fe.Cursor = Cursors.Arrow;
        }
    }

    class CursorHandAction : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is not FrameworkElement fe) return;
            fe.Cursor = Cursors.Hand;
        }
    }

    class CursorScrollAllAction : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is not FrameworkElement fe) return;
            fe.Cursor = Cursors.ScrollAll;
        }
    }
}
