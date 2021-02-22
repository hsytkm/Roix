using Roix.Wpf;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RoixApp.Wpf.Converters
{
    [ValueConversion(typeof(RoixRect), typeof(PointCollection))]
    class RectToPointCollectionConverter : GenericValueConverter<RoixRect, PointCollection?>
    {
        public override PointCollection? Convert(RoixRect rect, object parameter, CultureInfo culture)
            => (!rect.IsZero && !rect.IsEmpty) ? rect.ToPointCollection() : null;

        public override RoixRect ConvertBack(PointCollection? points, object parameter, CultureInfo culture)
            => RoixRect.Zero;
    }
}
