using Roix.SourceGenerator;
using System;

namespace Roix.Wpf
{
    [RoixStructGenerator]
    public readonly partial struct RoixRatioXYWH
    {
        readonly struct SourceValues
        {
            public readonly RoixRatioXY PointRatio;
            public readonly RoixRatioXY SizeRatio;
            public SourceValues(in RoixRatioXY pointRatio, in RoixRatioXY sizeRatio) => (PointRatio, SizeRatio) = (pointRatio, sizeRatio);
        }

        public RoixRatioXYWH(double x, double y, double width, double height) : this(new(x, y), new(width, height)) { }

        public double X => PointRatio.X;
        public double Y => PointRatio.Y;
        public double Width => SizeRatio.X;
        public double Height => SizeRatio.Y;

        public bool IsIncludeZero => PointRatio.IsIncludeZero || SizeRatio.IsIncludeZero;
        public bool IsIncludeNegative => PointRatio.IsIncludeNegative || SizeRatio.IsIncludeNegative;

        // ◆これを使うより、変換前に Border で制限した方が良いので無効化
        //public RoixRatioXYWH ClipByPercent()
        //{
        //    var minLength = double.Epsilon;
        //    var x = Math.Clamp(X, 0, 1d - minLength);
        //    var y = Math.Clamp(Y, 0, 1d - minLength);
        //    // 負数側にめり込んでいた場合に、点を制限した分だけサイズを縮める
        //    var width = Math.Clamp(Width - (x - X), minLength, 1d - x);
        //    var height = Math.Clamp(Height - (y - Y), minLength, 1d - y);
        //    return new(x, y, width, height);
        //}

        // No Borders (double)
        public static RoixBorderRect operator *(in RoixRatioXYWH ratio, in RoixSize size)
        {
            if (size.IsIncludeNegative) throw new ArgumentException(ExceptionMessages.SizeIsNegative);
            var rect = new RoixRect(ratio.X * size.Width, ratio.Y * size.Height, ratio.Width * size.Width, ratio.Height * size.Height);
            return new(rect, size);
        }

        // No Borders (int)
        public static RoixBorderIntRect operator *(in RoixRatioXYWH ratio, in RoixIntSize size)
            => (ratio * (RoixSize)size).ToRoixInt(RoundingMode.Round);

    }
}
