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

        public RoixRatioXY(double d) : this(d, d) { }

        public bool IsIncludeZero => X == 0 || Y == 0;
        public bool IsIncludeNegative => X < 0 || Y < 0;

        // No Borders
        public static RoixPoint operator *(in RoixRatioXY ratio, in RoixPoint point) => new(point.X * ratio.X, point.Y * ratio.Y);
        public static RoixRect operator *(in RoixRatioXY ratio, in RoixRect rect) => new(rect.X * ratio.X, rect.Y * ratio.Y, rect.Width * ratio.X, rect.Height * ratio.Y);
        public static RoixSize operator *(in RoixRatioXY ratio, in RoixSize size)
        {
            if (size.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
            return new(size.Width * ratio.X, size.Height * ratio.Y);
        }
        public static RoixVector operator *(in RoixRatioXY ratio, in RoixVector vector) => new(vector.X * ratio.X, vector.Y * ratio.Y);

        // Borders
        public static RoixBorderPoint operator *(in RoixRatioXY ratio, in RoixBorderPoint borderPoint) => new(borderPoint.Point * ratio, borderPoint.Border);
        public static RoixBorderRect operator *(in RoixRatioXY ratio, in RoixBorderRect borderRect) => new(borderRect.Roi * ratio, borderRect.Border);
        public static RoixBorderSize operator *(in RoixRatioXY ratio, in RoixBorderSize borderSize) => new(borderSize.Size * ratio, borderSize.Border);
        public static RoixBorderVector operator *(in RoixRatioXY ratio, in RoixBorderVector borderVector) => new(borderVector.Vector * ratio, borderVector.Border);

    }
}
