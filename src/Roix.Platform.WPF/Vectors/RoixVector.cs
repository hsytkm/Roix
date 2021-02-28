using System;

namespace Roix.Wpf
{
    // https://github.com/dotnet/wpf/blob/d49f8ddb889b5717437d03caa04d7c56819c16aa/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Vector.cs

    [SourceGenerator.RoixStructGenerator]
    public readonly partial struct RoixVector
    {
        readonly struct SourceValues
        {
            public readonly double X;
            public readonly double Y;
            public SourceValues(double x, double y) => (X, Y) = (x, y);
        }

        #region implicit
        public static implicit operator RoixVector(System.Windows.Vector vector) => new(vector.X, vector.Y);
        public static implicit operator System.Windows.Vector(in RoixVector vector) => new(vector.X, vector.Y);
        #endregion

        #region explicit
        public static explicit operator System.Windows.Point(in RoixVector vector) => new(vector.X, vector.Y);
        public static explicit operator RoixVector(System.Windows.Point point) => new(point.X, point.Y);

        public static explicit operator RoixPoint(in RoixVector vector) => new(vector.X, vector.Y);
        #endregion

        #region operator
        public static RoixVector operator +(in RoixVector vector1, in RoixVector vector2) => new(vector1.X + vector2.X, vector1.Y + vector2.Y);
        public static RoixVector operator -(in RoixVector vector) => Zero - vector;
        public static RoixVector operator -(in RoixVector vector1, in RoixVector vector2) => new(vector1.X - vector2.X, vector1.Y - vector2.Y);
        #endregion

        #region Properties
        #endregion

    }
}
