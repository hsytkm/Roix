using Roix.Core;
using Roix.Wpf.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Roix.Wpf.Converters
{
    [ValueConversion(typeof(RoixRectDouble), typeof(PointCollection))]
    class RectToPointCollectionConverter : GenericValueConverter<RoixRectDouble, PointCollection>
    {
        public override PointCollection Convert(RoixRectDouble rect, object parameter, CultureInfo culture)
            => rect.ToPointCollection();

        public override RoixRectDouble ConvertBack(PointCollection points, object parameter, CultureInfo culture)
            => RoixRectDouble.Zero;
    }
}