using System;
using System.Windows.Controls.Primitives;

namespace Roix.Wpf.Extensions
{
    public static class RoixPointExtension
    {
        public static RoixPoint ToRoixPoint(this DragStartedEventArgs e) => new(e.HorizontalOffset, e.VerticalOffset);

    }
}
