using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    [SourceGenerator.RoixStructGenerator]
    public readonly partial struct RoixGaugePoint
    {
        readonly struct SourceValues
        {
            public readonly RoixPoint Point;
            public readonly RoixSize Border;
            public SourceValues(in RoixPoint point, in RoixSize border) => (Point, Border) = (point, border);
        }

        #region ctor
        public RoixGaugePoint(in RoixPoint point, in RoixSize border)
        {
            if (border.IsEmpty) throw new ArgumentException(ExceptionMessages.SizeIsEmpty);
            _values = new(point, border);
        }

        public RoixGaugePoint(double x, double y, double width, double height) => _values = new(new(x, y), new(width, height));
        #endregion

        #region implicit
        #endregion

        #region explicit
        //public static explicit operator RoixGaugeSize(in RoixGaugePoint gaugePoint) => new((RoixSize)gaugePoint.Point, gaugePoint.Border);
        //public static explicit operator RoixGaugeVector(in RoixGaugePoint gaugePoint) => new((RoixVector)gaugePoint.Point, gaugePoint.Border);
        #endregion

        #region operator
        public static RoixGaugePoint operator +(in RoixGaugePoint gaugePoint, in RoixGaugeVector gaugeVector) => (gaugePoint.Border == gaugeVector.Border) ? new(gaugePoint.Point + gaugeVector.Vector, gaugePoint.Border) : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);

        public static RoixGaugePoint operator -(in RoixGaugePoint gaugePoint, in RoixGaugeVector gaugeVector) => (gaugePoint.Border == gaugeVector.Border) ? new(gaugePoint.Point - gaugeVector.Vector, gaugePoint.Border) : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);

        public static RoixGaugeVector operator -(in RoixGaugePoint gaugePoint1, in RoixGaugePoint gaugePoint2) => (gaugePoint1.Border == gaugePoint2.Border) ? new(gaugePoint1.Point - gaugePoint2.Point, gaugePoint1.Border) : throw new NotImplementedException(ExceptionMessages.BorderSizeIsDifferent);
        #endregion

        #region Properties
        public bool IsInsideBorder => Point.X.IsInside(0, Border.Width) && Point.Y.IsInside(0, Border.Height);
        public bool IsOutsideBorder => !IsInsideBorder;
        public RoixPoint ClippedRoixPoint => new(Math.Clamp(Point.X, 0, Border.Width), Math.Clamp(Point.Y, 0, Border.Height));
        #endregion

        #region Methods
        public RoixGaugePoint ConvertToNewGauge(in RoixSize newBorder)
        {
            if (Border.IsInvalid) return this;
            if (newBorder.IsInvalid) throw new ArgumentException(ExceptionMessages.SizeIsInvalid);

            var newPoint = new RoixPoint(Point.X * newBorder.Width / Border.Width, Point.Y * newBorder.Height / Border.Height);
            return new(newPoint, newBorder);
        }

        public RoixIntPoint ToRoixIntPoint(bool isCheckBorder = true)
        {
            if (isCheckBorder && IsOutsideBorder) throw new InvalidOperationException(ExceptionMessages.MustInsideTheBorder);

            var srcPoint = (RoixIntPoint)Point;
            var intSize = (RoixIntSize)Border;
            if (intSize.IsZero) throw new InvalidOperationException(ExceptionMessages.SizeIsZero);

            var x = Math.Clamp(srcPoint.X, 0, intSize.Width - 1);
            var y = Math.Clamp(srcPoint.Y, 0, intSize.Height - 1);
            return new(x, y);
        }

        public RoixGaugeRect CreateRoixGaugeRect(in RoixVector vector) => new RoixGaugeRect(new RoixRect(Point, vector), Border);
        public RoixGaugeRect CreateRoixGaugeRect(in RoixGaugeVector gaugeVector)
        {
            if (Border != gaugeVector.Border) throw new ArgumentException(ExceptionMessages.BorderSizeIsDifferent);
            return CreateRoixGaugeRect(gaugeVector.Vector);
        }
        #endregion

    }
}
