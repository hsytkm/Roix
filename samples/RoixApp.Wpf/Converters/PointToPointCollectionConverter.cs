using Roix.Wpf;
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
                _ => 1d,
            };
            if (point.IsZero) return null;

            var length = GetLength(parameter);
            return new PointCollection(new System.Windows.Point[]
            {
                point,
                point + new RoixVector(length, 0),
                point + new RoixVector(length, length),
                point + new RoixVector(0, length),
            });
        }

        public override RoixPoint ConvertBack(PointCollection? points, object parameter, CultureInfo culture)
            => default;
    }
}
