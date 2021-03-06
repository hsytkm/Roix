using Roix.SourceGenerator;
using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Point.cs

    [RoixStructGenerator(RoixStructGeneratorOptions.ArithmeticOperator2)]
    public readonly partial struct RoixPoint
    {
        readonly struct SourceValues
        {
            public readonly double X;
            public readonly double Y;
            public SourceValues(double x, double y) => (X, Y) = (x, y);
        }

        #region implicit
        public static implicit operator RoixPoint(System.Windows.Point point) => new(point.X, point.Y);
        public static implicit operator System.Windows.Point(in RoixPoint point) => new(point.X, point.Y);
        #endregion

        #region explicit
        public static explicit operator RoixSize(in RoixPoint point) => new(point.X, point.Y);
        public static explicit operator RoixVector(in RoixPoint point) => new(point.X, point.Y);
        #endregion

        #region operator
        public static RoixPoint operator +(in RoixPoint point, in RoixVector vector) => new(point.X + vector.X, point.Y + vector.Y);
        public static RoixVector operator -(in RoixPoint point1, in RoixPoint point2) => new(point1.X - point2.X, point1.Y - point2.Y);
        public static RoixPoint operator -(in RoixPoint point, in RoixVector vector) => new(point.X - vector.X, point.Y - vector.Y);
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion

    }
}
