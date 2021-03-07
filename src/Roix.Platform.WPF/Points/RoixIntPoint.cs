using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.ArithmeticOperator2)]
    public readonly partial struct RoixIntPoint
    {
        readonly struct SourceValues
        {
            public readonly int X;
            public readonly int Y;
            public SourceValues(int x, int y) => (X, Y) = (x, y);
        }

        #region ctor
        public RoixIntPoint(double x, double y) => _values = new(x.FloorToInt(), y.FloorToInt());
        #endregion

        #region implicit
        public static implicit operator RoixPoint(in RoixIntPoint point) => new(point.X, point.Y);

        public static implicit operator System.Windows.Point(in RoixIntPoint point) => new(point.X, point.Y);
        #endregion

        #region explicit
        public static explicit operator RoixIntPoint(in RoixPoint point) => new(point.X.FloorToInt(), point.Y.FloorToInt());
        public static explicit operator RoixIntPoint(System.Windows.Point point) => new(point.X.FloorToInt(), point.Y.FloorToInt());
        #endregion

        #region operator
        public static RoixIntPoint operator +(in RoixIntPoint point, in RoixIntVector vector) => new(point.X + vector.X, point.Y + vector.Y);
        public static RoixIntVector operator -(in RoixIntPoint point1, in RoixIntPoint point2) => new(point1.X - point2.X, point1.Y - point2.Y);
        public static RoixIntPoint operator -(in RoixIntPoint point, in RoixIntVector vector) => new(point.X - vector.X, point.Y - vector.Y);
        #endregion

        #region Properties

        /// <summary>引数で指定した座標系(int)に変換する</summary>
        public static RoixIntPoint Create(in RoixPoint srcPoint, in RoixSize srcSize, in RoixIntSize destSize, RoundingMode mode)
        {
            var point = srcPoint * (destSize / srcSize);
            return new RoixIntPoint(point.X.ToInt(mode), point.Y.ToInt(mode));
        }

        public RoixIntPoint GetClippedIntPoint(in RoixIntSize size)
        {
            var x = Math.Clamp(X, 0, size.Width - 1);
            var y = Math.Clamp(Y, 0, size.Height - 1);
            return new RoixIntPoint(x, y);
        }

        #endregion

    }
}
