using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator(RoixStructGeneratorOptions.Validate)]
    public readonly partial struct RoixRatioXY
    {
        readonly struct SourceValues
        {
            public readonly double X;
            public readonly double Y;
            public SourceValues(double x, double y) => (X, Y) = (x, y);
        }

        #region ctor
        public RoixRatioXY(double d) : this(d, d) { }

        private partial void Validate(in RoixRatioXY ratio)
        {
            if (ratio.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.CannotBeNegativeValue);
        }
        #endregion

        public bool IsIncludeZero => X == 0 || Y == 0;
        public bool IsIncludeNegative => X < 0 || Y < 0;

        // double
        public static RoixRatioXY operator /(in RoixRatioXY ratio, double scalar)
        {
            if (scalar == 0) throw new DivideByZeroException();
            return new(ratio.X / scalar, ratio.Y / scalar);
        }

        public static RoixRatioXY operator /(double scalar, in RoixRatioXY ratio)
        {
            if (ratio.IsIncludeZero) throw new DivideByZeroException();
            return new(scalar / ratio.X, scalar / ratio.Y);
        }

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
