using Roix.Wpf;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RoixApp.Wpf.Converters
{
    [ValueConversion(typeof(RoixLine), typeof(Geometry))]
    class ArrowLineConverter : GenericValueConverter<RoixLine, Geometry?>
    {
        const double ArrowAngle = 35d;
        const double ArrowLengthMinDefault = 20d;
        const double StartEllipseRadius = 3d;

        public override Geometry? Convert(RoixLine line, object parameter, CultureInfo culture)
        {
            if (line.IsSamePoints) return null;

            var arrowLength = Math.Min(line.GetDistance(), ArrowLengthMinDefault);
            var geometries = new Geometry[]
            {
                new LineGeometry(line.Point1, line.Point2),
                GetArrowPoint(line.Point1, line.Point2, arrowLength, ArrowAngle),
                GetArrowPoint(line.Point1, line.Point2, arrowLength, -ArrowAngle),
                new EllipseGeometry(line.Point1, StartEllipseRadius, StartEllipseRadius)
            };

            var group = new GeometryGroup();
            foreach (var geometry in geometries)
            {
                geometry.Freeze();
                group.Children.Add(geometry);
            }
            group.Freeze();
            return group;

            static LineGeometry GetArrowPoint(in RoixPoint point1, in RoixPoint point2, double arrowLength, double angle)
            {
                static double AngleToRadian(double angle) => angle * Math.PI / 180d;

                var radian = Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
                var d = radian + AngleToRadian(angle);
                var x = point2.X - arrowLength * Math.Cos(d);
                var y = point2.Y - arrowLength * Math.Sin(d);
                return new LineGeometry(point2, new Point(x, y));
            }
        }

        public override RoixLine ConvertBack(Geometry? points, object parameter, CultureInfo culture) => default;
    }
}
