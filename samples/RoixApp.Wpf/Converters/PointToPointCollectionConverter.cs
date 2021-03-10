using Roix.Wpf;
using Roix.Wpf.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RoixApp.Wpf.Converters
{
    [ValueConversion(typeof(RoixPoint), typeof(PointCollection))]
    class PointToPointCollectionConverter : GenericValueConverter<RoixPoint, PointCollection?>
    {
        public override PointCollection? Convert(RoixPoint point, object parameter, CultureInfo culture)
        {
            static double GetLength(object param) => param switch
            {
                double d => d,
                int i => i,
                _ => double.Epsilon
            };
            if (point.IsZero) return null;

            var length = GetLength(parameter);
            var rect = new RoixRect(point, new RoixSize(length, length));
            return rect.ToPointCollection();
        }

        public override RoixPoint ConvertBack(PointCollection? points, object parameter, CultureInfo culture)
            => default;
    }
}
