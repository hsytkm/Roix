using Roix.Wpf.Internals;
using System;

namespace Roix.Wpf
{
    [SourceGenerator.RoixStructGenerator]
    public readonly partial struct RoixIntPoint
    {
        readonly struct SourceValues
        {
            public readonly int X;
            public readonly int Y;
            public SourceValues(int x, int y) => (X, Y) = (x, y);
        }

        #region implicit
        public static implicit operator RoixIntPoint(in RoixPoint point) => new(point.X.RoundToInt(), point.Y.RoundToInt());
        public static implicit operator RoixPoint(in RoixIntPoint point) => new(point.X, point.Y);

        public static implicit operator RoixIntPoint(System.Windows.Point point) => new(point.X.RoundToInt(), point.Y.RoundToInt());
        public static implicit operator System.Windows.Point(in RoixIntPoint point) => new(point.X, point.Y);
        #endregion

        #region explicit
        #endregion

        #region operator
        #endregion

        #region Properties
        #endregion

    }
}
