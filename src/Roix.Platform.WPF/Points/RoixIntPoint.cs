using Roix.SourceGenerator;
using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.XYPair | RoixStructGeneratorOptions.TypeInt)]
    public readonly partial struct RoixIntPoint
    {
        readonly struct SourceValues
        {
            public readonly int X;
            public readonly int Y;
            public SourceValues(int x, int y) => (X, Y) = (x, y);
        }

        #region implicit
        public static implicit operator RoixPoint(in RoixIntPoint point) => new(point.X, point.Y);

        public static implicit operator System.Windows.Point(in RoixIntPoint point) => new(point.X, point.Y);
        #endregion

        #region explicit
        public static explicit operator RoixIntPoint(in RoixPoint point) => new(point.X.RoundToInt(), point.Y.RoundToInt());
        public static explicit operator RoixIntPoint(System.Windows.Point point) => new(point.X.RoundToInt(), point.Y.RoundToInt());
        #endregion

        #region operator
        #endregion

        #region Properties
        #endregion

    }
}
