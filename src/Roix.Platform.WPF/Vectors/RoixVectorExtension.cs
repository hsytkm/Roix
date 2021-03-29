using System;
using System.Windows.Controls.Primitives;

namespace Roix.Wpf.Extensions
{
    public static class RoixVectorExtension
    {
        public static RoixVector ToRoixVector(this DragDeltaEventArgs e) => new(e.HorizontalChange, e.VerticalChange);
        public static RoixVector ToRoixVector(this DragCompletedEventArgs e) => new(e.HorizontalChange, e.VerticalChange);
        
    }
}
