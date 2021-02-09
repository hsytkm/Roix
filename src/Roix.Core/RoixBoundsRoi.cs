using Roix.Core.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace Roix.Core
{
    public record RoixBoundsRoi<T>(RoixRect<T> Roi, RoixRect<T> Bounds) where T : struct, IComparable<T>
    {
        //private static readonly GenericOperation<T> _op = GenericOperation<T>.GetInstance();

        //public bool Contains() => Bounds.Contains(Roi);

        //public RoixRect<T> ClipToBounds() => Roi.ClipToBounds(Bounds);
    }

    static class RoixBoundsRoiExtension
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    }

    public record RoixBoundsRoiInt : RoixBoundsRoi<int>
    {
        public RoixBoundsRoiInt(RoixRectInt roi, RoixRectInt bounds) : base(roi, bounds) { }
        public RoixBoundsRoiInt(RoixRectInt roi, RoixSizeInt size) : base(roi, new RoixRectInt(size)) { }
    }

    public record RoixBoundsRoiDouble : RoixBoundsRoi<double>
    {
        public RoixBoundsRoiDouble(RoixRectDouble roi, RoixRectDouble bounds) : base(roi, bounds) { }
        public RoixBoundsRoiDouble(RoixRectDouble roi, RoixSizeDouble size) : base(roi, new RoixRectDouble(size)) { }

        public RoixRectDouble GetClippedRoi()
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static double GetSizeOffset(double value, double min, double max)
            {
                if (value < min) return value - min;
                if (value > max) return value - max;
                return 0;
            }

            var minReso = double.Epsilon;    // ◆int で 1 にするところを double.Epsilon にしてみてるけど自信ない。
            var x = MathExtension.Clamp(Roi.X, 0, Bounds.Width - minReso);
            var y = MathExtension.Clamp(Roi.Y, 0, Bounds.Height - minReso);
            var width = MathExtension.Clamp(Roi.Width + GetSizeOffset(Roi.X, 0, Bounds.Width - minReso), minReso, Bounds.Width - x);
            var height = MathExtension.Clamp(Roi.Height + GetSizeOffset(Roi.Y, 0, Bounds.Height - minReso), minReso, Bounds.Height - y);
            return new RoixRectDouble(x, y, width, height);
        }
    }

}
