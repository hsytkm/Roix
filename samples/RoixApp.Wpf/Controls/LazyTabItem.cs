using System;
using System.Windows;
using System.Windows.Controls;

namespace RoixApp.Wpf.Controls
{
    sealed class LazyTabItem : TabItem
    {
        public Type? ContentType { get; set; }

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (Content is IDisposable d) d.Dispose();
            Content = null;

            if (ContentType is not null)
                Content = Activator.CreateInstance(ContentType);

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            if (Content is IDisposable d) d.Dispose();
            Content = null;

            base.OnUnselected(e);
        }
    }
}
