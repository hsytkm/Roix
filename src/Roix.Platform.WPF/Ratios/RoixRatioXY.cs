using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator]
    public readonly partial struct RoixRatioXY
    {
        readonly struct SourceValues
        {
            public readonly double X;
            public readonly double Y;
            public SourceValues(double x, double y) => (X, Y) = (x, y);
        }

        public RoixRatioXY(double d) => _values = new(d, d);

        public bool IsIncludeZero => X == 0 || Y == 0;
        public bool IsIncludeNegative => X < 0 || Y < 0;

    }
}
