using System;

namespace Roix.Wpf.Extensions
{
    public static class RoixPointExtension
    {
        /// <summary>2点の距離を計算します</summary>
        public static double GetDistance(in this RoixPoint point1, in RoixPoint point2)
        {
            // ◆Point構造体内に定義した方が良いような気がする…
            var x = point2.X - point1.X;
            var y = point2.Y - point1.Y;
            return Math.Sqrt(x * x + y * y);
        }

    }
}
